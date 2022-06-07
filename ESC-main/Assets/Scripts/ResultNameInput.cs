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
            nameOverlap.text = "�̸��� �Է��ϼ���";
            return;
        }

        if (inputName.Length > 6)
        {
            nameOverlap.text = "6���� ���Ϸ� �Է����ּ���";
            return;
        }

        DB db = dbManager.GetComponent<DB>();
        GameManagement gm = gameManager.GetComponent<GameManagement>();

        if (db.DBInsertName(inputName)) //�ߺ��� �̸��� ������ DB�� �̸��� Insert �ǰ� true ��ȯ
        {
            gm.SetPlayerName(inputName);
            nameOverlap.text = "";
            playModePanel.SetActive(true);
            Destroy(GameObject.Find("SoundManager"));
        }
        else
            nameOverlap.text = "�ߺ��� �̸��Դϴ�";
    }
}
