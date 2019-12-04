using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prac_SoccerPlayer : MonoBehaviour
{

    public delegate void ShowScore(int ballCnt);
    public static event ShowScore OnShoot;

    public delegate void ClientValue();
    public static ClientValue StartPlay;

    GameObject ballInstance;
    public GameObject ball;

    Animator s_Animator;

    Vector3 movement;
    public float speed = 3.0f;

    public float ballSpeed = 10f;
     float bufferSpeed = 3f;

    float addY = 10;

    // private Rigidbody rb;

    public Transform ballPos;

    float screenX;
    float screenY;

    float mouseX;
    float mouseY;

    public Camera ballCam;

    //    public int ballPosZ = 1;

    Vector3 unityPos;

    public static bool isShoot = false;
    // bool isShooting = false;

    // 슈팅할 수 있는 공의 갯수
    public static int ballCnt = 0;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        StartPlay += StartPlayerAction;
    }

    private void OnDisable()
    {
        StartPlay -= StartPlayerAction;
    }

    // Start is called before the first frame update
    void Start()
    {
        s_Animator = gameObject.GetComponent<Animator>();
        ballCam = ballCam.GetComponent<Camera>();
        // StartCoroutine (DelayShooting());
    }

    // Update is called once per frame
    void Update()
    {

        // 마우스 위치에 따른 공 발사
        if (Input.GetMouseButtonDown(0) == true)
        {

            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;

            ScreenToUnity();
            ZoomOutCam();
            Invoke("ShootBall", 0.2f);
        }

        playerCtrl();

        if (Input.GetMouseButtonUp(0) == true)
        {
            isShoot = false;
        }
    }
    /// <summary>
    /// by준희, 클라이언트로부터 값을 받아 게임 시작
    /// </summary>
    public void StartPlayerAction()
    {
        screenX = ClientReDecode1.PNCVariable.UscreenX;
        screenY = ClientReDecode1.PNCVariable.UscreenY;

        ScreenToUnity();
        ZoomOutCam();
        Invoke("ShootBall", 0.2f);
    }

    private void playerCtrl()
    {
        // 테스트를 위한 플레이어 이동 기능
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            s_Animator.SetBool("FrontWalk", true);
            Run();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            s_Animator.SetBool("FrontWalk", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * speed * Time.deltaTime);
            s_Animator.SetBool("BackWalk", true);
            Run();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            s_Animator.SetBool("BackWalk", false);
        }


        //테스트를 위한 플레이어의 회전
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0.0f, -90.0f * Time.deltaTime, 0.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f);
        }
    }

    public void ShootBall()
    {
        s_Animator.SetTrigger("Shooting");
        Invoke("CreateBall", 0.4f);
        isShoot = true;
        ballCnt++;

        OnShoot(ballCnt);
        audioSource.Play();
    }

    public void ScreenToUnity() //스크린 좌표를 Unity카메라에 보이는 씬 좌표로 변환
    {
        //PNC에서 값을 보낼 때 사용할 메소드(테스트시 생략)
        screenX -= 1080;
        screenY = Mathf.Abs(screenY);

        //unityPos = ballCam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, ballPosZ));
        unityPos = ballCam.ScreenToWorldPoint(new Vector3(screenX, screenY, 100));
        UnityPos unityPos2 = new UnityPos(unityPos);
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            s_Animator.SetBool("Run", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            s_Animator.SetBool("Run", false);
        }
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
        // 2f에서 -3으로 수정함
        Prac_SmoothFollow.dist = -3f;

        Invoke("Reset", 2f);
    }

    void Reset()
    {
        Prac_SmoothFollow.dist = -2f;
    }

    void CreateBall()
    {
        ballInstance = Instantiate(ball, ballPos.position, ballPos.rotation);
        ballInstance.GetComponent<Rigidbody>().AddForce((UnityPos.ConvertedPos + new Vector3(0, addY, 0))* ballSpeed / bufferSpeed, ForceMode.Acceleration);

        Destroy(ballInstance, 3f);
    }
}

