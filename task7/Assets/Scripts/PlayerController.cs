using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public void UpButtonDown()
    {
        transform.Translate(0, 1, 0);
    }

    public void DownButtonDown()
    {
        transform.Translate(0, -1, 0);
    }


    void Update()
    {
        
    }
}
