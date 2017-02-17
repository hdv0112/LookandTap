using UnityEngine;

public class Result : MonoBehaviour
{
    public static Result Instance;

    public GameObject PanelMenu;
    
    void Start()
    {
        Instance = this;
    }

    public void ReplayClick()
    {
        PlayManager.Instance.Replay();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelMenu.SetActive(true);
            PlayManager.Instance.TxtNumber.color = Color.white;
            PlayManager.Instance.TxtNumber.text = "Let's Go!";
            gameObject.SetActive(false);
        }
    }
}
