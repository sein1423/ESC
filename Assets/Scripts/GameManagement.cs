using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    public GameObject helpSet;
    public GameObject rankingSet;
    public GameObject configurationSet;
    public GameObject menuSet;
    public GameObject optionSet;
    public GameObject timeManager;
    public GameObject dbManager;
    public bool isMenu = false;

    public static string staticPlayerName;
    public static string staticPlayTime;
    public static string staticDisplay;
    public static string staticPlaymode;

    private void Awake()
    {

    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) //esc ��ư�� ������ ��
        {
            if (menuSet.activeSelf) //����������
            {
                menuSet.SetActive(false); //��
                isMenu = false;
                Time.timeScale = 1.0f; //�ð��� �ٽ� ���
            }
            else
            {                    //����������
                menuSet.SetActive(true); //Ŵ
                isMenu = true;
                Time.timeScale = 0f; //�ð��� ����
            }

            if (optionSet.activeSelf) //����޴��� ����������
            {
                optionSet.SetActive(false); //��
                Time.timeScale = 0f; //�ð��� ����
            }

            Time.fixedDeltaTime = 0.02f * Time.timeScale; //fixedUpdate ������ �ð��� �Բ� �ٲ������
        }

        if (staticDisplay == "��üȭ��")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1920, 1080, true); //��üȭ��

        }
        else if (staticDisplay == "â���")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1280, 720, false); //â���

        }
    }

    public void SetPlayerName(string name)
    {
        staticPlayerName = name;
    }

    public void GameContinue()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Login");
    }

    public void Dead()
    {
        SceneManager.LoadScene("Die");
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            menuSet.SetActive(false);
            optionSet.SetActive(false);
        }
    }

    void OnDisable()
    {
        // ��������Ʈ ü�� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void Manager()
    {
        helpSet.SetActive(true);
    }

    public void init()
    {
        NetworkManager.StaticNetworkDisconnect();
        SceneManager.LoadScene("Main");
    }

    public void RePlay()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void GameConfiguration()
    {
        configurationSet.SetActive(true);
    }

    public void ConfigurationCloseButton()
    {
        configurationSet.SetActive(false);
    }

    public void GameClear()
    {
        DBManager db = dbManager.GetComponent<DBManager>();
        TimeManager timer = timeManager.GetComponent<TimeManager>();

        string timeNum = $"{timer.time_current:N2}";    //���� Ÿ���� �Ҽ��� �ι�°�ڸ� ������ ������
        float timeFloat = float.Parse(timeNum);           //����ȯ
        int timeInt = Mathf.RoundToInt(timeFloat);        //�ݿø�
        string minute = (timeInt / 60).ToString();      //�� ���
        string second = (timeInt % 60).ToString();      //�� ���
        string timeStr = minute + "m" + second + "s";
        staticPlayTime = timeStr;
        db.DBCommand("updateTime", staticPlayerName, timeStr, timeNum);      //�÷���Ÿ���� ������Ʈ�ϴ� DB�Լ�
    }

    public void TestCharacter()
    {
        SceneManager.LoadScene("Game");
        //DontDestroyOnLoad(timeManager);
        Destroy(GameObject.Find("SoundManager"));
    }

    public void GameSave()
    {

    }

    public void GameLoad()
    {

    }

    public void GameOption()
    {
        menuSet.SetActive(false);
        optionSet.SetActive(true);
    }

    public void BackButton()
    {
        optionSet.SetActive(false);
        menuSet.SetActive(true);
    }

    public void CloseButton()
    {
        optionSet.SetActive(false);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void HelpCloseButton()
    {
        helpSet.SetActive(false);
    }

    public void FullToggleClick()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // ȭ���� ������ �ʰ� �ϴ� �Լ�
        Screen.SetResolution(1920, 1080, true); //��üȭ��
        Debug.Log("��üȭ�� �����Ϸ�");
        staticDisplay = "��üȭ��";
    }

    public void WindowToggleClick()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ���� ������ �ʰ� �ϴ� �Լ�
        Screen.SetResolution(1280, 720, false); //â���
        Debug.Log("â��� �����Ϸ�");
        staticDisplay = "â���";
    }


    public void EasyToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Easy ���̵� �����Ϸ�");
        }
        else
        {

        }
    }

    public void NormalToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Normal ���̵� �����Ϸ�");
        }
        else
        {

        }
    }

    public void HardToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Hard ���̵� �����Ϸ�");
        }
        else
        {

        }
    }
}
