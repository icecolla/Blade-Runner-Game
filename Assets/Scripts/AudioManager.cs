using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioMan;

    public Button[] buttonsOn;
    public Button[] buttonsOff;
    public AudioClip buttonOnSound;
    public AudioClip buttonOffSound;

    public Slider sliderBg;
    public Slider sliderSfx;
    public AudioMixer bgMixer;
    public AudioMixer sfxMixer;

    public AudioSource audioSourceBG;
    public AudioSource audioSourceEffects;
    public AudioClip coinSound;


    private void Awake()
    {
        if (audioMan == null)
        {
            audioMan = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Button btn in buttonsOn)
        {
            btn.onClick.AddListener(() => PlayButtonOnSound());
        }

        foreach (Button btn in buttonsOff)
        {
            btn.onClick.AddListener(() => PlayButtonOffSound());
        }
    }

    public void RefreshSoundState()
    {
        if (GameManager.gameManager.isSound)
        {
            audioSourceBG.UnPause();
        }
        else
        {
            audioSourceBG.Pause();
        }
    }

    public void PlayCoinEffect()
    {
        if (GameManager.gameManager.isSound)
        {
            audioSourceEffects.PlayOneShot(coinSound, 0.05f);
        }
    }

    public void PlayButtonOnSound()
    {
        audioSourceEffects.PlayOneShot(buttonOnSound);
    }

    public void PlayButtonOffSound()
    {
        audioSourceEffects.PlayOneShot(buttonOffSound);
    }

    public void SetBgLevel(float sliderValue)
    {
        bgMixer.SetFloat("bg_mixer_volume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSfxLevel(float sliderValue)
    {
        sfxMixer.SetFloat("sfx_mixer_volume", Mathf.Log10(sliderValue) * 20);
    }
}
