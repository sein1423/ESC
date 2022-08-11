using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDirector : MonoBehaviour
{
    int fireCount;
    public int fireNumber;
    GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        fireCount = 0;
        fireNumber = 10;
        door = GameObject.Find("door");
    }

    // Update is called once per frame
    void Update()
    {
        Complete();
    }

    public void Increase()
    {
        fireCount++;
        Debug.Log(fireCount);
    }

    public void Decrease()
    {
        fireCount--;
        Debug.Log(fireCount);
    }

    void Complete()
    {
        if(fireCount == fireNumber)
        {
            door.transform.Translate(Vector3.up * Time.deltaTime);
            if (door.transform.position.y > 25)
            {
                door.SetActive(false);
            }
        }
    }
}
