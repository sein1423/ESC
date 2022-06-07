using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    public float speed = 5;
    public GameObject Timer_text1;
    GameObject nearObject;
    void Awake()
    {
      
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = Vector3.right * h + Vector3.forward * v;

       

        dir = Camera.main.transform.TransformDirection(dir);

        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            SceneManager.LoadScene("Escape");
            DontDestroyOnLoad(Timer_text1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }
    



}
