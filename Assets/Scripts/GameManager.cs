using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static Action confirmedBet;
    public static Action pulledBet;

    public enum GameState
    {
        bet = 0,
        playing,
        pulled,
        endgame
    }

    public static GameState gameState;

    [Header("Game Actors")]
    [SerializeField] UserStatistics userStats;
    [SerializeField] BetBehavior betBehavior;

    [Header("Gameplay State")]
    [SerializeField, Range(0.2f, 0.5f) , Tooltip("Determines how much the multiplier increase each second (Default 0.2)")] float multiplierIncreaseFactor;
    [SerializeField , Tooltip("Gameplay duration express in seconds")] float gameplayLenght;
    [SerializeField] float gameplayTimer;
    
    private float multiplier;
    [SerializeField] BombBehavior bomb;
    [SerializeField] TMP_Text possibleRevenueTxt;

    [Header("Endgame State")]
    [SerializeField] GameObject winPannel;    
    [SerializeField] TMP_Text revenueTxt;
    [Tooltip("User bet multiply by the current value of the Multplier")] public float possibleRevenue; 


    private void Start()
    {
        gameState = GameState.bet;// Delete in case of needing a persistence system
        multiplier = 1;
        bomb.multiplierTxt.text = "X" + multiplier.ToString("F2");
    }
    private void OnEnable()
    {
        confirmedBet += InitializeGameplay;
        pulledBet += PullBet;
    }

    private void InitializeGameplay()
    {
        multiplier = 1;
        bomb.multiplierTxt.text = "X" + multiplier.ToString("F2");
        bomb.ResetBomb();
        bomb.LightUp();
        CalculateGameplayLenght();
    }
    private void CalculateGameplayLenght()
    {
        gameplayLenght = UnityEngine.Random.Range(0, 15);

        bomb.StartGameplay();
        gameState = GameState.playing;
        StartCoroutine(Gameplay());
    }
    IEnumerator Gameplay()
    {        
        gameplayTimer = 0;
        while (gameState == GameState.playing || gameState == GameState.pulled)
        {
            if (gameplayTimer >= gameplayLenght)
            {
                gameState = GameState.endgame;
                bomb.Explote();
                bomb.Invoke("ResetBomb", 5f);
                yield return new WaitForSeconds(4f);
                gameState = GameState.bet;
                betBehavior.ResetBetsBehavior();
                break;
            }
            else if (gameplayLenght == 0)
            {
                gameState = GameState.endgame;
                bomb.Explote();
                bomb.Invoke("ResetBomb", 5f);
                yield return new WaitForSeconds(4f);
                gameState = GameState.bet;
                betBehavior.ResetBetsBehavior();
                break;
            }
            else
            {
                gameplayTimer += 1 * Time.deltaTime;
                multiplier = 1 * MathF.Pow(MathF.E , (multiplierIncreaseFactor * gameplayTimer));
                bomb.multiplierTxt.text = "X" + multiplier.ToString("F2");
                possibleRevenue = multiplier * userStats.totalBet;
                possibleRevenueTxt.text = "$" + possibleRevenue;
                yield return null;
            }
        }
    }
    private void PullBet()
    {
        if (gameState == GameState.playing)
        {
            winPannel.SetActive(true);
            gameState = GameState.pulled;            
            userStats.balance += possibleRevenue;
            StartCoroutine(ShowRevenue());
        }
    }

    IEnumerator ShowRevenue()
    {
        revenueTxt.text = "$" + possibleRevenue;
        yield return new WaitForSeconds(3f);
        revenueTxt.text = string.Empty;
        betBehavior.ResetBetsBehavior();
        winPannel.SetActive(false);
    }
    private void OnDisable()
    {
        confirmedBet -= InitializeGameplay;
        pulledBet -= PullBet;
    }
}
