using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PhotonNetwork;
using PV = Photon.Pun.PhotonView;

public class Camcontroller : MonoBehaviourPunCallbacks
{
    public Transform cam;
    public GameObject player;
    string playername;
    // Start is called before the first frame update
    void Start()
    {

        cam = gameObject.transform;
        player = gameObject.transform.parent.gameObject;
        playername = player.GetComponent<MultiPlayer>().photonView.Owner.NickName;
        if (!player.GetComponent<MultiPlayer>().photonView.IsMine)
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    
    void Update()
    {
        if (playername == GameManagement.staticPlayerName || GameManagement.staticPlaymode == "soloplay")
        {
            cam.position = new Vector3(player.transform.position.x, player.transform.position.y+3.5f, player.transform.position.z);
            cam.rotation = player.transform.rotation;
        }
    }
}
