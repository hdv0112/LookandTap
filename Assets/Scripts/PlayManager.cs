using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance;

    public Text TxtNumber;
    public Text TxtBestScore;
    int[] ListNumber = new int[9];

    // Score
    int Score;
    public int ResultScore;
    public Text TxtResultScore;
    string SaveHighScore = "HighScore"; // HighScore 1,2,3,4,5
    string SavePlayerName = "PlayerName"; // Players got high score
    public InputField InputName;
    public Text TxtTOP;

    public HighScore_Entry[] ListHighScore_Entry = new HighScore_Entry[5];
    int RankOffline;

    // Label
    public Text TxtLabel_Score;

    int level;
    int dem;

    bool isNextNumber;

    public GameObject eventSystem;

    // Audio
    AudioSource SoundClickRight;
    AudioSource SoundClickWrong;

    public GameObject PanelStart;
    public GameObject PanelResult;
    public GameObject PanelLeaderboard;
    public GameObject PanelHighScorePopup;

    // Timer
    public Image ImgTimer;
    float TimeRate = 1.0f;

    // State
    public bool isPlayed = false;

    // Ads times
    int AdsTimes;
    string SaveAdsTimes = "SaveAdsTimes";

    public MobileNativeRateUs ratePopUp;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        // Rate for app
        ratePopUp = new MobileNativeRateUs("Like this game?", "Rate my app!!!");
        ratePopUp.SetAndroidAppUrl("https://play.google.com/store/apps/details?id=com.OhGame.LookTap");

        isNextNumber = false;
        Score = 0;
        level = 1;
        dem = 0;
        RankOffline = 0;

        // Load high score
        for (int i = 0; i <= 4; i++)
        {
            ListHighScore_Entry[i] = new HighScore_Entry();
            ListHighScore_Entry[i].Score = PlayerPrefs.GetInt(SaveHighScore + (i + 1).ToString(), 0);
            ListHighScore_Entry[i].PlayerName = PlayerPrefs.GetString(SavePlayerName + (i + 1).ToString());

        }

        for (int i = 0; i <= 4; i++)
        {
            Debug.Log(ListHighScore_Entry[i].PlayerName + ": " + ListHighScore_Entry[i].Score.ToString());

        }
            
        TxtBestScore.text = ListHighScore_Entry[0].Score.ToString();

        // Audio
        SoundClickRight = (AudioSource)gameObject.AddComponent<AudioSource>();
        AudioClip RightClick_AudioClip;
        RightClick_AudioClip = (AudioClip)Resources.Load("Audio/taptap");
        SoundClickRight.clip = RightClick_AudioClip;
        SoundClickRight.loop = false;

        SoundClickWrong = (AudioSource)gameObject.AddComponent<AudioSource>();
        AudioClip WrongClick_AudioClip;
        WrongClick_AudioClip = (AudioClip)Resources.Load("Audio/wrong");
        SoundClickWrong.clip = WrongClick_AudioClip;
        SoundClickWrong.loop = false;

        // Ads
        UM_AdManager.instance.Init();
        int bannerId1 = UM_AdManager.instance.CreateAdBanner(TextAnchor.LowerCenter);
        UM_AdManager.instance.ShowBanner(bannerId1);

    }

    // Generate list of numbers
    void RandomGenerateNumbers()
    {
        int count = 0;

        // Generate different numbers
        for (count = 0; count < level; count++)
        {
            ListNumber[count] = Random.Range(1, 10);
        }

        // Display list numbers
        for (int i = 0; i < level; i++)
        {
            TxtNumber.text += ListNumber[i].ToString();
        }

    }

    // Event click to number buttons
    public void Number_Click()
    {
        EventSystem ev = eventSystem.GetComponent<EventSystem>();
        GameObject btnSelect = ev.currentSelectedGameObject;
        Button btnNumber = btnSelect.GetComponent<Button>();

        if (btnNumber.image.sprite.name == ListNumber[dem].ToString())
        {
            SoundClickRight.Play();
            dem++;

            if (dem == level)
            {
                Score++;

                ImgTimer.fillAmount = 1;

                isNextNumber = true;

                TxtBestScore.text = Score.ToString();

                TxtNumber.text = "";

                //level++;
                dem = 0;
            }

        }
        else
        {
            ResultScore = Score;
            GameOver();
        }

        // Increase velocity
        if (Score < 100 && Score % 10 == 0)
        {
            TimeRate += 0.1f;
        }

    }

    // Start game
    public void StartGame()
    {
        isPlayed = true;
        TxtNumber.text = "";
        RandomGenerateNumbers();
    }

    // Game over
    void GameOver()
    {
        SoundClickWrong.Play();

        TxtNumber.color = Color.red;
        TxtNumber.text = "Game Over";
        TxtLabel_Score.text = "BEST";

        TxtResultScore.text = ResultScore.ToString();

        RankOffline = CheckListHighScore(Score);

        TxtBestScore.text = ListHighScore_Entry[0].Score.ToString();

        // Get high score (in top 5)
        if (RankOffline > 0 && RankOffline < 6)
        {
            ShowHighScorePopup();

            TxtTOP.text = "TOP " + RankOffline.ToString();

            // Get best score
            if (RankOffline == 1)
            {
                TxtBestScore.text = Score.ToString();

                // TxtNumbers when player get new high score
                TxtNumber.color = Color.yellow;
                TxtNumber.fontSize = 50;
                TxtNumber.text = "BEST SCORE";
                TxtResultScore.color = Color.yellow;
            }

        }

        ImgTimer.fillAmount = 1;
        isPlayed = false;

        // Show Ads
        AdsTimes = PlayerPrefs.GetInt(SaveAdsTimes, 0);
        AdsTimes++;
        if (AdsTimes > 5)
        {
            UM_AdManager.instance.StartInterstitialAd();
            AdsTimes = 0;
        }
        PlayerPrefs.SetInt(SaveAdsTimes, AdsTimes);
        PlayerPrefs.Save();

        PanelResult.SetActive(true);
    }

    public void RateApp()
    {
        ratePopUp.Start();
    }

    // Replay
    public void Replay()
    {
        level = 1;
        Score = 0;
        dem = 0;
        TimeRate = 1.0f;
        TxtLabel_Score.text = "SCORE";
        TxtBestScore.text = "0";
        TxtNumber.fontSize = 70;
        TxtNumber.color = Color.white;
        TxtNumber.text = "";
        TxtResultScore.color = Color.black;
        isPlayed = true;
        RandomGenerateNumbers();
    }

    public void SaveHighScoreClick()
    {
        HighScore_Entry entry = new HighScore_Entry();
        entry.Score = Score;
        entry.PlayerName = InputName.text;

        UpdateListHighScore(RankOffline - 2, entry);

        // Save high score
        for (int i = 0; i <= 4; i++)
        {
            PlayerPrefs.SetInt(SaveHighScore + (i + 1).ToString(), ListHighScore_Entry[i].Score);
            PlayerPrefs.Save();
            PlayerPrefs.SetString(SavePlayerName + (i + 1).ToString(), ListHighScore_Entry[i].PlayerName);
            PlayerPrefs.Save();

            Debug.Log(ListHighScore_Entry[i].PlayerName + ": " + ListHighScore_Entry[i].Score.ToString());
        }

        HideHighScorePopup();

    }

    // Check new high score
    public int CheckListHighScore(int newScore)
    {
        int i = 4;

        // Tìm vị trí cần chèn của số điểm mới
        while (i >= 0)
        {
            if (newScore > ListHighScore_Entry[i].Score)
            {
                i--;
            }
            else
            {
                break;
            }
        }

        return i + 2; // Return Rank number
    }

    // Update list highscore
    public void UpdateListHighScore(int insert_position, HighScore_Entry new_entry)
    {
        // Thực hiện chèn entry mới vào dãy
        // Nếu số cần chèn nằm ở vị trí cuối (nhỏ nhất trong 5 số)
        if (insert_position == 3)
        {
            ListHighScore_Entry[insert_position + 1] = new_entry;
        }
        // Nếu vị trí nằm ở giữa hoặc đầu dãy
        if (insert_position < 3)
        {
            for (int j = 4; j >= 0; j--)
            {
                ListHighScore_Entry[j] = ListHighScore_Entry[j - 1];

                if ((insert_position + 1) == (j - 1))
                {
                    ListHighScore_Entry[insert_position + 1] = new_entry;
                    break;
                }
            }
        }
    }

    // Show Leaderboard
    public void ShowLeaderboard()
    {
        PanelLeaderboard.SetActive(true);
        Invoke("ShowRank", 0.2f);
    }

    void ShowRank()
    {
        Leaderboard_Manager.Instance.ShowLeaderboard();
    }

    // High score popup
    public void ShowHighScorePopup()
    {
        PanelHighScorePopup.SetActive(true);
    }

    public void HideHighScorePopup()
    {
        PanelHighScorePopup.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (isPlayed == true)
        {
            if (isNextNumber == true)
            {
                if (level > 3)
                    level = 1;

                RandomGenerateNumbers();
                TxtNumber.color = Color.white;
                isNextNumber = false;
            }

            ImgTimer.fillAmount = Mathf.MoveTowards(ImgTimer.fillAmount, 0, TimeRate * Time.deltaTime);
        }

        if (ImgTimer.fillAmount <= 0)
        {
            ResultScore = Score;
            GameOver();
        }
    }
}
