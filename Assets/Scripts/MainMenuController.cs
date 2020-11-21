using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject gameRootScreen;
    public GameObject gameInfoScreen;
    public GameObject menuSettingsScreen;


    public Sprite SoundOn, SoundOff;
    public Image SoundBtnImages;

    public static MainMenuController mmc;

    private void Awake()
    {
        mmc = this;
    }

    public void PlayButton()
    {
        gameObject.SetActive(false);
        gameRootScreen.SetActive(true);
        GameManager.gameManager.AwakeGame();
    }

    public void InfoButton()
    {
        gameObject.SetActive(false);
        gameInfoScreen.SetActive(true);
    }

    public void ReturnButton()
    {
        gameInfoScreen.SetActive(false);
        menuSettingsScreen.SetActive(false);
        gameObject.SetActive(true);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void SettingButton()
    {
        gameObject.SetActive(false);
        menuSettingsScreen.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SoundButton()
    {
        GameManager.gameManager.isSound = !GameManager.gameManager.isSound;
        SoundBtnImages.sprite = GameManager.gameManager.isSound ? SoundOn : SoundOff;
        AudioManager.audioMan.RefreshSoundState();
    }
}
