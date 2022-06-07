using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultNameInput : MonoBehaviour
{
    public InputField playerNameInput;
    public string inputName = null;
    public Text nameOverlap;
    public GameObject dbManager;
    public GameObject gameManager;
    public GameObject playModePanel;

    void Awake()
    {
        inputName = playerNameInput.GetComponent<InputField>().text;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputName();
        }
    }


    public void InputName()
    {
        inputName = playerNameInput.text;

        if (inputName == "")
        {
            nameOverlap.text = "이름을 입력하세요";
            return;
        }

        if (inputName.Length > 6)
        {
            nameOverlap.text = "6글자 이하로 입력해주세요";
            return;
        }

        DB db = dbManager.GetComponent<DB>();
        GameManagement gm = gameManager.GetComponent<GameManagement>();

        if (db.DBInsertName(inputName)) //중복된 이름이 없으면 DB에 이름이 Insert 되고 true 반환
        {
            gm.SetPlayerName(inputName);
            nameOverlap.text = "";
            playModePanel.SetActive(true);
            Destroy(GameObject.Find("SoundManager"));
        }
        else
            nameOverlap.text = "중복된 이름입니다";
    }
}
