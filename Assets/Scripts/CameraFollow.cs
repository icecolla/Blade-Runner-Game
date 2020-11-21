using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 startDistance;
    private Vector3 moveVector;

    void Start()
    {
        startDistance = transform.position - target.position;
    }

    void LateUpdate()
    {
        moveVector = target.position + startDistance;

        moveVector.z = 0;
        moveVector.y = startDistance.y;

        transform.position = moveVector;

    }
}
