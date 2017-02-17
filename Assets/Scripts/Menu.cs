using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    public static Menu Instance;

    public GameObject PanelFBPopUp;
    public GameObject PanelLoading;

    AudioSource SoundClick;

    //FBResult result;

	// Use this for initialization
	void Start () {

        // Sound
        SoundClick = (AudioSource)gameObject.AddComponent<AudioSource>();
        AudioClip ClickAudioClip;
        ClickAudioClip = (AudioClip)Resources.Load("Audio/taptap");
        SoundClick.clip = ClickAudioClip;
        SoundClick.loop = false;

        PlayManager.Instance.TxtNumber.text = "Let's Go!";

        // Check to login FB
        //if (FB.IsLoggedIn)
        //{
        //    // Load UserName and Profile Picture
        //    IDictionary dict = Json.Deserialize(result.Text) as IDictionary;
        //    string userName = dict["name"].ToString();
        //    UserManager.Instance.UpdateGUI(FB.UserId, userName);
        //}
        //else {
        //    ShowPanelFBPopUp();
        //}
	}

    // Start game
    public void PlayClick()
    {
        SoundClick.Play();
        PlayManager.Instance.StartGame();
        this.gameObject.SetActive(false);
    }

    // Facebook
    public void ShowPanelFBPopUp()
    {
        PanelFBPopUp.SetActive(true);
    }

    public void LoginBtnClick()
    {
        PanelLoading.SetActive(true);
        //LoginFB.Instance.FBInit();
        PanelFBPopUp.SetActive(false);
    }

    public void CloseBtnClick()
    {
        PanelFBPopUp.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
