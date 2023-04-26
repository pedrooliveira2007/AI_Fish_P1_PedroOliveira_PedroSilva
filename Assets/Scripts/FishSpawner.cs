using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] allFishes;

    public void SpawnFish(FishType newFishType, Vector3 position)
    {
        switch(newFishType)
        {
            case FishType.small:
                Instantiate(allFishes[0], position, Quaternion.identity);
                break;
            case FishType.normal:
                Instantiate(allFishes[1], position, Quaternion.identity);
                break;
            case FishType.big:
                Instantiate(allFishes[2], position, Quaternion.identity);
                break;
        }
    }
}
