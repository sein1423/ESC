using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class ResultNameInput : MonoBehaviour
{
    public GameObject dbManager;
    public GameObject gameManager;
    public GameObject inputNamePanel;
    public GameObject connectFailPanel;
    public GameObject playModePanel;
    public InputField playerNameInput;
    public Text inputNameWarning;
    public string inputName = null;

    void Awake()
    {
        inputName = playerNameInput.GetComponent<InputField>().text;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputNameInsertButton();
        }
    }


    public async void InputNameInsertButton()
    {
        inputName = playerNameInput.text;
        inputName = Regex.Replace(inputName, @"[^0-9a-z]", "");
        playerNameInput.text = inputName;
        if (inputName == "")
        {
            inputNameWarning.text = "이름을 입력하세요";
            return;
        }
        if (inputName.Length > 10)
        {
            inputNameWarning.text = "10글자 이하로 입력해주세요";
            return;
        }

        DBManager db = dbManager.GetComponent<DBManager>();
        GameManagement gm = gameManager.GetComponent<GameManagement>();
        db.DBCommand("insertName", inputName, "", "");
        await Task.Delay(500);
        if (GameManagement.staticQueryResult == "success")
        {
            gm.SetPlayerName(inputName);
            inputNameWarning.text = "";
            playModePanel.SetActive(true);
            inputNamePanel.SetActive(false);
            Destroy(GameObject.Find("SoundManager"));
        }
        else if(GameManagement.staticQueryResult == "connectFail")
        {
            gm.SetPlayerName(inputName);
            inputNameWarning.text = "";
            connectFailPanel.SetActive(true);
            inputNamePanel.SetActive(false);
            Destroy(GameObject.Find("SoundManager"));
        }
        else if (GameManagement.staticQueryResult == "overlap")
        {
            inputNameWarning.text = "중복된 이름입니다";
            return;
        }
        else if(GameManagement.staticQueryResult == "fail")
        {
            inputNameWarning.text = "잠시 후 다시시도 바랍니다";
            return;
        }
    }

    public void InputNameCloseButton()
    {
        if (GameManagement.staticPlayerName != null)
        {
            GameManagement.staticPlayerName = null;
            DBManager db = dbManager.GetComponent<DBManager>();
            db.DBCommand("deleteName", GameManagement.staticPlayerName, "", "");
        }
        SceneManager.LoadScene("Main");
    }

    public void ConnectFailCloseButton()
    {
        connectFailPanel.SetActive(false);
        playModePanel.SetActive(true);
    }
}
