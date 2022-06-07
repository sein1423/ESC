using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeManager : MonoBehaviour
{

    GameObject Timer1;
    public Text Timer_Text;
    float time_current;
    float time_start;

    void Start()
    {
        Timer1 = GameObject.Find("Timer_text1"); //오브젝트 Timer_text1을 가져옴
        time_current = Timer1.GetComponent<Timer>().time_current; // Timer_text1 오브젝트의 Timer 스크립트 안의 time_current라는 변수를 가져옴
        Timer_Text.text = $"기록: {time_current:N2}"; //그 변수를 텍스트에 적용
        Destroy(GameObject.Find("Timer_text1")); //타이머 변수를 삭제(초기화)
        

    }

    void Update()
    {
   
    }

   
}
