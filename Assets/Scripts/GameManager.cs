using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public bool runnin = false;

    public float baseMoveSpeed;
    public float currentMoveSpeed;

    public int coins = 0;
    public float points = 0;
    public float pointsBaseValue = 3;
    public float pointsMultiplier = 1;
    public float powerUpMultiplier = 1;
    public Text TextScore;
    public Text TextCoins;

    public GameObject Player;

    public PlayerMovement playerMovement;
    public PlatformBuilder platformBuilder;

    public bool isSound = true;
    
    public PausedMenuController pmc;
    public GameObject gameOverScreen;


    public void Awake()
    {
        gameManager = this;

        //playerMovement.Init();
        platformBuilder.Init();
        //platformBuilder.StartGame();
    }

    public void AwakeGame()
    {
        Player.SetActive(true);
        playerMovement.Init();
        //platformBuilder.Init();
        platformBuilder.StartGame();
        runnin = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pmc.PausedButton();
        }

        RunAddPoints();
    }

    public void restartGame()
    {
        coins = 0;
        TextCoins.text = "Coins: " + coins.ToString();

        points = 0;
        pointsBaseValue = 3;
        pointsMultiplier = 1;
        powerUpMultiplier = 1;

        playerMovement.Respawn();
        gameOverScreen.SetActive(false);
        playerMovement.ResetPosition();

        playerMovement.ac.SetTrigger("Respawn");

        platformBuilder.StartGame();

        runnin = true;

        currentMoveSpeed = baseMoveSpeed;

        MainMenuController.mmc.gameRootScreen.SetActive(true);

    }

    public void ShowResult()
    {
        gameOverScreen.SetActive(true);
        MainMenuController.mmc.gameRootScreen.SetActive(false);
        
    }

    public void RunAddPoints()
    {
        if (runnin)
        {
            points += pointsBaseValue * pointsMultiplier * powerUpMultiplier * Time.deltaTime;
            pointsMultiplier += 0.05f * Time.deltaTime;
            pointsMultiplier = Mathf.Clamp(pointsMultiplier, 1, 10);

            currentMoveSpeed += .1f * Time.deltaTime;
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 1, 20);

            TextScore.text = "Score: " + ((int)points).ToString();
        }
    }

    public void AddCoins(int number)
    {
        coins += number;
        TextCoins.text = "Coins: " + coins.ToString();
    }
}
