using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public GameObject dbManager;
    public GameObject rankingPanel;
    public GameObject connectFailPanel;
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
            myRecord.text = GameManagement.staticPlayerName + "¥‘¿« ±‚∑œ: " + GameManagement.staticPlayTime;
            return;
        }
    }

    public async void RankButton()
    {
        rankingPanel.SetActive(true);

        DBManager db = dbManager.GetComponent<DBManager>();
        Text[] textArr = new Text[20] {text0, text1, text2, text3, text4, text5, text6, text7, text8, text9,
                            text10, text11, text12, text13, text14, text15, text16, text17, text18, text19};
        db.DBCommand("rank", "", "", "");
        await Task.Delay(1500);
        if (GameManagement.staticQueryResult == "connectFail")
        {
            rankingPanel.SetActive(false);
            connectFailPanel.SetActive(true);
            return;
        }
        else
        {
            string[] rankSplit = GameManagement.staticQueryResult.Split(',');
            for (int i = 0; i < 20; i++)
            {
                textArr[i].text = rankSplit[i];
            }
            return;
        }
    }

    public void RankingCloseButton()
    {
        rankingPanel.SetActive(false);
    }

    public void ConnectFailCloseButton()
    {
        connectFailPanel.SetActive(false);
    }
}
