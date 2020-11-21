using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform endPoint;
    public GameObject coins;
    public int coinChance;

    //public ParticleSystem weather;

    private bool coinsSpawn;
    private bool powerUpSpawn;

    public List<GameObject> PowerUps;

    private void Awake()
    {
        
        PowerUpController.CoinsPowerUpEvent += CoinsEvent;

        endPoint = gameObject.transform.Find("End");

        coinsSpawn = Random.Range(0, 101) <= coinChance;

        coins.SetActive(coinsSpawn);

        powerUpSpawn = Random.Range(0, 101) <= 75;// && !coinsSpawn;

        PowerUps[Random.Range(0, PowerUps.Count)].SetActive(powerUpSpawn);

        // weather = Instantiate(Resources.Load<ParticleSystem>("PS/Rain"), transform);
    }

    void CoinsEvent(bool activate)
    {
        if (activate)
        {
            coins.SetActive(true);
            return;
        }

        if (!coinsSpawn)
        {
            coins.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        PowerUpController.CoinsPowerUpEvent -= CoinsEvent;
    }
}
