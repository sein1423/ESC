using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OtherPlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<PhotonView>().IsMine)
        {
            if(gameObject.GetComponent<PhotonView>().ViewID == 1001)
            {
                gameObject.transform.position = MultiplayManager.Player1Position;
                gameObject.transform.rotation = MultiplayManager.Player1Rotation;
            }
            else
            {
                gameObject.transform.position = MultiplayManager.Player2Position;
                gameObject.transform.rotation = MultiplayManager.Player2Rotation;
            }
        }
    }
}
