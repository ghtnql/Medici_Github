using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prac_SoccerBall : MonoBehaviour
{
    public delegate void CheckScore(int goalCnt);
    public static event CheckScore OnGoal;

    [Range(-365,365)]
    public float spinX = 0f;
    [Range(-365,365)]
    public float spinY = 0f;
    //public float spinZ = 0f;

    //PNC센서에서 넘어오는 real 변수
    int screenX;
    int screenY;
    float spinTop;
    float spinSide;
    float shootAng;
    float directionAng;
    float velocity;

    Vector3 curveDirection;

    private Rigidbody rigid;

    int goalCnt = 0;

    Vector3 targetPos;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // screenX = ClientReDecode1.PNCVariable.UscreenX;
        // screenY = ClientReDecode1.PNCVariable.UscreenY;
        pncValue();

        transform.Rotate(spinX, spinY, 0);

    }
    private void pncValue()
    {
        spinTop = ClientReDecode1.PNCVariable.UspinTop;
        spinSide = ClientReDecode1.PNCVariable.UspinSide;
        spinTop = ClientReDecode1.PNCVariable.UspinTop;
        shootAng = ClientReDecode1.PNCVariable.UshootAng;
        directionAng = ClientReDecode1.PNCVariable.UdirectionAng;
        velocity = ClientReDecode1.PNCVariable.Uvelocity;

        targetPos = Prac_SoccerPlayer.UnityPos.ConvertedPos;
    }
    void FixedUpdate()
    {
        curveDirection = Vector3.up;
        rigid.AddForce(curveDirection);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "TARGET")
        {
            goalCnt++;
            OnGoal(goalCnt);
            audioSource.Play();
            Debug.Log("how many goal u have? = " + goalCnt);
        }
    }

    private void NoGoal()
    {
        if (Prac_SoccerPlayer.isShoot == true && goalCnt != 1)
        {
            goalCnt = 0;
            OnGoal(goalCnt);
            Debug.Log("No Goal bro try again");

        }
    }
}
