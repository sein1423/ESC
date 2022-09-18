using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PN = Photon.Pun.PhotonNetwork;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject dbManager;
    public GameObject inputNamePanel;
    public GameObject playModePanel;
    public GameObject multiplayLobbyPanel;
    public GameObject makeRoomPanel;
    public GameObject searchRoomPanel;
    public GameObject roomPanel;
    public GameObject randomMatchFailPanel;
    public GameObject joinRoomFailPanel;
    public InputField makeRoomNameInput;
    public InputField searchRoomNameInput;
    public Text makeRoomNameOverlap;
    public Text searchRoomNameOverlap;
    public Text playerCountError;
    public Text roomText;
    public Text player1;
    public Text player2;
    public Text masterPlayer;
    public Text playerCount;
    public byte maxPlayer = 2;          //�ִ� �÷��̾� ��
    private string gameVersion = "1.0";

    void Awake()
    {
        PN.AutomaticallySyncScene = true;
        PN.GameVersion = this.gameVersion;
    }


    void Update()
    {
        if (PN.InRoom)   //���� �濡 ���� �ִ� ���¸� �ؽ�Ʈ ������Ʈ
        {
            Text[] arr = new Text[2] { player1, player2 };
            if(PN.PlayerList.Length == 1)
            {
                player1.text = "";
                player2.text = "";
            }
            else
            {
                for (int i = 0; i < PN.PlayerList.Length; i++)
                {
                    arr[i].text = PN.PlayerList[i].NickName;
                    GameManagement.staticMultiPlayerNames += PN.PlayerList[i].NickName + ",";
                }
            }
            playerCount.text = $"{PN.CurrentRoom.PlayerCount} / {PN.CurrentRoom.MaxPlayers}";
        }
        else
        {
            GameManagement.staticMultiPlayerNames = "";
        }
    }




    public void PlayModeCloseButton()
    {
        DBManager db = dbManager.GetComponent<DBManager>();
        db.DBCommand("deleteName", GameManagement.staticPlayerName, "", "");
        GameManagement.staticPlayerName = null;
        playModePanel.SetActive(false);
        inputNamePanel.SetActive(true);
    }
    public void SoloplayButton()
    {
        GameManagement.staticPlaymode = "soloplay";
        SceneManager.LoadScene("Game");
    }
    public void MultiplayButton()       //2�ο� ��ư�� ������ OnConnectedToMaster() �Լ� �ݹ�
    {
        GameManagement.staticPlaymode = "multiplay";
        PN.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()  //���������� ������ ���ӵǸ� �ݹ�Ǿ� ����Ǹ� OnJoinLobby() �Լ� ����
    {
        PN.LocalPlayer.NickName = GameManagement.staticPlayerName;
        PN.JoinLobby();
        multiplayLobbyPanel.SetActive(true);
    }
    public void MultiplayCloseButton()       //�κ񿡼� Ŭ���� ��ư ������ �� OnDisconnected() �Լ� �ݹ�
    {
        PN.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)  //�������� �������� �ݹ�Ǿ� ����
    {
        GameManagement.staticPlaymode = null;
        multiplayLobbyPanel.SetActive(false);
        playModePanel.SetActive(true);
    }



    public void MakeRoomButton()    //�κ񿡼� MakeRoom��ư ������ ��
    {
        makeRoomPanel.SetActive(true);
        makeRoomNameOverlap.text = "";
    }   
    public void MakeRoomButton2()   //MakeRoom �гο��� ���� �游��� ��ư ��������
    {
        if (makeRoomNameInput.text == "")
        {
            makeRoomNameOverlap.text = "���̸��� �Է��ϼ���";
            return;
        }

        if (makeRoomNameInput.text.Length > 10)
        {
            makeRoomNameOverlap.text = "10���� ���Ϸ� �Է����ּ���";
            return;
        }
        CreateRoom();
    }
    public void CreateRoom()    //�游��� ��ư�� ������ �� ȣ��Ǵ� �Լ�, OnCreatedRoom �Լ� �ݹ�
    {
        PN.JoinOrCreateRoom(makeRoomNameInput.text, new RoomOptions { MaxPlayers = maxPlayer }, null);
    }
    public void MakeRoomCancel()    //MakeRoom �гο��� ��ҹ�ư ������ ��
    {
        makeRoomPanel.SetActive(false);
    }
    public override void OnJoinedRoom()     //�濡 ���������� �ݹ�Ǵ� �Լ�
    {
        roomPanel.SetActive(true);
        roomText.text = $"�� �̸�:  {PN.CurrentRoom.Name}";
        playerCountError.text = "";
        makeRoomNameInput.text = "";
        makeRoomNameOverlap.text = "";
        makeRoomPanel.SetActive(false);
        searchRoomNameInput.text = "";
        searchRoomNameOverlap.text = "";
        searchRoomPanel.SetActive(false);
    }



    public void SearchRoomButton()    
    {
        searchRoomPanel.SetActive(true);
        searchRoomNameOverlap.text = "";
    }
    public void SearchRoomButton2()   
    {
        if (searchRoomNameInput.text == "")
        {
            searchRoomNameOverlap.text = "���̸��� �Է��ϼ���";
            return;
        }
        if (searchRoomNameInput.text.Length > 10)
        {
            searchRoomNameOverlap.text = "10���� ���Ϸ� �Է����ּ���";
            return;
        }
        JoinSearchRoom();
    }
    public void JoinSearchRoom()      
    {
        PN.JoinRoom(searchRoomNameInput.text);
    }
    public void SearchRoomCancel()    
    {
        searchRoomPanel.SetActive(false);
    }



    public void RandomMatch()       //������Ī ��ư Ŭ�� ��
    {
        JoinRandomRoom();
    }
    public void JoinRandomRoom()    //������ ���� �ִٸ� �������� ��Ī�ȴ�
    {
        PN.JoinRandomRoom();
    }



    public void RoomOutButton()     //�濡�� ������ ��ư
    {
        LeaveRoom();
        roomPanel.SetActive(false);
    }
    public void LeaveRoom()     //�� ������ �Լ�
    {
        PN.LeaveRoom();
    }



    public override void OnCreateRoomFailed(short returnCode, string message)       //�� ����� �������� �� �ݹ�Ǵ� �Լ�
    {
        makeRoomNameOverlap.text = "�ߺ��� �� �̸��Դϴ�";
    }
    public override void OnJoinRoomFailed(short returnCode, string message)     //�� ���� ���н� �ݹ�Ǵ� �Լ�
    {
        joinRoomFailPanel.SetActive(true);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)   //���� �� ����
    {
        randomMatchFailPanel.SetActive(true);
    }
    public void RandomMatchFailCloseButton()    //���� ��Ī ���еǰ� ������ �г� ��� ��ư
    {
        randomMatchFailPanel.SetActive(false);
    }
    public void JoinRoomFailCloseButton()    //�� ���忡 ���еǰ� ������ �г� ��� ��ư
    {
        joinRoomFailPanel.SetActive(false);
    }
    static public void StaticNetworkDisconnect()    //���� �����߿� ���ӿ��� ���� �� ����� �Լ�
    {
        PN.Disconnect();
    }



    public void GameStartButton()    //���� ����
    {
        if (PN.CurrentRoom.PlayerCount == maxPlayer && PN.IsMasterClient)
        {
            //PN.IsMessageQueueRunning = false;
            SceneManager.LoadScene("Game");
        }
        else if(PN.CurrentRoom.PlayerCount == maxPlayer && !PN.IsMasterClient)
        {
            playerCountError.text = "���常 ������ ������ �� �ֽ��ϴ�";
        }
        else if (PN.CurrentRoom.PlayerCount != PN.CurrentRoom.MaxPlayers)
        {
            playerCountError.text = "�÷��̾� ���� 2���� �Ǿ�� ������ ���۵˴ϴ�";
        }
    }
}