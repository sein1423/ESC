using Photon.Pun;
using UnityEngine;
using PN = Photon.Pun.PhotonNetwork;

public class MultiplayManager : MonoBehaviourPunCallbacks
{
    public GameObject multiplayManager;

    void Awake()
    {
        PN.IsMessageQueueRunning = true;

        if (GameManagement.staticPlaymode != "multiplay")
        {
            Destroy(multiplayManager);
        }
        else if (GameManagement.staticPlaymode == "multiplay")
        {
            CreatePlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PN.Instantiate("Player", points[idx].position, Quaternion.identity);
    }
}
