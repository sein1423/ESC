using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool activeInventory = false;
    public Player player;
    public GameObject lighter;

    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
            lighter.SetActive(player.hasWeapons);
        }

    }

}
