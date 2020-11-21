using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBuilder : MonoBehaviour
{
    public GameObject[] freePlatforms;
    public GameObject[] obstaclePlatforms;

    public List<GameObject> currentBlocks = new List<GameObject>();

    public Transform lastPlatform = null;
    
    private Vector3 startPosition;

    private bool isObstacle;

    public void Init()
    {
        startPosition = transform.position;

        StartCreatePlatforms();
    }

    private void Update()
    {
        if (GameManager.gameManager.runnin)
        {
            transform.position -= Vector3.right * GameManager.gameManager.currentMoveSpeed * Time.deltaTime;

            CheckForSpawn();
        }
    }

    public void CreatePlatforms()
    {
        if (isObstacle)
        {
            CreateFreePlatform();
        }
        else
        {
            CreateObstaclePlatform();
        }
    }

    public void CheckForSpawn()
    {
        if (currentBlocks[0].transform.position.x - PlayerMovement.startPosition.x < - 30f)
        {
            DestroyBlock();

            if (GameManager.gameManager.points < 100)
            {
                CreatePlatforms();
            }
            if (GameManager.gameManager.points > 100)
            {
                CreatePlatform(obstaclePlatforms, obstaclePlatforms.Length);
            }
        }
    }

    public void DestroyBlock()
    {
        Destroy(currentBlocks[0]);
        currentBlocks.RemoveAt(0);
    }

    public void StartGame()
    {
        transform.position = startPosition;

        foreach (var go in currentBlocks)
        {
            Destroy(go);
        }
        currentBlocks.Clear();
        lastPlatform.position = startPosition;
        lastPlatform = null;

        StartCreatePlatforms();
    }

    public void StartCreatePlatforms()
    {
        for (int i = 0; i < 4; i++)
        {
            CreateFreePlatform();
        }
    }

    public void CreateFreePlatform()
    {
        CreatePlatform(freePlatforms, freePlatforms.Length);

        isObstacle = false;
    }

    public void CreateObstaclePlatform()
    {
        CreatePlatform(obstaclePlatforms, obstaclePlatforms.Length);

        isObstacle = true;
    }

    public void CreatePlatform(GameObject[] platform, int lenght)
    {
        int index = Random.Range(0, lenght);

        GameObject block = Instantiate(platform[index], PlatformPosition(), Quaternion.identity, transform);//, platformController);
        lastPlatform = block.transform;

        currentBlocks.Add(block);
    }

    public Vector3 PlatformPosition()
    {
        Vector3 position = (lastPlatform == null) ?
                           transform.position :
                           lastPlatform.GetComponent<PlatformController>().endPoint.position;
        return position;
    }
}
