using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCam : MonoBehaviour
{
    public GameObject Cam;

    Vector3 startScale;
    public float distance = 3;

    private void Start()
    {
        startScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(Cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;
        
        transform.rotation = Cam.transform.rotation;        
    }
}
