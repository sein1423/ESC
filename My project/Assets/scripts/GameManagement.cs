using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManagement : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("stage0");
    }

    public void Manager()
    {
        SceneManager.LoadScene("Manager");
    }

    public void init()
    {
        SceneManager.LoadScene("Main");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
