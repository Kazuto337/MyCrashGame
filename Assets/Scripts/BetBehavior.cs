using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class BetBehavior : MonoBehaviour
{
    [SerializeField] TMP_InputField betInputField;
    int currentBet;
    UserStatistics userStats;
    void Start()
    {
        userStats = UserStatistics.instance;
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
                castValue = 100;
            }
        }

        currentBet = castValue;
        betInputField.text = castValue.ToString("C");
    }

    public void ConfirmBet()
    {
        userStats.totalBet = currentBet;
    }
}
