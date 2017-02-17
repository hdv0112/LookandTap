using UnityEngine;
using UnityEngine.UI;

public class RankPrefab : MonoBehaviour
{
    public Text RankNumber;
    public Text PlayerName;
    public Text Score;

    public void SetInfo(int rank, string name, int score)
    {
        RankNumber.text = rank.ToString();
        PlayerName.text = name;
        Score.text = score.ToString();
    }

}
