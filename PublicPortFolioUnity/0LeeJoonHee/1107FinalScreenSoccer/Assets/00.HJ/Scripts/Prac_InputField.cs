using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prac_InputField : MonoBehaviour
{
    public static string playerName;
    public Text inputText;

    Prac_SoccerPlayer playerSC;
    void Start()
    {
        playerSC = GameObject.FindWithTag("Player").GetComponent<Prac_SoccerPlayer>();
    }

    public void GameStart()
    {
        // Inputfield 
        playerName = inputText.text;
        gameObject.SetActive(false);
        playerSC.enabled = true;
        Debug.Log(playerName);
    }
}
