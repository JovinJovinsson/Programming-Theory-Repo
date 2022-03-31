using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainManager : MonoBehaviour
{
    // This is the main driver of the game
    public static MainManager Instance { get; private set; }

    // This will allow the MainManager to provide access to the BattlegroundController
    [SerializeField] private BattlegroundController bgController;
    public BattlegroundController BGController
    {
        get { return bgController; }
    }

    // The array of monsters in the carousel along with the active index
    public Monster[] monsters = new Monster[2];

    // Current Round
    public int CurrentRound { get; set; }

    private bool gameActive = false;
    public bool GameActive
    {
        get { return gameActive; }
        set { gameActive = value; }
    }
    private int victory = -1;
    public int Victory
    {
        get { return victory; }
        set 
        { 
            victory = value; 
            
            // Check that we have a battlegroundController & hat Victory is not -1
            if (value != -1 && bgController != null)
            {
                // Trigger the results screen
                bgController.ShowResultScreen();
            }
        }
    }

    // Player attributes
    public string PlayerName { get; set; }
    public int SelectedMonster { get; set; }
    public int StrengthMod { get; set; }
    public int DefenseMod { get; set; }
    public int SpeedMod { get; set; }
    public int MonsterLevel { get; set; }
    public int MonsterExp { get; set; }
    public int UnspentPoints { get; set; }

    [SerializeField] public List<HighScores> Scores = new List<HighScores>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            // We've just loaded up the game let's load the high scores
            LoadHighScores();
        } else
        {
            Destroy(gameObject);
        }
    }

    // This allows us to attach methods for when Scenes change
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we're in the Battlegound Controller, make a reference to it
        if (scene.buildIndex == 1)
        {
            bgController = GameObject.Find("BattlegroundController").GetComponent<BattlegroundController>() as BattlegroundController;
        }
    }

    public void UpdateHighScores()
    {
        // Create a new high score
        HighScores newScore = new HighScores();
        newScore.round = CurrentRound;
        newScore.playerName = PlayerName;
        newScore.monsterID = SelectedMonster;
        newScore.monsterLevel = MonsterLevel;
        // The score will only track the raw data to reduce amount of math when rendering them
        newScore.strength = monsters[SelectedMonster].Strength;
        newScore.defense = monsters[SelectedMonster].Defense;
        newScore.speed = monsters[SelectedMonster].Speed;

        if (Scores.Count == 0)
        {
            // Our first score! Let's add it in
            Scores.Add(newScore);
        } else
        {
            // Iterate over all of the high scores to check if we beat them
            for (int i = 0; i < Scores.Count; i++)
            {
                if (CurrentRound >= Scores[i].round)
                {
                    // We beat it! So lets store this one at the index
                    Scores.Insert(i, newScore);
                }
            }
        }

        // Now drop all scores above the top 5!
        if (Scores.Count > 5)
        {
            Scores.RemoveRange(5, Scores.Count - 5);
        }

        // Now save them
        SaveHighScores();
    }

    [System.Serializable]
    public class HighScores
    {
        public int round;
        public string playerName;
        public int monsterID;
        public int monsterLevel;
        public int strength;
        public int defense;
        public int speed;
    }

    [System.Serializable]
    public class ScoreList
    {
        public string one;
        public string two;
        public string three;
        public string four;
        public string five;
    }
    /// <summary>
    /// This function saves the highscores in JSON
    /// </summary>
    public void SaveHighScores()
    {
        /// TODO: Find out how to do this in a less dirty mannner....
        ScoreList scoreList = new ScoreList();

        if (Scores.Count > 0)
        {
            scoreList.one = JsonUtility.ToJson(Scores[0]);
        }
        if (Scores.Count > 1)
        {
            scoreList.two = JsonUtility.ToJson(Scores[1]);
        }
        if (Scores.Count > 2)
        {
            scoreList.three = JsonUtility.ToJson(Scores[2]);
        }
        if (Scores.Count > 3)
        {
            scoreList.four = JsonUtility.ToJson(Scores[3]);
        }
        if (Scores.Count > 4)
        {
            scoreList.five = JsonUtility.ToJson(Scores[4]);
        }

        string json = JsonUtility.ToJson(scoreList);

        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/highscores.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);

            if (scoreList.one != "")
            {
                Scores.Add(JsonUtility.FromJson<HighScores>(scoreList.one));
            }
            if (scoreList.two != "")
            {
                Scores.Add(JsonUtility.FromJson<HighScores>(scoreList.two));
            }
            if (scoreList.three != "")
            {
                Scores.Add(JsonUtility.FromJson<HighScores>(scoreList.three));
            }
            if (scoreList.four != "")
            {
                Scores.Add(JsonUtility.FromJson<HighScores>(scoreList.four));
            }
            if (scoreList.five != "")
            {
                Scores.Add(JsonUtility.FromJson<HighScores>(scoreList.five));
            }
        }
    }
}
