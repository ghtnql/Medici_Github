using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prac_InitialScoreBoard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private List<Transform> highscoreEntryTransformList;
    private List<HighscoreEntry> highscoreEntryList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        highscoreEntryList = new List<HighscoreEntry>()
        {
            new HighscoreEntry{score = 1111, name = "AAA"},
            new HighscoreEntry{score = 4444, name = "CSD"},
            new HighscoreEntry{score = 3333, name = "GES"},
            new HighscoreEntry{score = 8888, name = "FGG"},
            new HighscoreEntry{score = 5555, name = "CDE"},
            new HighscoreEntry{score = 2222, name = "ABC"}
        };

        // Sort entry list by Score
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].score > highscoreEntryList[i].score)
                {
                    // Swap
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }


        highscoreEntryTransformList = new List<Transform>();
        int cnt = (highscoreEntryList.Count > 10) ? 10 : highscoreEntryList.Count;
        for (int k = 0; k < cnt; k++)
        {
            CreateHighscoreEntryTransform(highscoreEntryList[k], entryContainer, highscoreEntryTransformList);
        }

        // this part makes highscoreboard available
        Highscores highscores = new Highscores{highscoreEntryList = highscoreEntryList};
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
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
