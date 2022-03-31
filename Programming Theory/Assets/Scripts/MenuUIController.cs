using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIController : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;

    public void StartGame()
    {
        MainManager.Instance.PlayerName = playerName.text;
        MainManager.Instance.MonsterLevel = 1;
        SceneManager.LoadScene(1);
    }

    public void ViewHighScores()
    {

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
