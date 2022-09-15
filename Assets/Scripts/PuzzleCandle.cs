using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCandle : MonoBehaviour
{
    GameObject fire;
    GameObject pairObject;
    GameObject puzzleDirector;
    public bool fire_state;

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
        if (Input.GetButtonDown("Interaction"))
        {
            PuzzleControl();
        }
    }

    public void PuzzleControl()
    {
        if (fire.GetComponent<ParticleSystem>().isPlaying == true && fire_state)
        {
            fire.GetComponent<ParticleSystem>().Stop();
            pairObject.GetComponent<Light>().enabled = false;
            puzzleDirector.GetComponent<PuzzleDirector>().Decrease();
        }
        else if (fire.GetComponent<ParticleSystem>().isPlaying == false && fire_state)
        {
            fire.GetComponent<ParticleSystem>().Play();
            pairObject.GetComponent<Light>().enabled = true;
            puzzleDirector.GetComponent<PuzzleDirector>().Increase();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            fire_state = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            fire_state = false;
        }
    }
}
