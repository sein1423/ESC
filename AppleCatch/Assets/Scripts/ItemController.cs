using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public float dropSpeed = -0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, this.dropSpeed, 0);
        if(transform.position.y < -1.0f)
        {
            Destroy(gameObject);
        }        
    }
}
