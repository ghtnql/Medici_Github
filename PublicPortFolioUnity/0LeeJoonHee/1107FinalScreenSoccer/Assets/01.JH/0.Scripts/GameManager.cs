using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingleToneMonoBehaviour<GameManager>
{
    #region 인트로 관련변수
    
    //by준희, 게임 첫 플레이 할 시 인트로 씬용 카메라(애니메이션 발동)
    public GameObject overViewCamera;   

    Animator overView;
    RuntimeAnimatorController overViewAnimCtr;


    //by준희, 인트로씬의 시간제어 (CountIntroAnimSeconds()에서 자동설정받음)
    //인트로씬이 끝난 후 바로 플레이가 될 수 있도록 해줌
    float overViewTime;

    //by준희,인트로가 끝났는지 체크하는 불값
    static bool penaltyKickIntroEnd = false;   

    #endregion

    #region 델리게이트

    //by준희, 센서로부터 값이 넘어와 게임 플레이의 시작을 알리는 델리게이트
    public delegate void StartBallDel();
    public static event StartBallDel StartBallEvent;

    //by준희, 스크린에 골여부, 볼 속도 등을 표시해주는 델리게이트
    public delegate void CheckBall();
    public static CheckBall InvokeShowGoalCheckInScreen;

    //by준희, 골인지 노골인지에 따라 플레이어의 세레모니를 결정하는 델리게이트
    public delegate void CheckCeremony();
    public static CheckCeremony Ceremony;

    //by준희, 골의 여부에 따라 스크린 왼쪽 상단의 골 체크 이미지의 색을 변경하는 델리게이트
    public delegate void CheckBallColor();
    public static CheckBallColor BallColorInScrMGR;

    //by준희, 플레이어가 볼을 찬 후 카메라가 플레이어쪽으로 돌아 오게하는 델리게이트
    public delegate void TimeFailZoomOut();
    public static TimeFailZoomOut ZoomOut;

    //by준희, FireBall Effect
    public delegate void Effect(bool effect);
    public static Effect FireBall;

    //by준희, Itween Move UI
    public delegate void Tween(GameObject gameObject);
    public static Tween TweenUI;

    public delegate void GK(float x, float y);
    public static GK KeepPos;

    #endregion


    #region 게임제어변수

    //by준희, StartBallEvent델리게이트를 실행을 제어하는 값(true이어야 게임 플레이 가능)
    //true값 조건 : 
    //false값 조건 :
    public static bool actionAble = false;

    bool actionableTest =true;

    //by준희, 카운트다운하여 waitingTime안에 플레이하지 않을 시 노골로 간주
    int waitingTime = 10;

    //by준희, 카운트 다운하는 텍스트를 스크린 우측에 보여줌
    Text CountDownGoalWait;

    //by준희, CountDownGoalWait 텍스트의 알파값 제어
    CanvasGroup countDownShowGoalWait;

    //by준희, 골여부에 따라 플레이어 애니메이션, 왼쪽 상단의 골 판정 이미지 색상을 변경
    public static bool isGoal;

    #region 골여부 텍스트 또는 이미지로 보여주기 결정
    //by준희, 골여부에 따라 스크린에 골, 노골을 텍스트로 보여줌
    Text goalCheckTEXT;     //by준희, 텍스트로 골 노골 쓸 때 쓰기

    GameObject NogoalImg;   
    GameObject GoalImg;

    //by준희, goallCheckTEXT의 알파값을 제어(골,노골여부 보여줌)
    CanvasGroup goalTextShow;   //by준희, 텍스트로 골 노골 쓸 때 쓰기

    CanvasGroup NogoalShow;
    CanvasGroup GoalShow;
    #endregion

    //by준희, PNC센서로부터 볼 속도 값을 받음
    Text goalSpeed;

    //by준희, goalSpeed의 알파값을 제어
    CanvasGroup goalSpeedShow;

    //by준희, 공이 생성 된후 아래의 초가 지났을 때 골 체크 구간에 없으면 노골 처리
    float offGoalCheckTxtTime =5;
    
    //by준희, 골체크 구간인지 아닌지를 체크.
    public static bool stillOnGround;

    //by준희, 몇번플레이 했는지 알려주는 값(왼쪽 상단의 골여부 이미지를 선택해줌, 
    //5번 다 차면 게임을 순환 할 수 있도록 하게 함, 0번부터 공차기 시작)
    public static int BallCOUNTinSCORE = -1 ;

    //by준희, Effect관련
    public static bool fireBall = false;    //파이어볼 제어변수
    public GameObject congratEffect;        //골들어갔을 때 Instantiate
    public Transform GoalEffectPos;         //congratEffect위치
    GameObject EffectParticle;

#endregion
    private void Awake()
    {
        //by준희, 인트로 카메라를 컴파일 전에 기능을 끈다.
        overViewCamera.SetActive(false);
        //by준희, 컴파일 전 아래의 컴포넌트들을 찾아 가져온다.
        //goalCheckTEXT = GameObject.Find("goalCheckTEXT").GetComponent<Text>();

        NogoalImg = GameObject.Find("NoGoalImage");
        GoalImg = GameObject.Find("GoalImage");

        CountDownGoalWait = GameObject.Find("CountDown").GetComponent<Text>();
        goalSpeed = GameObject.Find("CurrentSpeed").GetComponent<Text>();
        
        overView = overViewCamera.GetComponent<Animator>();
    }

    private void Start()    
    {
        //by준희, 게임 플레이를 처음 하면 인트로씬 보여주기(카메라 이동)
        StartCoroutine(IStartIntro()); 

        //goalTextShow = goalCheckTEXT.GetComponent<CanvasGroup>();   ////by준희, 텍스트로 골 노골 쓸 때 쓰기

        //by 준희, 골,노골 이미지로 보여주기.
        NogoalShow = NogoalImg.GetComponent<CanvasGroup>();
        GoalShow = GoalImg.GetComponent<CanvasGroup>();

        countDownShowGoalWait = CountDownGoalWait.GetComponent<CanvasGroup>();
        goalSpeedShow = goalSpeed.GetComponent<CanvasGroup>();
        InvokeShowGoalCheckInScreen += ShowGoalCheckInScreen;
        

    }

    private void Update()
    {
        //by준희, 센서값 대신에 테스트용으로 PC에서 클릭하여 볼의 목적지 좌표값을 생성
        // 볼의 속도는 퍼블릭으로 인스펙터 창에서 조정 가능.
        if (Input.GetMouseButtonDown(0) == true)
        {
            HJ_SoccerPlayer.mouseX = Input.mousePosition.x;
            HJ_SoccerPlayer.mouseY = Input.mousePosition.y;
            StartBall();
        }

    }
    IEnumerator IStartIntro()
    {
        //by준희, 인트로용 카메라 애니메이션 발동
        overViewCamera.SetActive(true);
        //by준희, 인트로 애니메이션(overView) 길이를 계산해 이시간 이후 다음 프로세스 진행
        yield return new WaitForSeconds(AnimRunningTime(overView, "OverViewMoving"));
        //by준희, 애니메이션 종료되면 인트로 카메라 끄기
        overViewCamera.SetActive(false);

        //by준희, BGM 관중소리 줄이기
        AudioManager.BGMmgr(0.5f);

        Debug.Log("오버뷰 카메라 끄기");
        //by준희,인트로가 끝났습니다.
        penaltyKickIntroEnd = true; 

        if (penaltyKickIntroEnd == true)
        {
            //by준희, 공차기 준비가 끝났습니다.
            StartCoroutine(IPenaltyKickStart()); 
        }
        
    }
    /// <summary>
    /// by준희, 애니메이션의 길이를 계산. Awake()에 계산할 애니메이터를 미리 선언.
    /// </summary>
    /// <param name="anim">계산하고자 하는 애니메이션의 애니메이터</param>
    /// <param name="animName">애니메이션의 이름</param>
    /// <returns></returns>
    float AnimRunningTime(Animator anim, String animName)
    {
        overViewAnimCtr = anim.runtimeAnimatorController;
        for (int i = 0; i < overViewAnimCtr.animationClips.Length; i++)
        {
            if (overViewAnimCtr.animationClips[i].name == animName)
            {
                overViewTime = overViewAnimCtr.animationClips[i].length;
            }
        }
        return overViewTime;
    }


    //public void Countdown()
    //{
    //    float waitingTime = 10;    //StartKickIntro()에서 패널티킥 대기 시간
    //    string currC;
    //    string prevC = null;
    //    Debug.Log(waitingTime);

    //    if (1 == 1)   //카운트 다운 조건 넣어주기 아니면 메소드 있는 곳에 조건 넣어주기
    //    {
    //        waitingTime -= Time.deltaTime;
    //        while (waitingTime > 0)
    //        {
    //            currC = string.Format("{0:f0}초", waitingTime);
    //            if (currC != prevC)
    //            {
    //                Debug.LogFormat(currC);
    //                //초 세는거 넣기
    //            }
    //            prevC = currC;
    //        }
    //    }
    //}

    /// <summary>
    /// by준희, 플레이어의 플레이 타임을 제약하는 카운트 다운 코루틴(waitingTime(초) 조정).
    /// 카운트다운 시간이 끝나면 No Goal로 간주.
    /// </summary>
    IEnumerator ICountDown()
    {
        String time; 
        for(int i = waitingTime; i > -1; i--)
        {
            if (actionAble || stillOnGround == true)
            {
                countDownShowGoalWait.alpha = 1;
                time = i.ToString();
                CountDownGoalWait.text = string.Format("{0}초",i);
            }
            else
            {
                countDownShowGoalWait.alpha = 0;
                yield break;
            }

            if (i == 0) //by준희, 시간초과로 골판정해야 할 때 0초 되면 무조건 Fail 
            {
                isGoal = false;
                ZoomOut();
                countDownShowGoalWait.alpha = 0;
                ShowGoalCheckInScreen();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    //by준희, 패널티킥게임 들어왔을 때 처음 1번만 실행. (플레이 환경 셋팅)
    /// <summary>
    /// by준희 패널티킥 시작시 환경설정 : 카메라 1인칭으로변환, 왼쪽 상단 5번의 골체크 이미지 보이기
    /// (패널티킥 시작시 딱 한번만 실행)
    /// </summary>
    /// <returns></returns>
    IEnumerator IPenaltyKickStart()
    {
        //by준희, 카메라가 1인칭(=플레이어가 플레이 가능하게 되는 시점)으로 돌아오는 시간
        yield return new WaitForSeconds(0.5f);    
        Debug.Log("패널티킥 준비 완료");

        //by준희, 왼쪽상단의 골여부 이미지 보이기
        HJ_ScoreMgr.CountBallShow.alpha = 1;
        actionAble = true;   //센서의 값 또는 테스트용 값을 받겠다.StartBall()발동

        //by준희, 1게임당 공차는 시간을 제약하는 카운트다운 하고 싶다면 주석 지우기
        //StartCoroutine(ICountDown());

        //by준희, (위의)카운트 다운 시간이 모두 끝나면 노골처리, 사용 원하면 주석 지우기
        //PenaltyKickFail();  

    }

    /// <summary>
    /// by준희, 스크린에 패널티킥 성공 실패여부 보이기. ball의 스피드 보이기.
    /// 왼쪽 상단 골 여부 표시하는 이미지의 색 변환. 성공 실패에 따른 플레이어의 
    /// 세레모니 애니메이션 실행.
    /// </summary>
    void ShowGoalCheckInScreen()
    {
        float velocity = ClientReDecode1.PNCVariable.Uvelocity;
        goalSpeedShow.alpha = 1;  ////볼 스피드를 스크린에 보이고 싶다면 주석 해제
        //by준희, 볼 스피드를 Text로 보여줌.
        goalSpeed.text = string.Format("시속 {0}km/h", velocity);
        BallColorInScrMGR();    //by준희, 골여부 이미지 색 변경.
        //goalTextShow.alpha = 1;   ////by준희, 텍스트로 골 노골 쓸 때 쓰기
        Ceremony();
        if (isGoal == false)
        {
            NogoalShow.alpha = 1;
            TweenUI(NogoalShow.gameObject);
            AudioManager.SoundEffect(AudioManager.Sound.GoalFail);   //by준희, 박수소리
            Debug.Log("패널티킥 실패");
            //goalCheckTEXT.text = "노골 ㅠㅠ";   //by준희, 텍스트로 골 노골 쓸 때 쓰기
            //이펙트 실행하는 곳
            
        }
        else
        {
            GoalShow.alpha = 1;
            TweenUI(GoalShow.gameObject);
            AudioManager.SoundEffect(AudioManager.Sound.GoalSuccess);
            Debug.Log("패널티킥 성공");
            //goalCheckTEXT.text = "고오오~~올!!!";   //by준희, 텍스트로 골 노골 쓸 때 쓰기
            EffectParticle = Instantiate(congratEffect, GoalEffectPos.position, GoalEffectPos.rotation);
        }
        StartCoroutine(IScreenReset());
    }
    /// <summary>
    /// by준희, 플레이어로부터 값을 받지 않도록 제어하고 스크린 리셋.
    /// </summary>
    /// <returns></returns>
    IEnumerator IScreenReset()
    {
        actionAble = false;
        //by준희, 센서값 리셋
        ClientReDecode1.PNCVariable.Uvelocity = 0;
        ClientReDecode1.PNCVariable.UscreenX = 0;
        ClientReDecode1.PNCVariable.UscreenY = 0;

        yield return new WaitForSeconds(offGoalCheckTxtTime);
        actionAble = true;
        //StartCoroutine(ICountDown());//(패널티킥 시간 초 제한 둘 때 사용하기)
        
        //goalTextShow.alpha = 0;   ////by준희, 텍스트로 골 노골 쓸 때 쓰기

        NogoalShow.alpha = 0;
        GoalShow.alpha = 0;

        goalSpeedShow.alpha = 0;
        //이펙트 사라지게하는 곳
        if(congratEffect != null)
        {
            Destroy(EffectParticle);
        }

    }



    #region 패널티킥 시작
    /// <summary>
    /// by준희, 플레이어의 볼 속도, 방향 값받아서 게임 시작(센서값 또는 테스트값 받기)
    /// </summary>
    public static void StartBall()  
    {
        if (actionAble == true)   //by준희, 인트로 애니메이션 다보고
        {
            if (StartBallEvent != null)
            {
                StartBallEvent();   //HJ_SoccerPlayer 의 StartShoot을 구독
            }
        }
    }
    #endregion

    #region 게임종료
    public void ToLobby()   //게임 실행 중 나가기(로비로 돌아옴)
    {
        SceneManager.LoadScene(0);
        penaltyKickIntroEnd = false;
        actionAble = false;
        isGoal = false;
        BallCOUNTinSCORE = -1;
        stillOnGround = false;
    }
    public void QuitGame()  //완전 종료
    {
        Application.Quit();
    }
    #endregion
}
