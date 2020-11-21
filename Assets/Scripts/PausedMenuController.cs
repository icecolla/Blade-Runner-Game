using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenuController : MonoBehaviour
{
    public PlayerMovement pm;


    public void PausedButton()
    {
        gameObject.SetActive(true);
        GameManager.gameManager.runnin = false;
        pm.Paused();
    }

    public void ResumeButton()
    {
        gameObject.SetActive(false);
        GameManager.gameManager.runnin = true;
        pm.UnPaused();
    }

    public void MenuButton()
    {
        pm.UnPaused();
        PowerUpController.puc.ResetAllPowerUps();
        gameObject.SetActive(false);
        MainMenuController.mmc.OpenMenu();

    }
}
