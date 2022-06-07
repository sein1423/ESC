using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public GameObject dbManager;
    public Text myRecord;
    public Text text0;
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Text text6;
    public Text text7;
    public Text text8;
    public Text text9;
    public Text text10;
    public Text text11;
    public Text text12;
    public Text text13;
    public Text text14;
    public Text text15;
    public Text text16;
    public Text text17;
    public Text text18;
    public Text text19;

    // Start is called before the first frame update
    void Start()
    {
        MyRecord();
        Rank();
    }

    void MyRecord()
    {
        if (GameManagement.staticPlayTime == null)
            return;
        else
            myRecord.text = GameManagement.staticPlayerName + "님의 기록: " + GameManagement.staticPlayTime;
    }

    void Rank()
    {
        DB db = dbManager.GetComponent<DB>();
        string[] rankDB = new string[10];

        db.DBDeleteToTime("100분0초");   //닉네임 입력 시 100분0초로 초기화 해놨는데 만일 게임 클리어를 못해 시간이 변경되지 못할 경우 DB 삭제
        rankDB = db.DBSelectRank(10);     //name컬럼과 timeStr컬럼을 조회하는데 timeNum의 오름차순으로 정렬하고 10개까지만 조회

        Text[] textArr = new Text[20] {text0, text1, text2, text3, text4, text5, text6, text7, text8, text9, 
                            text10, text11, text12, text13, text14, text15, text16, text17, text18, text19};
        
        for (int i = 0; i < rankDB.Length; i++)
        {
            textArr[i].text = rankDB[i];
        }
    }
}
