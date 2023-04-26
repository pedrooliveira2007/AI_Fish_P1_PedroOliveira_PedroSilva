using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SeaWeed")
        {
            Destroy(other.gameObject);
        }
    }
}
