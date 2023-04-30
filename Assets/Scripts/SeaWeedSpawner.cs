using System.Collections;
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
        while (true)
        {
            int randomNum = Random.Range(0, 100);
            if (randomNum >= 50)
                SpawnSeaWeed();
            yield return new WaitForSeconds(5f);
        }
    }

    public void SpawnSeaWeed()
    {
        Instantiate(seaWeed, transform.position, Quaternion.identity);
    }
}
