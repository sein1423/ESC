using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public GameObject gameManagement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagement gm = gameManagement.GetComponent<GameManagement>();
            gm.GameClear();
        }
    }
}
