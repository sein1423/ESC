using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Text tutorial;
    public GameObject timeManager;

    // Start is called before the first frame update
    void Start()
    {
        tutorial.text = "������ ��Ʈ�ѷ��� ������ ���ϰ� \n ���� ��Ʈ�ѷ��� �̿��� ������ ������ \n ����Ʈ ��ư�� ���� �޸� ���� �ֽ��ϴ�";
    }

    // Update is called once per frame
    void Update()
    {
        if (10.0f >= timeManager.GetComponent<TimeManager>().GetCurrentTime())
        {
            tutorial.text = "�����Ϳ� �ٰ��� F ��ư�� ������ ȹ���� �� �ֽ��ϴ� \n G ��ư�� ���� �����͸� �Ѱ� �� �� �ֽ��ϴ�";
        }
        else if (20.0f >= timeManager.GetComponent<TimeManager>().GetCurrentTime() && 10.0f < timeManager.GetComponent<TimeManager>().GetCurrentTime())
        {
            tutorial.text = "�д뿡 �ٰ��� E ��ư�� ������ ���� ���� �� �ֽ��ϴ�. \n ���͸� ���� 10���� �к��� ��� �Ѹ� Ż�ⱸ�� �����ϴ� ����� ���ϴ�";
        }
        else if (30.0f > timeManager.GetComponent<TimeManager>().GetCurrentTime() && 20.0f < timeManager.GetComponent<TimeManager>().GetCurrentTime())
        {
            tutorial.text = "";
        }
    }
}
