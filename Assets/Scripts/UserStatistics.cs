using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserStatistics : MonoBehaviour
{
    public static UserStatistics instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }

    public float balance;
    public int totalBet;

    [SerializeField] TMP_Text balanceTxt;
    [SerializeField] TMP_Text totalBetTxt;

    private void Update()
    {
        balanceTxt.text = "$" + balance;
        totalBetTxt.text = "$" + totalBet;
    }

}
