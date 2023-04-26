using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaWeedSpawner : MonoBehaviour
{
    [SerializeField] private GameObject seaWeed;

    void Start()
    {
        StartCoroutine("SeaWeedTimer");
    }

    private IEnumerator SeaWeedTimer()
    {
        while(true)
        {
            int randomNum = Random.Range(0,100);
            if(randomNum >= 50)
                SpawnSeaWeed();
            yield return new WaitForSeconds(1f);
        }
    }

    public void SpawnSeaWeed()
    {
        //Instantiate(sewWeed);
    }
}
