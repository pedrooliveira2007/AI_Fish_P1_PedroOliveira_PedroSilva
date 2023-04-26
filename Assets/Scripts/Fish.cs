using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishType fishType;
    [SerializeField] private float energy;
    public float Energy {get => energy;} 
    private FishSpawner fishSpawner;

    void Start()
    {
        GameObject fishSpawnerParent = GameObject.FindGameObjectWithTag("FishSpawner");
        fishSpawner = fishSpawnerParent.GetComponent<FishSpawner>();
    }

    void Update()
    {
        if(energy >= 75f)
        {
            Reproduce();
        }
    }

    public void ReduceEnergy(float oldEnergy)
    {
        energy -= oldEnergy;
    }

    public void Eat()
    {
        
    }

    public void Reproduce()
    {
        Debug.Log("Spawn");
        fishSpawner.SpawnFish(fishType, transform.position);
        energy -= 55f;
    }
}
