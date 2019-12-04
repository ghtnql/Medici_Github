using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//볼의 체크만 함
public class HJ_SoccerBall : MonoBehaviour
{

    float spinTop = ClientReDecode1.PNCVariable.UspinTop;
    float spinSide = ClientReDecode1.PNCVariable.UspinSide;

    #region 골체크
    public static Transform tr;
    public static float initZ = 52.531f;
    float endZ = 60;//53.95f;
    float initX = -3.589f;
    float endX = 3.589f;
    float initY = 0;
    float endY= 2.353f;
    #endregion

    //public float spinZ = 0f;

    bool stopCheck;
    Rigidbody ballStop;

    void Start()
    {
        tr = GetComponent<Transform>();
        ballStop = gameObject.GetComponent<Rigidbody>();
        Invoke("StillOnGround", 3); //2초 뒤에 그라운드에 있으면 노골 판정
        AudioManager.SoundEffect(AudioManager.Sound.BallKick);

    }

    void Update()
    {
        transform.Rotate(spinTop, spinSide, 0);
        GoalCheck();

    }

    void StillOnGround()
    {
        if (tr.position.z < initZ)
        {
            GameManager.isGoal = false;
            GameManager.InvokeShowGoalCheckInScreen();
            stopCheck = true;
        }
    }

    void GoalCheck()
    {
        if(tr.position.z < initZ)
        {
            //by준희, 공이 골체크구간과 생성위치 사이에 있으면 
            //3초후 노골 판정.
            GameManager.stillOnGround = true;
        }
        if (tr.position.z > initZ && tr.position.z < endZ)
        {
            //by준희, 볼의 속도가 높으면 프레임을 뚫어서 검출 구간을
            //뛰어 넘음으로 속도를 줄여준다.
            ballStop.velocity = new Vector3(0, 0, 0);
            GameManager.stillOnGround = false;
            if (tr.position.x > initX && tr.position.x < endX)
            {
                if (tr.position.y > initY && tr.position.y < endY)
                {
                    if(stopCheck == false)
                    {
                        Debug.Log("Goall");
                        GameManager.isGoal = true;
                        
                        //by준희, isGoal에 따라 골, 노골이 정해져서 
                        //InvokeShowGoalCheckInScreen()는 이 위치에 있어야함.
                        GameManager.InvokeShowGoalCheckInScreen();
                        stopCheck = true;
                    }
                }
                else
                {
                    if (stopCheck == false)
                    {
                        Debug.Log("No Goall*");
                        GameManager.isGoal = false;
                        GameManager.InvokeShowGoalCheckInScreen();
                        stopCheck = true;
                    }
                }
            }
            else
            {
                if (stopCheck == false)
                {
                    Debug.Log("No Goall**");
                    GameManager.isGoal = false;
                    GameManager.InvokeShowGoalCheckInScreen();
                    stopCheck = true;
                }
            }
        }
    }
}
