using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerLogic : MonoBehaviour
{
    public int totalItemCount;
    public int stage;
    public Text stageCountText;
    public Text playerCountText;
    // Start is called before the first frame update
    void Awake()
    {
        stageCountText.text = "/ " + totalItemCount;
    }

    // Update is called once per frame
    public void GetItem(int count)
    {
        playerCountText.text = count.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(stage);
        }
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
