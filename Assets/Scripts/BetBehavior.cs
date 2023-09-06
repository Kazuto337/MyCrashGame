using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class BetBehavior : MonoBehaviour
{
    [SerializeField] TMP_InputField betInputField;
    [SerializeField] Button confirmBetButton;
    int currentBet;
    UserStatistics userStats;
    void Start()
    {
        userStats = UserStatistics.instance;
        currentBet = int.Parse(betInputField.text.Substring(1));
    }

    public void ResetBetsBehavior()
    {
        confirmBetButton.gameObject.SetActive(true);
        userStats.totalBet = 0;
    }
    public void ValidateValue(string value)
    {
        int castValue = 0;
        if (!int.TryParse(value.Substring(1), out castValue))
        {
            float temp = 0;
            if (float.TryParse(value, out temp))
            {
                Debug.LogWarning("BET CAST INTO INT");
                castValue = Mathf.RoundToInt(temp);
            }
            else
            {
                Debug.LogWarning("BET IS NOT IN THE CORRECT FORMAT");
                UI_Manager.Feedback("BET IS NOT IN THE CORRECT FORMAT");
            }
        }

        currentBet = castValue;
        betInputField.text = "$" + castValue;
    }

    public void IncreaseBet()
    {
        if (currentBet < 10)
        {
            currentBet += 1;
        }
        else if (currentBet >= 10)
        {
            currentBet += 10;
        }
        else if (currentBet >= 100)
        {
            currentBet += 100;
        }
        else
        {
            currentBet += 1000;
        }

        betInputField.text = "$" + currentBet;
    }
    public void DecreaseBet()
    {
        if (currentBet > 0)
        {
            if (currentBet < 10)
            {
                currentBet -= 1;
            }
            else if (currentBet >= 10)
            {
                currentBet -= 10;
            }
            else if (currentBet >= 100)
            {
                currentBet -= 100;
            }
            else
            {
                currentBet -= 1000;
            }

            betInputField.text = "$" + currentBet; 
        }
    }

    public void ConfirmBet()
    {
        currentBet = int.Parse(betInputField.text.Substring(1)); //the input comes in money format. The SubString Helps to ignore de coin symbol
        if (userStats.balance >= currentBet && currentBet > 0)
        {
            userStats.totalBet = currentBet;
            userStats.balance -= userStats.totalBet;
            GameManager.confirmedBet();
        }
        else
        {
            UI_Manager.Feedback("NOT ENOUGH CREDITS");
            confirmBetButton.gameObject.SetActive(true);
        }
    }
    public void PullBet()
    {
        GameManager.pulledBet();
    }
}
