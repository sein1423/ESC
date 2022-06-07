using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteController : MonoBehaviour
{
    float rotSpeed = 0.0f;

    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
             this.rotSpeed = 100.0f;
            GetComponent<AudioSource>().Play();
            GetComponent<ParticleSystem>().Play();
        }

        transform.Rotate(0,0,this.rotSpeed);

        this.rotSpeed *= 0.98f;
        
    }
}
