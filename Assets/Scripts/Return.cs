using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour {

    private GameObject gm;

    private PlayerStatus ps;

    private InventoryManager im;
    private GameObject inventory;
    private void Start()
    {
        gm = GameObject.Find("GameManager");
        ps = gm.GetComponent(typeof(PlayerStatus)) as PlayerStatus;

        inventory = GameObject.Find("Inventory");
        im = inventory.GetComponent(typeof(InventoryManager)) as InventoryManager;
    }
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {

            while(im.returnEnergy())
            {
                ps.decPack(1);
                ps.spawnEnergy();
                ps.chargeUp(PlayerStatus.CHARGEAMOUNT);
            }
        }
    }
}
