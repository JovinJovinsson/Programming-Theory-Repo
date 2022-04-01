using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelUpController : MonoBehaviour
{
    // Monster Information
    [SerializeField] private TextMeshProUGUI monsterName;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI nextLevel;

    // Allocation Control
    [SerializeField] private TextMeshProUGUI pointsRemaining;
    // Always get 6 points to spend per level
    private int points = 6;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button confirmButton;

    // Increase Buttons
    [SerializeField] private Button increaseStr;
    [SerializeField] private Button increaseDef;
    [SerializeField] private Button increaseSpd;

    // Decrease Buttons
    [SerializeField] private Button decreaseStr;
    [SerializeField] private Button decreaseDef;
    [SerializeField] private Button decreaseSpd;

    // Upgrade Costs
    [SerializeField] private TextMeshProUGUI strengthCost;
    [SerializeField] private TextMeshProUGUI defenseCost;
    [SerializeField] private TextMeshProUGUI speedCost;

    // The arrays will store each step of upgrade cost
    [SerializeField] private int[] strengthArray = { 0, 0, 0, 0, 0 };
    [SerializeField] private int[] defenseArray = { 0, 0, 0, 0, 0 };
    [SerializeField] private int[] speedArray = { 0, 0, 0, 0, 0 };

    // Current Stats
    [SerializeField] private TextMeshProUGUI currentStr;
    [SerializeField] private TextMeshProUGUI currentDef;
    [SerializeField] private TextMeshProUGUI currentSpd;
    [SerializeField] private TextMeshProUGUI currentHP;
    [SerializeField] private TextMeshProUGUI currentCrit;

    // Current Stats
    [SerializeField] private TextMeshProUGUI nextStr;
    [SerializeField] private TextMeshProUGUI nextDef;
    [SerializeField] private TextMeshProUGUI nextSpd;
    [SerializeField] private TextMeshProUGUI nextHP;
    [SerializeField] private TextMeshProUGUI nextCrit;

    // Store the Current Stats for ease
    private int strength;
    private int defense;
    private int speed;

    // Start is called before the first frame update
    void Start()
    {
        SetupBasics();
        SetupCurrentStats();
        CheckCanDecrease();
        CheckCanIncrease();
    }

    /// <summary>
    /// Setup all the initial basics at the top.
    /// </summary>
    private void SetupBasics()
    {
        // Put the basic details in
        monsterName.text = MainManager.Instance.monsters[MainManager.Instance.SelectedMonster].name;
        currentLevel.text = "Level " + MainManager.Instance.MonsterLevel;
        // Increment the Monster's level
        nextLevel.text = "Level " + ++MainManager.Instance.MonsterLevel;

        // Add in any unspent points from previous rounds
        points += MainManager.Instance.UnspentPoints;
        pointsRemaining.text = $"{points}";
    }

    /// <summary>
    /// This puts in the current stats for both the Current Level and Next Level sides.
    /// </summary>
    private void SetupCurrentStats()
    {
        // Store the monster to make it easier
        Monster currentMonster = MainManager.Instance.monsters[MainManager.Instance.SelectedMonster];

        currentStr.text = $"{currentMonster.Strength(true)}";
        strength = currentMonster.Strength(true);
        currentDef.text = $"{currentMonster.Defense(true)}";
        defense = currentMonster.Defense(true);
        currentHP.text = $"{currentMonster.Defense(true) * 10}";
        currentSpd.text = $"{currentMonster.Speed(true)}";
        currentCrit.text = $"{10 + (0.2 * currentMonster.Speed(true))}%";
        speed = currentMonster.Speed(true);

        nextStr.text = $"{currentMonster.Strength(true)}";
        nextDef.text = $"{currentMonster.Defense(true)}";
        nextHP.text = $"{currentMonster.Defense(true) * 10}";
        nextSpd.text = $"{currentMonster.Speed(true)}";
        nextCrit.text = $"{10 + (0.2 * currentMonster.Speed(true))}%";
    }

    /// <summary>
    /// Checks if the next level can be afforded with current points, disables the button if not.
    /// </summary>
    private void CheckCanIncrease()
    {
        // Check if we can increase Strength
        if (GetCost(strength + 1) > points)
        {
            increaseStr.interactable = false;
        } else
        {
            increaseStr.interactable = true;
        }

        // Check if we can increase Defense
        if (GetCost(defense + 1) > points)
        {
            increaseDef.interactable = false;
        }
        else
        {
            increaseDef.interactable = true;
        }

        // Check if we can increase Speed
        if (GetCost(speed + 1) > points)
        {
            increaseSpd.interactable = false;
        }
        else
        {
            increaseSpd.interactable = true;
        }
    }

    /// <summary>
    /// Checks if there are any increases applied, if so enable the buttons
    /// </summary>
    private void CheckCanDecrease()
    {
       
        // Check Strength
        if (SumCosts(strengthArray) == 0)
        {
            decreaseStr.interactable = false;
        } else
        {
            decreaseStr.interactable = true;
        }

        // Check Defense
        if (SumCosts(defenseArray) == 0)
        {
            decreaseDef.interactable = false;
        }
        else
        {
            decreaseDef.interactable = true;
        }

        // Check Speed
        if (SumCosts(speedArray) == 0)
        {
            decreaseSpd.interactable = false;
        }
        else
        {
            decreaseSpd.interactable = true;
        }
    }

    /// <summary>
    /// Resets the view
    /// </summary>
    public void ResetButton()
    {
        // If we don't do this it's infinite level ups
        MainManager.Instance.MonsterLevel--;
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// Commits the new stats to the MainManager
    /// </summary>
    public void ConfirmButton()
    {
        // Grab a reference to the monster for ease
        Monster monster = MainManager.Instance.monsters[MainManager.Instance.SelectedMonster];

        // Update the mods to the new mods for the player
        MainManager.Instance.StrengthMod = strength - monster.BaseStrength;
        MainManager.Instance.DefenseMod = defense - monster.BaseDefense;
        MainManager.Instance.SpeedMod = speed - monster.BaseSpeed;

        // Store any unspent points
        MainManager.Instance.UnspentPoints = points;

        // Reset the experience
        MainManager.Instance.MonsterExp = 0;

        // Load the next combat
        SceneManager.LoadScene(1);
    }
    
    // ABSTRACTION
    /// <summary>
    /// Simply returns the cost that the next level will require
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    private int GetCost(int stat)
    {
        return Mathf.RoundToInt(stat / 5);
    }

    /// <summary>
    /// Updates the Points Remaining UI
    /// </summary>
    /// <param name="cost"></param>
    public void UpdatePointsRemaining(int cost)
    {
        // Reduce the points remaining
        points -= cost;
        // Update the UI
        pointsRemaining.text = $"{points}";
    }
    
    /// <summary>
    /// Sums the array of costs into a singular int
    /// </summary>
    /// <param name="array"></param>
    private int SumCosts(int[] array)
    {
        int costs = 0;
        for (int i = 0; i < array.Length; i++)
        {
            costs += array[i];
        }
        return costs;
    }

    /// <summary>
    /// Finds the next spot in the provided array and inputs the cost.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="cost"></param>
    private void AddCostToArray(ref int[] array, int cost)
    {
        // Iterate over the array of increases till we find an empty (0)
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == 0)
            {
                // Found the empty, so store the cost
                array[i] = cost;
                // Don't insert any more
                break;
            }
        }
    }

    /// <summary>
    /// Finds the cost in the provided array and removes it.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="cost"></param>
    private void RemoveCostFromArray(ref int[] array)
    {
        // Iterate over the array of increases till we find an empty (0)
        for (int i = array.Length - 1; i >= 0; i--)
        {
            if (array[i] != 0)
            {
                // Found the empty, so store the cost
                array[i] = 0;
                // Don't insert any more
                break;
            }
        }
    }

    /// <summary>
    /// Increases the strength and updates all values necessary
    /// </summary>
    public void IncreaseStrength()
    {
        // Increment the next level of strength
        strength++;

        // Get the cost
        int cost = GetCost(strength);
        // Update the UI points remaining
        UpdatePointsRemaining(cost);
        // Add the cost to the array
        AddCostToArray(ref strengthArray, cost);

        // Update the Next Level strength stat
        nextStr.text = $"{strength}";

        // Calculate the total cost
        int totalCost = SumCosts(strengthArray);
        // Update the cost number
        strengthCost.text = $"{totalCost}";

        // Update the button statuses
        CheckCanIncrease();
        CheckCanDecrease();
    }

    /// <summary>
    /// Decreases the strength and updates all values necessary
    /// </summary>
    public void DecreaseStrength()
    {
        // Get the cost
        int cost = GetCost(strength);
        // Increment the next level of strength
        strength--;
        // Update the UI points remaining
        UpdatePointsRemaining(-cost);
        // Remove the most recent cost
        RemoveCostFromArray(ref strengthArray);

        // Update the Next Level strength stat
        nextStr.text = $"{strength}";

        // Calculate the total cost
        int totalCost = SumCosts(strengthArray);
        // Update the cost number
        strengthCost.text = $"{totalCost}";

        // Update the button statuses
        CheckCanIncrease();
        CheckCanDecrease();
    }

    /// <summary>
    /// Increases the defense and updates all values necessary
    /// </summary>
    public void IncreaseDefense()
    {
        // Increment the next level of defense
        defense++;

        // Get the cost
        int cost = GetCost(defense);
        // Update the UI points remaining
        UpdatePointsRemaining(cost);
        // Add the cost to the array
        AddCostToArray(ref defenseArray, cost);

        // Update the Next Level defense stat
        nextDef.text = $"{defense}";
        nextHP.text = $"{defense * 10}";

        // Calculate the total cost
        int totalCost = SumCosts(defenseArray);
        // Update the cost number
        defenseCost.text = $"{totalCost}";

        // Update the button defense
        CheckCanIncrease();
        CheckCanDecrease();
    }

    /// <summary>
    /// Decreases the defense and updates all values necessary
    /// </summary>
    public void DecreaseDefense()
    {
        // Get the cost
        int cost = GetCost(defense);
        // Increment the next level of defense
        defense--;
        // Update the UI points remaining
        UpdatePointsRemaining(-cost);
        // Remove the most recent cost
        RemoveCostFromArray(ref defenseArray);

        // Update the Next Level defense stat
        nextDef.text = $"{defense}";
        nextHP.text = $"{defense * 10}";

        // Calculate the total cost
        int totalCost = SumCosts(defenseArray);
        // Update the cost number
        defenseCost.text = $"{totalCost}";

        // Update the button statuses
        CheckCanIncrease();
        CheckCanDecrease();
    }

    /// <summary>
    /// Increases the speed and updates all values necessary
    /// </summary>
    public void IncreaseSpeed()
    {
        // Increment the next level of defense
        speed++;

        // Get the cost
        int cost = GetCost(speed);
        // Update the UI points remaining
        UpdatePointsRemaining(cost);
        // Add the cost to the array
        AddCostToArray(ref speedArray, cost);

        // Update the Next Level defense stat
        nextSpd.text = $"{speed}";
        nextCrit.text = $"{10 + (0.2 * speed)}%";

        // Calculate the total cost
        int totalCost = SumCosts(speedArray);
        // Update the cost number
        speedCost.text = $"{totalCost}";

        // Update the button defense
        CheckCanIncrease();
        CheckCanDecrease();
    }

    /// <summary>
    /// Decreases the speed and updates all values necessary
    /// </summary>
    public void DecreaseSpeed()
    {
        // Get the cost
        int cost = GetCost(speed);
        // Increment the next level of defense
        speed--;
        // Update the UI points remaining
        UpdatePointsRemaining(-cost);
        // Remove the most recent cost
        RemoveCostFromArray(ref speedArray);

        // Update the Next Level defense stat
        nextSpd.text = $"{speed}";
        nextCrit.text = $"{10 + (0.2 * speed)}%";

        // Calculate the total cost
        int totalCost = SumCosts(speedArray);
        // Update the cost number
        speedCost.text = $"{totalCost}";

        // Update the button statuses
        CheckCanIncrease();
        CheckCanDecrease();
    }
}
