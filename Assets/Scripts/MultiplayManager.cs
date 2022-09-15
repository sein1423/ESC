using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayManager : MonoBehaviourPunCallbacks
{
    public GameObject multiplayManager;
    public GameObject playerPrefab;

    void Start()
    {
        //PN.IsMessageQueueRunning = true;

        if (GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null)
        {
            GameObject go;
            go = Instantiate(playerPrefab) as GameObject;
            go.transform.position = GameObject.Find("OVRCameraRig").transform.position + (Vector3.down*7f);
            go.transform.parent = GameObject.Find("OVRCameraRig").transform;
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
        go.transform.position = GameObject.Find("OVRCameraRig").transform.position;
        go.transform.parent = GameObject.Find("OVRCameraRig").transform;
    }
}
