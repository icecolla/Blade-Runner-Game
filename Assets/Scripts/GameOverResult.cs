using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverResult : MonoBehaviour
{
    public Text TextScorego;
    public Text TextCoinsgo;

    private void Update()
    {
        if (!GameManager.gameManager.runnin)
        {
            TextScorego.text = "Score: " + ((int)GameManager.gameManager.points).ToString();
            TextCoinsgo.text = "Coins: " + GameManager.gameManager.coins.ToString();
        }
    }
}
