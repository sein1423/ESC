using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    public GameObject helpSet;
    public GameObject rankingSet;
    public GameObject configurationSet;
    public GameObject menuSet;
    public GameObject optionSet;
    public GameObject timeManager;
    public GameObject dbManager;

    public static string staticPlayerName;
    public static string staticPlayTime;
    public static string staticDisplay;
    public static string staticQueryResult;
    public static string staticPlaymode;

    private void Awake()
    {

    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) //esc 버튼을 눌렀을 때
        {
            if (menuSet.activeSelf) //켜져있으면
            {
                menuSet.SetActive(false); //끔
                Time.timeScale = 1.0f; //시간을 다시 재생
            }
            else
            {                    //꺼져있으면
                menuSet.SetActive(true); //킴
                Time.timeScale = 0f; //시간을 멈춤
            }

            if (optionSet.activeSelf) //사운드메뉴가 켜져있으면
            {
                optionSet.SetActive(false); //끔
                Time.timeScale = 0f; //시간을 멈춤
            }

            Time.fixedDeltaTime = 0.02f * Time.timeScale; //fixedUpdate 프레임 시간도 함께 바꿔줘야함
        }

        if (staticDisplay == "전체화면")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1920, 1080, true); //전체화면

        }
        else if (staticDisplay == "창모드")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1280, 720, false); //창모드

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
        SceneManager.LoadScene("SampleScene");
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

    public async void GameClear()
    {
        DBManager db = dbManager.GetComponent<DBManager>();
        TimeManager timer = timeManager.GetComponent<TimeManager>();

        string timeNum = $"{timer.time_current:N2}";    //현재 타임을 소수점 두번째자리 까지만 가져옴
        float timeFloat = float.Parse(timeNum);           //형변환
        int timeInt = Mathf.RoundToInt(timeFloat);        //반올림
        string minute = (timeInt / 60).ToString();      //분 계산
        string second = (timeInt % 60).ToString();      //초 계산
        string timeStr = minute + "m" + second + "s";
        db.DBCommand("updateTime", staticPlayerName, timeStr, timeNum);      //플레이타임을 업데이트하는 DB함수
        await Task.Delay(1000);
        staticPlayTime = timeStr;

        if (staticQueryResult == "success")
        {
            SceneManager.LoadScene("Escape");
            Destroy(GameObject.Find("SoundManager"));
        }
        else
        {
            SceneManager.LoadScene("Escape");
            Destroy(GameObject.Find("SoundManager"));
        }
    }

    public void TestCharacter()
    {
        SceneManager.LoadScene("SampleScene");
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
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않게 하는 함수
        Screen.SetResolution(1920, 1080, true); //전체화면
        Debug.Log("전체화면 설정완료");
        staticDisplay = "전체화면";
    }

    public void WindowToggleClick()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면이 꺼지지 않게 하는 함수
        Screen.SetResolution(1280, 720, false); //창모드
        Debug.Log("창모드 설정완료");
        staticDisplay = "창모드";
    }


    public void EasyToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Easy 난이도 설정완료");
        }
        else
        {

        }
    }

    public void NormalToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Normal 난이도 설정완료");
        }
        else
        {

        }
    }

    public void HardToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Hard 난이도 설정완료");
        }
        else
        {

        }
    }
}
