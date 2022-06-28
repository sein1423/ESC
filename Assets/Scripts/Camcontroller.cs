using UnityEngine;


public class Camcontroller : MonoBehaviour
{
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = cam.position;
        this.transform.rotation = cam.rotation;
    }
}
