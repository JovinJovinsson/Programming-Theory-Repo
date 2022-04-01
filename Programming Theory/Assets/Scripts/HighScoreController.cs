using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreController : MonoBehaviour
{
    // The fields to store the high scores in
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Awake()
    {
        if (MainManager.Instance.Scores.Count == 0)
        {
            highScoreText.text = "no high scores yet";
        }
        else
        {
            highScoreText.text = "";
            for (int i = 0; i < MainManager.Instance.Scores.Count; i++)
            {
                highScoreText.text += $"[ {GetOrdinalNumber(i)} ] {MainManager.Instance.Scores[i].playerName} (Round {MainManager.Instance.Scores[i].round}) " +
                    $"{MainManager.Instance.monsters[MainManager.Instance.Scores[i].monsterID].name} (Level {MainManager.Instance.Scores[i].monsterLevel}) " +
                    $"<i><{MainManager.Instance.Scores[i].strength} STR | {MainManager.Instance.Scores[i].defense} DEF | " +
                    $"{MainManager.Instance.Scores[i].speed} SPD | {MainManager.Instance.Scores[i].defense * 10} HP></i>\n\n";
            }
        }
    }

    private string GetOrdinalNumber(int i)
    {
        string ordinal = "<b>";
        i++;
        switch (i)
        {
            case 1: ordinal += $"<color=#FFD700>{i}st</color></b>"; break;
            case 2: ordinal += $"<color=#C0C0C0>{i}nd</color></b>"; break;
            case 3: ordinal += $"<color=#CD7F32>{i}rd</color></b>"; break;
            default: ordinal += $"{i}th"; break;
        }
        return ordinal;
    }
    
    /// <summary>
    /// Takes user back to the Main Menu
    /// </summary>
    public void BackToMenu()
    {
        // Resets everything
        Destroy(MainManager.Instance.gameObject);
        
        SceneManager.LoadScene(0);
    }
}
