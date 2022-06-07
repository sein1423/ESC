using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환 시 반드시 포함.

public class ClearDirector : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("GameScene");
        }
    }
}
