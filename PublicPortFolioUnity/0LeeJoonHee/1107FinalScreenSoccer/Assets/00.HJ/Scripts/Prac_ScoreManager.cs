using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Prac_ScoreManager : MonoBehaviour
{
    public GameObject scoreBoard;
    public GameObject inputField;

    public static int ballScore = 0;
    //int ballCombo = 1;

    void Start()
    {
        Prac_SoccerPlayer.OnShoot += RealTimeScore;
        Prac_SoccerBall.OnGoal += Scoring;
    }

    void RealTimeScore(int ballCnt)
    {
        Debug.Log("#" + Prac_SoccerPlayer.ballCnt + " score = " + ballScore);

        if (ballCnt == 5)
        {
            Debug.Log("your score is ? after 5 shoot " + ballScore);
            Invoke("DelayScoreBoard", 1.5f);
            ballScore = 0;
            Prac_SoccerPlayer.ballCnt = 0;
            //goalCnt = 0;
        }
    }

    private void DelayScoreBoard()
    {
        scoreBoard.SetActive(true);
        if (scoreBoard == true)
        {
            GameObject.FindWithTag("Player").GetComponent<Prac_SoccerPlayer>().enabled = false;
        }
    }

    void Scoring(int goalCnt)
    {
        ballScore = ballScore + 1000 * goalCnt;
        // HJ_Target.isGoal = false;
        Prac_SoccerPlayer.isShoot = false;
    }
    void OnDisable()
    {
        Prac_SoccerPlayer.OnShoot -= RealTimeScore;
        Prac_SoccerBall.OnGoal -= Scoring;
    }

    public void CloseBoard()
    {
        scoreBoard.SetActive(false);
        inputField.SetActive(true);
    }
    public void ToLobbyScene()
    {
        SceneManager.LoadScene(0);
    }
}
