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
        Timer1 = GameObject.Find("Timer_text1"); //������Ʈ Timer_text1�� ������
        time_current = Timer1.GetComponent<Timer>().time_current; // Timer_text1 ������Ʈ�� Timer ��ũ��Ʈ ���� time_current��� ������ ������
        Timer_Text.text = $"���: {time_current:N2}"; //�� ������ �ؽ�Ʈ�� ����
        Destroy(GameObject.Find("Timer_text1")); //Ÿ�̸� ������ ����(�ʱ�ȭ)
        

    }

    void Update()
    {
   
    }

   
}
