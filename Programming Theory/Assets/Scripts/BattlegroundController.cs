using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlegroundController : MonoBehaviour
{
    // These are the game objects in which the monsters will be spawned
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private GameObject enemyContainer;

    // The delay for which the monsters will spawn
    private const float spawnDelay = 1.5f;

    private int combatCountdown = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginCountdown());
        StartCoroutine(SpawnPlayer());
        StartCoroutine(SpawnEnemy());
    }

    /// <summary>
    /// Initiates a countdown till combat begins and then sets GameOver to false
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginCountdown()
    {
        while (combatCountdown >= 0)
        {
            yield return new WaitForSeconds(1);

            if (combatCountdown == 0)
            {
                MainManager.Instance.GameOver = false;
            } else
            {
                combatCountdown--;
            }
        }
    }

    /// <summary>
    /// After the 2x spawnDelay spawn the Enemy in the appropriate position
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnDelay * 2);

        Monster enemy = Instantiate(MainManager.Instance.monsters[MainManager.Instance.SelectedMonster], enemyContainer.transform.position, enemyContainer.transform.rotation);
        enemy.transform.parent = enemyContainer.transform;
    }

    /// <summary>
    /// After the 2x spawnDelay spawn the Player in the appropriate position
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);

        Monster player = Instantiate(MainManager.Instance.monsters[MainManager.Instance.SelectedMonster], playerContainer.transform.position, playerContainer.transform.rotation);
        player.isPlayer = true;
        player.transform.parent = playerContainer.transform;
    }
}
