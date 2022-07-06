using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text text_Timer;

    float time_start;
    public float time_current; //�ð� ����
    float time_Max = 1000000;
    bool isEnded;
    // Start is called before the first frame update
    void Start()
    {
        Reset_Timer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnded)
            return;
        Check_Timer();
    }

    void Check_Timer() //�ð� �˻�
    {
        time_current = Time.time - time_start;
        if (time_current < time_Max)
        {
            text_Timer.text = $"{time_current:N2}";
        }
        else if (!isEnded)
        {
            End_Timer();
        }
    }

    void End_Timer() //����� ����
    {
        time_current = time_Max;
        text_Timer.text = $"{time_current:N2}";
        isEnded = true;
    }

    void Reset_Timer() //����
    {
        time_start = Time.time;
        time_current = 0;
        text_Timer.text = $"{time_current:N2}";
        isEnded = false;
    }
}
