using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController puc;

    public struct PowerUp
    {
        public enum Type
        {
            MULTIPLIER,
            IMMORTALITY,
            COINS_SPAWN
        }
        public Type PowerUpType;
        public float duration;
    }

    PowerUp[] powerUps = new PowerUp[3];
    Coroutine[] powerUpsTime = new Coroutine[3];

    public delegate void OnCoinsPowerUp(bool activete);
    public static event OnCoinsPowerUp CoinsPowerUpEvent;


    public GameObject powerUpPref;
    public Transform powerUpGrid;
    private List<PowerUpScript> powerUpsList = new List<PowerUpScript>();

    private void Start()
    {
        puc = this;
        powerUps[0] = new PowerUp() { PowerUpType = PowerUp.Type.MULTIPLIER, duration = 8 };
        powerUps[1] = new PowerUp() { PowerUpType = PowerUp.Type.IMMORTALITY, duration = 5 };
        powerUps[2] = new PowerUp() { PowerUpType = PowerUp.Type.COINS_SPAWN, duration = 7 };

        PlayerMovement.PowerupUseEvent += PowerUpUse;
    }

    private void PowerUpUse(PowerUp.Type type)
    {
        PowerUpReset(type);
        ResetAllPowerUps();
        powerUpsTime[(int)type] = StartCoroutine(PowerUpTime(type, CreatePowerUpPref(type)));

        switch(type)
        {
            case PowerUp.Type.MULTIPLIER:
                GameManager.gameManager.powerUpMultiplier = 2;
                break;
            case PowerUp.Type.IMMORTALITY:
                PlayerMovement.isImmortal = true;
                break;
            case PowerUp.Type.COINS_SPAWN:
                if (CoinsPowerUpEvent != null)
                {
                    CoinsPowerUpEvent(true);
                }
                break;
        }
    }

    private void PowerUpReset(PowerUp.Type type)
    {
        if (powerUpsTime[(int)type] != null)
        {
            StopCoroutine(powerUpsTime[(int)type]);
            //ResetAllPowerUps();
        }
        else
        {
            return;
        }

        powerUpsTime[(int)type] = null;

        switch (type)
        {
            case PowerUp.Type.MULTIPLIER:
                GameManager.gameManager.powerUpMultiplier = 1;
                break;
            case PowerUp.Type.IMMORTALITY:
                PlayerMovement.isImmortal = false;
                break;
            case PowerUp.Type.COINS_SPAWN:
                if (CoinsPowerUpEvent != null)
                {
                    CoinsPowerUpEvent(false);
                }
                break;
        }
    }

    public void ResetAllPowerUps()
    {
        for (int i = 0; i < powerUps.Length; i++)
        {
            PowerUpReset(powerUps[i].PowerUpType);
        }

        foreach (var pu in powerUpsList)
        {
            StartCoroutine(pu.DestroyBar());
        }
        powerUpsList.Clear();
    }

    IEnumerator PowerUpTime(PowerUp.Type type, PowerUpScript powerUpPref)
    {
        float time = powerUps[(int)type].duration;
        float currentTime = time;

        while (currentTime > 0)
        {
            powerUpPref.SetProgress(currentTime / time);
            if (GameManager.gameManager.runnin)
            {
                currentTime -= Time.deltaTime;
            }
            yield return null;
        }

        powerUpsList.Remove(powerUpPref);
        StartCoroutine(powerUpPref.DestroyBar());

        PowerUpReset(type);
    }

    PowerUpScript CreatePowerUpPref(PowerUp.Type type)
    {
        GameObject go = Instantiate(powerUpPref, powerUpGrid, false);
        var ps = go.GetComponent<PowerUpScript>();

        powerUpsList.Add(ps);
        ps.SetData(type);
        return ps;
    }
}
