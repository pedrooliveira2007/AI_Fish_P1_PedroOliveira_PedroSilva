
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
        cube.transform.position = new Vector3(0, height/2, 0);
        cube.transform.localScale = new Vector3(lenght, height, width);
        cube.name = "Tank";
        cube.GetComponent<Renderer>().material = tankMaterial;
        Destroy(cube.GetComponent<BoxCollider>());
    }

  
}
