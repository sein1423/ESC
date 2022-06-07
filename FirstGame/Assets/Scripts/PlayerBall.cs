using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    Rigidbody rigid;
    AudioSource audio;
    bool isJump;
    public float jumpPower;
    public int itemCount;
    public GameManagerLogic manager;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        isJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isJump = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            other.gameObject.SetActive(false);//비활성화
            audio.Play();
            itemCount++;
            manager.GetItem(itemCount);
        }
        else if(other.tag == "Finish")
        {
            if(itemCount == manager.totalItemCount)
            {
                if (manager.stage == 2)
                    SceneManager.LoadScene(0);
                SceneManager.LoadScene(manager.stage + 1);
            }
            else
            {
                SceneManager.LoadScene(manager.stage);
            }
        }
    }
}
