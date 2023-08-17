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
    [SerializeField, Range(0.2f, 0.5f)] float multiplierIncreaseFactor; //Determines how much the multiplier increase each second (Default 0.2)
    [SerializeField] float timeBeforeGameplay, gameplayLenght;
    [SerializeField] float gameplayTimer;
    private float multiplier;
    [SerializeField] BombBehavior bomb;


    private void Start()
    {
        gameState = GameState.bet;// Delete in case of needing a persistence system
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
        bomb.multiplierTxt.text = "X" + multiplier;
        bomb.ResetBomb();
        bomb.LightUp();
        CalculateGameplayLenght();
        StartCoroutine(GameplayTimer());
    }
    public void CancelGameplay()
    {
        StopCoroutine(GameplayTimer());
    }
    public void CalculateGameplayLenght()
    {
        gameplayLenght = UnityEngine.Random.Range(0, 15);
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
                StartCoroutine(Gameplay());
                break;
            }
            else
            {
                timer -= 1 * Time.deltaTime;
            }
            yield return null;
        }
    }
    IEnumerator Gameplay()
    {
        gameplayTimer = 0;
        while (gameState == GameState.playing)
        {
            if (gameplayTimer >= gameplayLenght)
            {
                gameState = GameState.endgame;
                bomb.Explote();
                bomb.Invoke("ResetBomb", 5f);
                break;
            }
            else
            {
                gameplayTimer++;
                multiplier = 1 + gameplayTimer * multiplierIncreaseFactor;
                bomb.multiplierTxt.text = "X" + multiplier;
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
