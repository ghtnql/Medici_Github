using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HJ_HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;   // **
    private Transform entryTemplate;    // **
    private List<Transform> highscoreEntryTransformList;
    private List<HighscoreEntry> highscoreEntryList;    // **

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");       
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");  

        entryTemplate.gameObject.SetActive(false);                      
       
        // Add data point
        AddHighscoreEntry(Prac_ScoreManager.ballScore, Prac_InputField.playerName);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Sort entry list by Score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    // Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        int cnt = (highscores.highscoreEntryList.Count > 10) ? 10 : highscores.highscoreEntryList.Count;
        for (int k = 0; k < cnt; k++)
        {
            CreateHighscoreEntryTransform(highscores.highscoreEntryList[k], entryContainer, highscoreEntryTransformList);
        }
        // check what data in json file
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 40f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);
        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    /*
    * Represents a single High score entry
     */
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

}