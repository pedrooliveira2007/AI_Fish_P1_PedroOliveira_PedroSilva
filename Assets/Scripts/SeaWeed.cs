using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaWeed : MonoBehaviour
{
    private float randomSpeed;

    void Start()
    {
        randomSpeed = Random.Range(0.005f,0.008f);
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0f,randomSpeed,0f);
    }
}
