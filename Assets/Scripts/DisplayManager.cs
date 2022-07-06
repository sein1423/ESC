using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private Toggle FullDisplay = null;

    public GameObject displayManager;

    void Start()
    {
        LoadValues();
        
    }

    public void SaveToggleButton()
    {
        if (GameManagement.staticDisplay == "��üȭ��")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1920, 1080, true); //��üȭ��
            PlayerPrefs.SetString("DisplayValue", GameManagement.staticDisplay);
            

        }
        else if (GameManagement.staticDisplay == "â���")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1280, 720, false); //â���
            PlayerPrefs.SetString("DisplayValue", GameManagement.staticDisplay);
            Debug.Log(GameManagement.staticDisplay);

        }
        LoadValues();
    }

    void LoadValues()
    {
        string displayValue = PlayerPrefs.GetString("DisplayValue");
        if(displayValue == "��üȭ��")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1920, 1080, true); //��üȭ��
            FullDisplay.GetComponent<Toggle>().isOn = true;
        }
        else if (displayValue == "â���")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1280, 720, false); //â���
            FullDisplay.GetComponent<Toggle>().isOn = false;
        }
    }
}
