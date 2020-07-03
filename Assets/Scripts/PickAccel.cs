using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAccel : MonoBehaviour
{
    private GameObject gm;
    private PlayerStatus ps;
    private InventoryManager im;

    private GameObject inventory;
    // Use this for initialization

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        ps = gm.GetComponent(typeof(PlayerStatus)) as PlayerStatus;

        inventory = GameObject.Find("Inventory");
        im = inventory.GetComponent(typeof(InventoryManager)) as InventoryManager;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            if (im.pickUpAccel())
            {
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
    }
}
