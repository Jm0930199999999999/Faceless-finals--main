using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int progressAmount;
    public Slider progressBar;
    public GameObject player;
    public GameObject gameOver;
    public GameObject gameWon;


    void Start()
    {
        progressAmount = 0;
        progressBar.value = 0;
        Fragment.OnFragmentCollected += UpdateProgressAmount;

        PlayerHealth.OnPlayerDeath += GameOverScreen;

        gameOver.SetActive(false);

        gameWon.SetActive(false);
    }

    void GameOverScreen()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
    void UpdateProgressAmount(int value)
    {
        progressAmount += value;
        progressBar.value = progressAmount;
        Debug.Log("Progress Amount: " + progressAmount);

        if (progressAmount >= 50)
        {
            gameWon.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
