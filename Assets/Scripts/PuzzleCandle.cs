using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PuzzleCandle : MonoBehaviourPunCallbacks
{
    GameObject fire;
    GameObject pairObject;
    GameObject puzzleDirector;
    public bool fire_state;
    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        fire = transform.GetChild(6).gameObject;
        pairObject = transform.GetChild(7).gameObject;
        puzzleDirector = GameObject.Find("PuzzleDirector");
    }

    // Update is called once per frame
    void Update()
    {
        Puzzle1();
    }

    public void Puzzle1()
    {
        PV = photonView;

        if (Input.GetButtonDown("Interaction") && GameManagement.staticGetLighter == true)
        {
            if (GameManagement.staticPlaymode == "soloplay" && GameManagement.staticPlaymode == null)
            {
                PuzzleControl();
            }
            else
            {
                if (PV.IsMine)
                {
                    PV.RPC("PuzzleControl", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    public void PuzzleControl()
    {
        if (fire.GetComponent<ParticleSystem>().isPlaying == true && GameManagement.staticFireState)
        {
            fire.GetComponent<ParticleSystem>().Stop();
            pairObject.GetComponent<Light>().enabled = false;
            puzzleDirector.GetComponent<PuzzleDirector>().Decrease();
        }
        else if (fire.GetComponent<ParticleSystem>().isPlaying == false && GameManagement.staticFireState)
        {
            fire.GetComponent<ParticleSystem>().Play();
            pairObject.GetComponent<Light>().enabled = true;
            puzzleDirector.GetComponent<PuzzleDirector>().Increase();
        }
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if(other.tag == "Player")
    //    {
    //        fire_state = true;
    //        Debug.Log("true");
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        fire_state = false;
    //        Debug.Log("false");
    //    }
    //}
}
