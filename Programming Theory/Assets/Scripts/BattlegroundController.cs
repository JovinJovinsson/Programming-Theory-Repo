using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattlegroundController : MonoBehaviour
{
    // Combat Logger
    [SerializeField] private CombatLogger combatLog;
    public CombatLogger CombatLog
    {
        get { return combatLog; }
    }

    // These are the game objects in which the monsters will be spawned
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private GameObject enemyContainer;

    // Combat starter UI
    [SerializeField] private TextMeshProUGUI countdownDisplay;
    [SerializeField] private TextMeshProUGUI playerMonsterName;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI enemyMonsterName;
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI vs;
    [SerializeField] private TextMeshProUGUI roundCount;

    // Round finish UI
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private TextMeshProUGUI experience;
    [SerializeField] private GameObject nextRoundButton;
    [SerializeField] private GameObject levelUpButton;
    [SerializeField] private GameObject highScoresButton;

    // The index of the enemy type the player will verse
    private int selectedEnemy;
    // The delay till combat begins
    private int combatCountdown = 4;

    // Start is called before the first frame update
    void Start()
    {
        // Increment the current round
        MainManager.Instance.CurrentRound++;
        // Display this at the top
        roundCount.text = $"Round {MainManager.Instance.CurrentRound}";

        // Kick off the countdown to trigger combat
        StartCoroutine(BeginCountdown());
    }

    /// <summary>
    /// Initiates a countdown till combat begins and then sets GameOver to false to start combat.
    /// Also controls the visual of combat.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginCountdown()
    {
        while (combatCountdown >= -1)
        {
            // Every second we'll countdown further
            yield return new WaitForSeconds(1);

            // Update the countdown display
            countdownDisplay.text = $"{combatCountdown}";

            // Do a different thing at each stage of the countdown
            switch (combatCountdown)
            {
                case 4: SpawnPlayer(); break;
                case 3: vs.GetComponent<TextMeshProUGUI>().enabled = true; break;
                case 2: SpawnEnemy(); break;
                case 0:
                    {
                        MainManager.Instance.GameActive = true;
                        countdownDisplay.text = "FIGHT!";
                    } break;
                case -1:
                    {
                        countdownDisplay.gameObject.SetActive(false);
                        GameObject.Find("NameAnnouncement").SetActive(false);
                    } break;
                default: break;
            }

            combatCountdown--;
        }
    }

    /// <summary>
    /// After the 2x spawnDelay spawn the Enemy in the appropriate position
    /// </summary>
    /// <returns></returns>
    private void SpawnEnemy()
    {
        //yield return new WaitForSeconds(spawnDelay * 2);

        // Generate a random enemy index
        selectedEnemy = Random.Range(0, MainManager.Instance.monsters.Length);
        // Create the enemy monster facing the correct direction
        Monster enemy = Instantiate(MainManager.Instance.monsters[selectedEnemy], enemyContainer.transform.position, enemyContainer.transform.rotation);
        // Correct the enemy name
        enemy.name = enemy.name.Replace("(Clone)", "");
        // Make it sit inside the container for targeting
        enemy.transform.parent = enemyContainer.transform;

        // Show the enemy information
        enemyMonsterName.text = enemy.name;
        enemyName.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    /// <summary>
    /// After the 2x spawnDelay spawn the Player in the appropriate position
    /// </summary>
    /// <returns></returns>
    private void SpawnPlayer()
    {
        //yield return new WaitForSeconds(spawnDelay);

        // Create the player monster facing the correct direction
        Monster player = Instantiate(MainManager.Instance.monsters[MainManager.Instance.SelectedMonster], playerContainer.transform.position, playerContainer.transform.rotation);
        // Correct the player name
        player.name = player.name.Replace("(Clone)", "");
        // Make sure we tag it as the player
        player.isPlayer = true;
        // Make it sit inside the container for targeting
        player.transform.parent = playerContainer.transform;

        // Show the player information
        playerMonsterName.text = player.name;
        playerName.text = MainManager.Instance.PlayerName.ToLower();
    }

    /// <summary>
    /// This method is triggered from the MainManager when the Victory value is modified.
    /// It determines what progression should be shown (next round, level up or high scores)
    /// </summary>
    public void ShowResultScreen()
    {
        if (MainManager.Instance.Victory == 1)
        {
            // Player has won!
            // Show the result banner
            result.GetComponent<TextMeshProUGUI>().enabled = true;
            // Make it say victory
            result.text = "VICTORY";

            // Add exp to the monster
            MainManager.Instance.MonsterExp += 10;
            // Show the exp/level up text
            experience.GetComponent<TextMeshProUGUI>().enabled = true;
            // Check if we have enough exp to level up
            if (MainManager.Instance.MonsterExp < ((MainManager.Instance.MonsterLevel + 1) * 10))
            {
                // Not yet, so show the exp gain message
                experience.text = "10exp gained";
                // Let them progress to the next round
                nextRoundButton.SetActive(true);
            }
            else
            {
                // We have enough, let player know they're levelling up!
                experience.text = "level up!";
                // Show the level up button instead
                levelUpButton.SetActive(true);
            }
        }
        else if (MainManager.Instance.Victory == 0)
        {
            // Player has lost :(
            result.GetComponent<TextMeshProUGUI>().enabled = true;
            result.text = "DEFEAT";
            // Change the text colour to red
            result.color = Color.red;

            MainManager.Instance.UpdateHighScores();

            // Display the button to go to the high scores screen
            highScoresButton.SetActive(true);
        }
    }

    public void NextRound()
    {
        // Reset the victory trigger to the invalid value
        MainManager.Instance.Victory = -1;
        // Load this same scene again
        SceneManager.LoadScene(1);
    }

    public void LevelUp()
    {

    }

    public void HighScores()
    {
        // Load the high score screen
        SceneManager.LoadScene(2);
    }
}
