using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    //public WorldBuilder worldBuilder;

    //public Vector3 startPosition;

    //public float minX = -10f;


    //public delegate void TryToDeleteAndAddPlatform();
    //public event TryToDeleteAndAddPlatform onPlatformMovement;

    //public static WorldController instance;

    //private void Awake()
    //{
    //    startPosition = transform.position;

    //    worldBuilder = GameObject.Find("World Builder").GetComponent<WorldBuilder>();

    //    if (WorldController.instance != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    WorldController.instance = this;
    //    //DontDestroyOnLoad(gameObject);
    //}

    //private void Start()
    //{
    //    StartCoroutine(OnPlatformMovementCoroutine());
    //}

    //private void OnDestroy()
    //{
    //    WorldController.instance = null;
    //}

    //private void Update()
    //{
    //    transform.position -= Vector3.right * GameManager.gameManager.moveSpeed * Time.deltaTime;
    //}

    //public void ResetPosition()
    //{
    //    transform.position = startPosition;

    //    worldBuilder.platformController.position = Vector3.zero;
    //    //worldBuilder.lastPlatform = null;
    //}

    //IEnumerator OnPlatformMovementCoroutine()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1f);

    //        if (onPlatformMovement != null)
    //        {
    //            onPlatformMovement();
    //        }
    //    }
    //}
}
