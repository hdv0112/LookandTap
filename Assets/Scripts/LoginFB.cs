//using UnityEngine;
//using Facebook;
//using Facebook.MiniJSON;
//using System.Collections;

//public class LoginFB : MonoBehaviour {

//    public static LoginFB Instance;
//    public GameObject PanelLoading;

//	// Use this for initialization
//	void Start () {
//        Instance = this;
//	}

//    public void FBInit()
//    {
//        FB.Init(SetInit, OnHideUnity);
//    }

//    private void SetInit()
//    {
//        Debug.Log("FB SetInit done.");
//        PanelLoading.SetActive(false);

//        if (FB.IsLoggedIn)
//        {
//            Debug.Log("FB Logged in");
//        }
//        else {
//            FBLogin();
//        }
//    }

//    private void OnHideUnity(bool isGameShown)
//    {
//        if (!isGameShown)
//        {
//            // pause the game - we will need to hide                                             
//            Time.timeScale = 0;
//        }
//        else
//        {
//            // start the game back up - we're getting focus again                                
//            Time.timeScale = 1;
//        }
//    }

//    void FBLogin()
//    {
//        FB.Login("public_profile,publish_actions,user_friends", 
//            AuthCallBack);
//    }

//    void AuthCallBack(FBResult result)
//    {
//        if (FB.IsLoggedIn)
//        {
//            Debug.Log("FB login worked.");

//            FB.API("/me?fields=name", Facebook.HttpMethod.GET, GetInfoCallBack);

//        }
            
//        else
//            Debug.Log("FB login fail.");
//    }

//    void GetInfoCallBack(FBResult result)
//    {
//        if (FB.IsLoggedIn)
//        {
//            // Load UserName and Profile Picture
//            IDictionary dict = Json.Deserialize(result.Text) as IDictionary;
//            string userName = dict["name"].ToString();
//            UserManager.Instance.UpdateGUI(FB.UserId, userName);
//        }
//        else {
//            Debug.Log("Get Info failed.");
//        }
//    }	
//}
