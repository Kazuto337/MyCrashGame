using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static Action<string> Feedback;

    [SerializeField] TMP_Text feedbackTxt;

    [SerializeField] GameObject betField, gameplayField;

    private void Start()
    {
        feedbackTxt.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Feedback += OnFeedback;
    }
    private void OnFeedback(string message)
    {
        feedbackTxt.gameObject.SetActive(true);
        feedbackTxt.text = message;

        StartCoroutine(ClearFeedback());
    }
    IEnumerator ClearFeedback()
    {
        yield return new WaitForSeconds(3f);
        feedbackTxt.text = string.Empty;
        feedbackTxt.gameObject.SetActive(false);
    }
    private void Update()
    {
        switch (GameManager.gameState)
        {
            case GameManager.GameState.bet:
                betField.SetActive(true);
                gameplayField.SetActive(false);
                break;
            case GameManager.GameState.playing:
                betField.SetActive(false);
                gameplayField.SetActive(true);
                break;
            case GameManager.GameState.endgame:
                betField.SetActive(false);
                gameplayField.SetActive(true);
                break;
        }
    }
    private void OnDisable()
    {
        Feedback -= OnFeedback;
    }
}
