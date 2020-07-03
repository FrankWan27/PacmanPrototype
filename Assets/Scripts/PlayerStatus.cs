using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    public class Energy
    {
        public int xPos;
        public int zPos;
        public Vector3 position;
    }

    public int sizeX = 50;
    public int sizeZ = 50;

    public static int CHARGEAMOUNT = 50;

    public int packages = 0;
    public TimerManager tm;
    public GameObject energyPrefab;
    public GameObject accelPrefab;
    public GameObject trapPrefab;
    public GameObject returnButton;
    public float charge = 100;

    private float nextEnergy = 20f;
    private float nextPower = 5.0f;

    private GameObject go;
    private SpawnGround sg;

    public List<Energy> energies;

    private void Start()
    {
        Application.targetFrameRate = 60;

        go = GameObject.Find("SpawnGround");
        sg = go.GetComponent(typeof(SpawnGround)) as SpawnGround;

        energies = new List<Energy>();
        //fixes shading problems
        DynamicGI.UpdateEnvironment();

        spawnEnergy();
        spawnEnergy();
        spawnEnergy();
        spawnEnergy();
        spawnAccel();
        spawnAccel();
        spawnTrap();
    }

    public void gameOver()
    {
        GameObject instance = (GameObject)Instantiate(returnButton, new Vector3(0, 0, 0), transform.rotation);

    }

    public void chargeUp(int amount)
    {
        charge += amount;
    }

    public void incPack(int PID)
    {
        packages++;
    }

    public void decPack(int PID)
    {
        packages--;
    }

    public void spawnEnergy()
    {
        Energy newEnergy = getFreeLoc();
        GameObject instance = (GameObject)Instantiate(energyPrefab, newEnergy.position, transform.rotation);
        energies.Add(newEnergy);
    }

    public void spawnEnergy(int x, int z, Vector3 pos)
    {
        Energy newEnergy = new Energy
        {
            xPos = x,
            zPos = z,
            position = pos
        };
        GameObject instance = (GameObject)Instantiate(energyPrefab, pos, transform.rotation);
        energies.Add(newEnergy);
    }


    public void removeEnergy(int x, int z)
    {
        for(int i = 0; i < energies.Count; i++)
        {
            Energy energy = energies[i];
            if(energy.xPos == x && energy.zPos == z)
            {
                energies.Remove(energy);
                Debug.Log("REMOVED");
                return;
            }
        }
    }

    public void spawnAccel()
    {
        Vector3 newPos = getFreeLoc().position;
        GameObject instance = (GameObject)Instantiate(accelPrefab, newPos, transform.rotation);
        
    }

    public void spawnTrap()
    {
        Vector3 newPos = getFreeLoc().position;
        GameObject instance = (GameObject)Instantiate(trapPrefab, newPos, transform.rotation);
    }

    public Energy getFreeLoc()
    {
        int randomX = 4 + (int)(Random.value * (sizeX - 9));
        int randomZ = 6 + (int)(Random.value * (sizeZ - 13));

        
        if(sg.tileInfo[randomX, randomZ] != 1)
        {
            //Debug.Log(randomX + " " + randomZ + " is in a wall");
            return getFreeLoc();
        }

        if (Physics.Linecast(sg.tiles[randomX, randomZ].transform.position, sg.tiles[randomX, randomZ].transform.position + Vector3.up))
        {
            Debug.Log(randomX + " " + randomZ + " is colliding with another object");
            return getFreeLoc();
        }

        Vector3 newPos = sg.tiles[randomX, randomZ].transform.position;
        newPos.y = 1;

        Energy item = new Energy
        {
            xPos = randomX,
            zPos = randomZ,
            position = newPos
        };
        return item;
    }

    private void Update()
    {
        if (charge > 0)
        {
            charge -= 0.05f;
        }

        if (Time.fixedTime >= nextPower)
        {
            nextPower = Time.fixedTime + Random.Range(10f, 20f);
            int rand = Random.Range(0, 2);

            if(rand == 0)

                spawnAccel();
            else
                spawnTrap();

        }

        if (Time.fixedTime >= nextEnergy)
        {
            nextEnergy = Time.fixedTime + Random.Range(15f, 20f);
            spawnEnergy();
        }
    }
}
