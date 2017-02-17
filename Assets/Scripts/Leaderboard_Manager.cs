using UnityEngine;
using System.Collections.Generic;

public class Leaderboard_Manager : MonoBehaviour
{
    public static Leaderboard_Manager Instance;

    int bestScore;

    public GameObject PanelBoardOff;
    public GameObject PanelBoardOn;

    public GameObject RankEntryPrefab;
    public GameObject PanelContent;

    GameObject Entry;
    RankPrefab PrefabScript;

    GameObject[] ListEntry = new GameObject[5];
    RankPrefab[] ListPrefabScript = new RankPrefab[5];

    float posX;
    float posY;

    int score;
    string name;
    
    void Start()
    {
        Instance = this;
    }

    public void ShowLeaderboard()
    {
        // Reset leaderboard
        for (int i = 0; i <= 4; i++)
        {
            Destroy(ListEntry[i]);
            Destroy(ListPrefabScript[i]);
        }

        // Add rank entry
        for (int i = 0; i <= 4; i++)
        {
            score = PlayerPrefs.GetInt("HighScore" + (i + 1).ToString());
            name = PlayerPrefs.GetString("PlayerName" + (i + 1).ToString());

            ListEntry[i] = Instantiate(RankEntryPrefab) as GameObject;
            ListEntry[i].transform.SetParent(PanelContent.transform);
            ListEntry[i].transform.localScale = new Vector3(1, 1, 1);

            ListPrefabScript[i] = ListEntry[i].GetComponent<RankPrefab>();
            ListPrefabScript[i].SetInfo(i + 1, name, score);
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void btnOfflineClick()
    {
        PanelBoardOff.SetActive(true);
        PanelBoardOn.SetActive(false);
    }

    public void btnOnlineClick()
    {
        PanelBoardOff.SetActive(false);
        PanelBoardOn.SetActive(true);
    }

    // Post score to facebook
    //public void PostScore()
    //{
    //    bestScore = PlayerPrefs.GetInt("BestScore", 0);
    //    var score_data = new Dictionary<string, string>() { { "score", bestScore.ToString() } };
    //    FB.API("me/scores", Facebook.HttpMethod.POST, PostScoreCallBack, score_data);
    //    Debug.Log(bestScore.ToString());
    //}

    //void PostScoreCallBack(FBResult result)
    //{
    //    if (FB.IsLoggedIn)
    //        Debug.Log("Score Updated!");
    //    else
    //        Debug.Log("Score update fail");
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
