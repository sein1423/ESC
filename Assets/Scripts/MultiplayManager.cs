using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayManager : MonoBehaviourPunCallbacks
{
    public GameObject multiplayManager;
    public GameObject playerPrefab;

    public static Vector3 Player1Position;
    public static Quaternion Player1Rotation;
    public static Vector3 Player2Position;
    public static Quaternion Player2Rotation;


    private void Awake()
    {
        Player1Position = Vector3.zero;
        Player1Rotation = Quaternion.identity;
        Player2Position = Vector3.zero;
        Player2Rotation = Quaternion.identity;
    }
    void Start()
    {
        //PN.IsMessageQueueRunning = true;

        if (GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null)
        {
            GameObject go;
            go = Instantiate(playerPrefab) as GameObject;
            go.transform.position = GameObject.Find("OVRCameraRig").transform.position + (Vector3.down*7f);
            go.transform.parent = GameObject.Find("OVRCameraRig").transform;
            go.tag = "Player";
            go.transform.parent.gameObject.AddComponent<MultiPlayer>();
            go.layer = 10;
            Destroy(multiplayManager);
        }
        else if(GameManagement.staticPlaymode == "multiplay")
        {
            CreateMultiPlayer();
        }
    }
    void CreateMultiPlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        GameObject go =  PhotonNetwork.Instantiate("Player", points[idx].position, Quaternion.identity);
        go.transform.position = GameObject.Find("OVRCameraRig").transform.position + (Vector3.down * 7f);
        go.transform.parent = GameObject.Find("OVRCameraRig").transform;
        go.tag = "Player";
        if (go.GetComponent<PhotonView>().IsMine)
        {
            go.AddComponent<MultiPlayer>();
            go.layer = 10;
        }
        else
        {
            go.AddComponent<OtherPlayer>();
        }
    }
}
