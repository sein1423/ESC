using UnityEngine;

public class Item : MonoBehaviour
{
    // enum: 열거형 타입(타입 이름 지정 필요)
    public enum Type
    {
        Ammo, Coin, Grenade, Heart, Weapon
    }
    public Type type;
    public int value;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}
