using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BamsongiController : MonoBehaviour
{
    GameObject text;
    GameObject target;
    
    
    void Start()
    {
        text = GameObject.Find("Text");
        target = GameObject.Find("target");
        
        
        //Shoot(new Vector3(0, 200, 2000));
    }
    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);

    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject == target)
        {
            GameManagement.jumsu += 10;
            text.GetComponent<Text>().text = "점수: " + GameManagement.jumsu + "점";
        }
        
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ParticleSystem>().Play();
       
    }
}
