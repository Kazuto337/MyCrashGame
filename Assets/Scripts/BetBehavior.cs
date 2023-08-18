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
    }

    public void ResetBetsBehavior()
    {
        confirmBetButton.gameObject.SetActive(true);
        userStats.totalBet = 0;
    }
    public void ValidateValue(string value)
    {
        int castValue = 0;
        if (!int.TryParse(value, out castValue))
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
