using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJ_SoccerPlayer : MonoBehaviour
{
    GameObject ballInstance;
    public GameObject ball;
    Transform playerTR;

    Animator s_Animator;

    public float ballSpeed = 10;
    float bufferSpeed = 0.07f;

    // private Rigidbody rb;

    public Transform ballPos;
    #region PNC센서값변수
    int screenX;
    int screenY;
    float screenVelocity;
    #endregion

    public static float mouseX;
    public static float mouseY;

    public Camera ballCam;

    float ballPosZ = 70;

    Vector3 unityPos;

    //public bool isShoot;

    float addY = 10;
    //float add_RIGHTx = -20.2f;
    //float add_LEFTx = -14.2f;

    float add_RIGHTx = -20.2f;
    float add_LEFTx = 0;

    private void OnEnable()
    {
        GameManager.StartBallEvent += StartShoot;
    }
    private void OnDisable()
    {
        GameManager.StartBallEvent -= StartShoot;
    }

    // Start is called before the first frame update
    void Start()
    {
        s_Animator = gameObject.GetComponent<Animator>();
        ballCam = ballCam.GetComponent<Camera>();
        GameManager.Ceremony += Ceremony;
        GameManager.ZoomOut += ZoomOutCam;
        playerTR = gameObject.GetComponent<Transform>();
    }
    public void PlayerReset()
    {
        playerTR.position = new Vector3(-0.17f, 0, 40.67f);
        playerTR.rotation = new Quaternion(0, 0, 0, 0);
    }
    /// <summary>
    /// 
    /// </summary>
    public void StartShoot()
    {
        screenX = ClientReDecode1.PNCVariable.UscreenX;
        screenY = ClientReDecode1.PNCVariable.UscreenY;
        screenVelocity = ClientReDecode1.PNCVariable.Uvelocity;

        ScreenToUnity();
        BallisReady();
        MovingBall();
       
    }
    /// <summary>
    /// by준희, 센서의 값이 들어왔기 때문에 Ball을 생성 시킵니다.
    /// </summary>
    public void BallisReady()
    {
        ballInstance = Instantiate(ball, ballPos.position, ballPos.rotation);
        //by준희, HJ_ScoreMgr에서 fireBall검출
        if(GameManager.fireBall == true)
        {
            GameManager.FireBall(true);
        }
        

        GameManager.actionAble = false;
        //by준희, 플레이어의 3인칭 시점으로 변환(카메라 애니메이션 실행)
        ZoomOutCam();

        //isShoot = true;

        s_Animator.SetTrigger("Shooting");

        //by준희, 5.5초뒤에 공은 사라진다.
        Destroy(ballInstance, 5.5f);
    }

    void MovingBall()
    {
        //조율후
        #region PC테스트용

        ballInstance.GetComponent<Rigidbody>().AddForce((UnityPos.ConvertedPos + new Vector3(0, addY, 0)) * ballSpeed * Time.deltaTime / bufferSpeed, ForceMode.Acceleration);
        GameManager.KeepPos(UnityPos.ConvertedPos.x, UnityPos.ConvertedPos.y);
        //Debug.LogFormat("쏜 좌표 : x({0}), y({1}), z({2})", UnityPos.ConvertedPos.x, UnityPos.ConvertedPos.y, UnityPos.ConvertedPos.z);

        #endregion

        #region PNC센서용
        //if (UnityPos.ConvertedPos.x <= ballCam.transform.position.x) //왼쪽으로 공이 갈 때
        //{
        //    Debug.Log("슛");
        //    ballInstance.GetComponent<Rigidbody>().AddForce((UnityPos.ConvertedPos + new Vector3(add_LEFTx, addY, 0)) * screenVelocity * Time.deltaTime / bufferSpeed, ForceMode.Acceleration);
        //}/*
        //    else if (UnityPos.ConvertedPos.x >= ballCam.transform.position.x && UnityPos.ConvertedPos.x >= ballCam.transform.position.x+100)
        //    {
        //        Debug.Log("슛");
        //        ballInstance.GetComponent<Rigidbody>().AddForce((UnityPos.ConvertedPos) * screenVelocity * Time.deltaTime / bufferSpeed, ForceMode.Acceleration);
        //    }
        //    */
        //else//오른쪽으로 갈때
        //{
        //    if (UnityPos.ConvertedPos.x > ballCam.transform.position.x && UnityPos.ConvertedPos.x < Mathf.Abs(add_RIGHTx))
        //    {
        //        ballInstance.GetComponent<Rigidbody>().AddForce((UnityPos.ConvertedPos + new Vector3(0, addY, 0)) * screenVelocity * Time.deltaTime / bufferSpeed, ForceMode.Acceleration);
        //    }
        //    else
        //    {
        //        Debug.Log("슛");
        //        ballInstance.GetComponent<Rigidbody>().AddForce((UnityPos.ConvertedPos + new Vector3(add_RIGHTx, addY, 0)) * screenVelocity * Time.deltaTime / bufferSpeed, ForceMode.Acceleration);

        //    }
        //}
        #endregion
    }

    void Ceremony()
    {
        if (GameManager.isGoal == true)
        {
            s_Animator.SetTrigger("IsGoal");
        }

        if (GameManager.isGoal == false)
        {
            s_Animator.SetTrigger("IsNoGoal");
        }
    }
    /// <summary>
    /// by준희, 스크린 좌표2D를 Unity좌표 3D로 변환
    /// </summary>
    public void ScreenToUnity() //스크린 좌표를 Unity카메라에 보이는 씬 좌표로 변환
    {
        #region 센서값 사용할 때 활성화
        ////PNC에서 값을 보낼 때 사용할 메소드(테스트시 생략)
        screenY -= 1080;
        screenY = Mathf.Abs(screenY);

        unityPos = ballCam.ScreenToWorldPoint(new Vector3(screenX, screenY, ballPosZ));
        UnityPos unityPos2 = new UnityPos(unityPos);
        #endregion

        #region PC테스트용
        //unityPos = ballCam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, ballPosZ));
        //UnityPos unityPos2 = new UnityPos(unityPos);
        #endregion
    }

    public struct UnityPos
    {
        public static Vector3 ConvertedPos;

        public UnityPos(Vector3 unityPosition)
        {
            ConvertedPos = unityPosition;
        }
    }

    public void ZoomOutCam()
    {
        HJ_SmoothFollow.dist = 3f;
        HJ_SmoothFollow.height = 2f;

        Invoke("Reset", 5f);
    }

    void Reset()
    {
        HJ_SmoothFollow.dist = -7.36f;
        HJ_SmoothFollow.height = 1.36f;
        PlayerReset();
    }
}

