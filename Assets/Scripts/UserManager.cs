using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserManager : MonoBehaviour {
    
    string userName, userId;
    Texture2D profilePicture;
    public Image Avatar;
    public Text UserName;
    
    public void UpdateGUI(string u_Id, string u_Name)
    {
        userId = u_Id;
        UserName.text = u_Name;

        StartCoroutine(getFBPicture());
    }

    public IEnumerator getFBPicture()
    {
        //To get our facebook picture we use this address which we pass our facebookId into
        var www = new WWW("http://graph.facebook.com/" + userId + "/picture?width=80&height=80");

        yield return www;

        profilePicture = www.texture;
        Sprite sprite = Sprite.Create(profilePicture, new Rect(0, 0, profilePicture.width, profilePicture.height), new Vector2(0.5f, 0.5f));
        Avatar.sprite = sprite;
    }
	
}
