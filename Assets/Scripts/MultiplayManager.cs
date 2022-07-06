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
            GameObject player;
            player = Instantiate(playerPrefab) as GameObject;
            player.transform.position = new Vector3(0,0,-10);
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
        PhotonNetwork.Instantiate("Player", points[idx].position, Quaternion.identity);
    }
}
