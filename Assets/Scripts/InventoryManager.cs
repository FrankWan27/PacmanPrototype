using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class InventoryManager : MonoBehaviour
{
    public class Item
    {
        public string name;
        public Sprite image;
    }

    public Movement mv;
    public SpawnGround sg;
    public Item[] energies = new Item[3];
    public Item[] powerups = new Item[3];
    public Image[] energyImage = new Image[3];
    public Image[] powerupImage = new Image[3];
    public Sprite blank;
    public Sprite energy;
    public Sprite accel;
    public Sprite trap;
    public GameObject trapPrefab;

    // Use this for initialization
    void Start()
    {
        energies = new Item[3];
        powerups = new Item[3];
        for (int i = 0; i < 3; i++)
        {
            energies[i] = new Item
            {
                name = "empty",
                image = blank
            };
            energyImage[i].sprite = energies[i].image;

            powerups[i] = new Item
            {
                name = "empty",
                image = blank
            };
            powerupImage[i].sprite = powerups[i].image;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool pickUpEnergy()
    {
        return pickUp("energy", energy, energies, energyImage);
    }

    public bool pickUpAccel()
    {
        return pickUp("accel", accel, powerups, powerupImage);

    }

    public bool pickUpTrap()
    {
        return pickUp("trap", trap, powerups, powerupImage);

    }

    private bool pickUp(string name, Sprite image, Item[] list, Image[] images)
    {
        for (int i = 0; i < 3; i++)
        {
            if (list[i].name.Equals("empty"))
            {
                list[i].name = name;
                list[i].image = image;
                images[i].sprite = list[i].image;

                return true;
            }
        }
        //no more space
        return false;
    }

    public bool returnEnergy()
    {
        for(int i = 2; i >= 0; i--)
        {
            if(energies[i].name.Equals("energy"))
            {
                energies[i].name = "empty";
                energies[i].image = blank;
                energyImage[i].sprite = energies[i].image;

                return true;
            }
        }
        return false;

    }

    public void buttonPress(int slot)
    {
        if(powerups[slot].name.Equals("accel"))
        {
            resetSlot(slot);
            mv.accelerate();
        }
        else if(powerups[slot].name.Equals("trap"))
        {
            if(dropTrap())
                resetSlot(slot);
        }
    }

    private void resetSlot(int slot)
    {
        powerups[slot].name = "empty";
        powerups[slot].image = blank;
        powerupImage[slot].sprite = powerups[slot].image;
    }

    public bool dropTrap()
    {

        if (sg.tileInfo[mv.xPos, mv.zPos] == 1)
        {
            GameObject instance = (GameObject)Instantiate(trapPrefab, mv.transform.position, transform.rotation);
            return true;
        }
        return false;

    }

}
