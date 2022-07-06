using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public GameObject dbManager;
    public GameObject rankingPanel;
    public GameObject connectFailPanel;
    public Text myRecord;

    // Start is called before the first frame update
    void Awake()
    {
        MyRecord();
    }

    void MyRecord()
    {
        if (GameManagement.staticPlayTime == null)
            return;
        else
        {
            myRecord.text = GameManagement.staticPlayerName + "���� ���: " + GameManagement.staticPlayTime;
            return;
        }
    }


    public void RankButton()
    {
        rankingPanel.SetActive(true);
        DBManager db = dbManager.GetComponent<DBManager>();
        db.DBCommand("rank", "", "", "");
    }


    public void RankingCloseButton()
    {
        rankingPanel.SetActive(false);
    }

    public void ConnectFailCloseButton()
    {
        connectFailPanel.SetActive(false);
        rankingPanel.SetActive(false);
    }
}
