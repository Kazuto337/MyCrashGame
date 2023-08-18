using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static Action confirmedBet;
    public static Action canceledBed;

    public enum GameState
    {
        bet = 0,
        initializing,
        playing,
        endgame
    }

    public static GameState gameState;

    [Header("Game Actors")]
    [SerializeField] UserStatistics userStats;
    [SerializeField] BetBehavior betBehavior;

    [Header("Initializing State")]
    [SerializeField] TMP_Text timeBeforeGameplayTxt;
    [SerializeField] float timeBeforeGameplay;


    [Header("Gameplay State")]
    [SerializeField, Range(0.2f, 0.5f)] float multiplierIncreaseFactor; //Determines how much the multiplier increase each second (Default 0.2)
    [SerializeField] float gameplayLenght;
    [SerializeField] float gameplayTimer;
    private float multiplier;

    [SerializeField] BombBehavior bomb;

    [SerializeField] TMP_Text possibleRevenueTxt; //bet per current multiplier


    private void Start()
    {
        gameState = GameState.bet;// Delete in case of needing a persistence system
        multiplier = 1;
        bomb.multiplierTxt.text = "X" + multiplier.ToString("F2");
    }
    private void OnEnable()
    {
        confirmedBet += InitializeGameplay;
        canceledBed += ReturnToBetState;
    }

    private void InitializeGameplay()
    {
        gameState = GameState.initializing;
        multiplier = 1;
        bomb.multiplierTxt.text = "X" + multiplier.ToString("F2");
        bomb.ResetBomb();
        bomb.LightUp();
        CalculateGameplayLenght();

        timeBeforeGameplayTxt.text = timeBeforeGameplay.ToString("F2");

        StartCoroutine(GameplayTimer());

    }
    public void CalculateGameplayLenght()
    {
        gameplayLenght = UnityEngine.Random.Range(0, 15);
    }
    private void ReturnToBetState()
    {
        gameState = GameState.bet;
        timeBeforeGameplayTxt.text = string.Empty;
        StopCoroutine(GameplayTimer());
    }
    IEnumerator GameplayTimer()
    {
        float timer = timeBeforeGameplay;
        while (gameState == GameState.initializing)
        {
            if (timer <= 0)
            {
                bomb.StartGameplay();
                gameState = GameState.playing;
                StartCoroutine(Gameplay());
                break;
            }
            else
            {
                timeBeforeGameplayTxt.text = timer.ToString("F2");
                timer -= 1 * Time.deltaTime;
            }
            yield return null;
        }
    }
    IEnumerator Gameplay()
    {
        float possibleRevenue;
        gameplayTimer = 0;
        while (gameState == GameState.playing)
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
            else
            {
                gameplayTimer++;
                multiplier = 1 + gameplayTimer * multiplierIncreaseFactor;
                bomb.multiplierTxt.text = "X" + multiplier.ToString("F2");
                possibleRevenue = multiplier * userStats.totalBet;
                possibleRevenueTxt.text = "$" + possibleRevenue;
                yield return new WaitForSeconds(1);
            }
        }
    }
    private void OnDisable()
    {
        confirmedBet -= InitializeGameplay;
        canceledBed -= ReturnToBetState;
    }
}
