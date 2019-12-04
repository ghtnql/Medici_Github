using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{

    public void ToPenealtyKickScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ToPracticeScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ToRankingScene()
    {
        SceneManager.LoadScene(3);
    }
}
