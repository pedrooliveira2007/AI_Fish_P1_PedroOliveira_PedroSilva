
using UnityEngine;

public class Fishtank : MonoBehaviour
{
    [SerializeField]
    private int lenght, width, height;
    [SerializeField]
    private Material tankMaterial;

    // Start is called before the first frame update
    void Start()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0,0, 0);
        cube.transform.localScale = new Vector3(lenght, height, width);
        cube.name = "Tank";
        cube.GetComponent<Renderer>().material = tankMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
