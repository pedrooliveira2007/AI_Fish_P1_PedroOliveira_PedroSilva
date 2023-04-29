
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishType fishType;
    [SerializeField] private float energy;
    public float Energy {get => energy;}
    public FishType FishType { get => fishType; }
    public bool Chased { get; internal set; }

    private FishSpawner fishSpawner;

    void Start()
    {
        GameObject fishSpawnerParent = GameObject.FindGameObjectWithTag("FishSpawner");
        fishSpawner = fishSpawnerParent.GetComponent<FishSpawner>();
        energy = 50f;
    }

    public void ReduceEnergy(float oldEnergy)
    {
        energy -= oldEnergy;
    }

    public void BeingChased(bool val)
    {
        Chased = val;
    }

    public void Eat(Transform target)
    {
        if (target.tag == "Fish")
        {
            energy += target.localScale.x ;
        }
        if (target.tag == "SeaWeed")
        {
            energy += 5 ;
        }
        
    }

    public void Reproduce()
    {
        Debug.Log("Spawn");
        fishSpawner.SpawnFish(fishType, transform.position);
        energy -= 55f;
    }
}
