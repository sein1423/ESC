using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon};
    public Type type;
    public int value;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.right * 10 * Time.deltaTime);
    }
}
