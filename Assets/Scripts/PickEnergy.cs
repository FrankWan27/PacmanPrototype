using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickEnergy : MonoBehaviour
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

            if (im.pickUpEnergy())
            {
                Movement mv = other.GetComponent<Movement>();

                ps.removeEnergy(mv.xPos, mv.zPos);
                ps.incPack(1);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
        else if(other.tag == "enemy3")

        {

            EnemyMovement3 mv = other.GetComponent<EnemyMovement3>();
            ps.removeEnergy(mv.xPos, mv.zPos);
            mv.charged = true;
            Renderer r = other.GetComponent<Renderer>();
            r.material.color = Color.yellow;
            gameObject.SetActive(false);
        }
    }
}
