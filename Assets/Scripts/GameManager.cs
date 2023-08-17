using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static Action confirmedBet;
    public static Action canceledBed;

    enum GameState
    {
        bet = 0,
        initializing,
        playing,
        endgame
    }

    [SerializeField] GameState gameState;
    [SerializeField] float timeBeforeGameplay, gameplayLenght;
    [SerializeField] float gameplayTimer;
    private float multiplier;
    [SerializeField] BombBehavior bomb;
    

    private void Start()
    {
        gameState = GameState.bet;// Delete in case of need a persistence system
    }
    private void OnEnable()
    {
        confirmedBet += InitializeGameplay;
        canceledBed += ReturnToBetState;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.bet:
                break;
            case GameState.initializing:
                break;
            case GameState.playing:                
                if (gameplayTimer >= gameplayLenght)
                {
                    gameState = GameState.endgame;
                    bomb.Explote();
                    bomb.Invoke("ResetBomb" , 1.5f);
                    break;
                }
                else
                {
                    gameplayTimer += 1 * Time.deltaTime;
                    multiplier += gameplayTimer * 0.2f;
                    bomb.multiplierTxt.text = "X" + multiplier;
                }
                break;
            case GameState.endgame:
                break;
            default:
                break;
        }
    }

    private void InitializeGameplay()
    {
        gameState = GameState.initializing;
        multiplier = 1;
        bomb.multiplierTxt.text = "X" + multiplier;
        bomb.ResetBomb();
        bomb.LightUp();
        CalculateGameplayLenght();
        StartCoroutine(GameplayTimer());
    }
    public void CalculateGameplayLenght()
    {
        gameplayLenght = UnityEngine.Random.Range(0 , 60);
    }
    private void ReturnToBetState()
    {
        gameState = GameState.bet;
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
                break;
            }
            else
            {
                timer -= 1 * Time.deltaTime;
            }
            yield return null;
        }
    }
    private void OnDisable()
    {
        confirmedBet -= InitializeGameplay;
        canceledBed -= ReturnToBetState;
    }
}
