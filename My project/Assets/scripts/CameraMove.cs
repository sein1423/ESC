using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float rx;
    float ry;
    public float rotSpeed = 200;


    void Awake()
    {
        
    }

    
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx += rotSpeed * my * Time.deltaTime;
        ry += rotSpeed * mx * Time.deltaTime;

        rx = Mathf.Clamp(rx, -80, 80);

        transform.eulerAngles = new Vector3(-rx, ry, 0);
    }


  
   
}
