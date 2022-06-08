using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cam;
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
