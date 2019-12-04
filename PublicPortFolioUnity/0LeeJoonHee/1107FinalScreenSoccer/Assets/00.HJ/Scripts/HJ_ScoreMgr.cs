using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HJ_ScoreMgr : MonoBehaviour
{
    public static CanvasGroup CountBallShow;
    public GameObject[] CountBall;

    Image[] Ball;

    float ballColorResetTime = 5;

    // Start is called before the first frame update

    private void Awake()
    {
        CountBallShow = gameObject.GetComponent<CanvasGroup>();
        CountBallShow.alpha = 0;
    }
    void Start()
    {
        Ball = new Image[5];

        //Ball의 이미지를 가져온다.
        for (int i = 0; i < 5; i++)
        {
            Ball[i] = CountBall[i].GetComponent<Image>();
        }
        GameManager.BallColorInScrMGR += ShowGoalColorOrReset;

    }

    void ShowGoalColorOrReset()
    {
        GameManager.BallCOUNTinSCORE += 1;

        if (-1 < GameManager.BallCOUNTinSCORE && GameManager.BallCOUNTinSCORE < 5)
        {
            if (GameManager.isGoal)
            {
                Ball[GameManager.BallCOUNTinSCORE].color = Color.green;
            }
            else
            {
                Ball[GameManager.BallCOUNTinSCORE].color = Color.red;
            }

            if (GameManager.BallCOUNTinSCORE == 4)
            {
                StartCoroutine(ISetGoalColor());
            }
        }

        //by준희, 3번이상 시도했을 때 FireBall 로직
        if (GameManager.BallCOUNTinSCORE > 1)
            FireBall();
    }

    /// <summary>
    /// by준희, 연속 3번 골일 경우 불꽃 슛
    /// </summary>
    void FireBall()
    {
        var firstBall = Color.white;
        var secondBall = Color.white;
        var thirdBall = Color.white;

        firstBall = Ball[GameManager.BallCOUNTinSCORE - 2].color;
        secondBall = Ball[GameManager.BallCOUNTinSCORE - 1].color;
        thirdBall = Ball[GameManager.BallCOUNTinSCORE].color;

        if (firstBall == secondBall && secondBall == thirdBall &&
            Ball[GameManager.BallCOUNTinSCORE].color == Color.green)
        {
            GameManager.fireBall = true;
        }
        else
        {
            GameManager.fireBall = false;
        }

    }

    IEnumerator ISetGoalColor()
    {
        yield return new WaitForSeconds(ballColorResetTime);
        for (int i = 0; i < 5; i++)
        {
            Ball[i].color = Color.white;
        }
        GameManager.BallCOUNTinSCORE = -1;
        GameManager.fireBall = false;   //by준희, 파이어볼 리셋
    }
}
