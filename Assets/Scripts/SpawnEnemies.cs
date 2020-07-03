using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
    public float gameStart = 0.0f;

    public float enemy1Time = 5.0f;
    public float enemy2Time = 15.0f;
    public float enemy3Time = 50.0f;
    public float randTime = 110f;
    private GameObject go;
    private SpawnGround sg;
    private PointArrows pa;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;


    private bool spawned1 = false;
    private bool spawned2 = false;
    private bool spawned3 = false;
    // Use this for initialization
    void Start () {
        gameStart = Time.fixedTime;

        go = GameObject.Find("SpawnGround");
        sg = go.GetComponent(typeof(SpawnGround)) as SpawnGround;
        go = GameObject.Find("ArrowIndicator");
        pa = go.GetComponent<PointArrows>();

    }

    // Update is called once per frame
    void Update () {

        if (Time.fixedTime - gameStart >= enemy1Time && !spawned1)
        {
            spawnEnemy1();
            spawned1 = true;
        }
        if (Time.fixedTime - gameStart >= enemy2Time && !spawned2)
        {
            spawnEnemy2();
            spawned2 = true;
        }
        if (Time.fixedTime - gameStart >= enemy3Time && !spawned3)
        {
            spawnEnemy3();
            spawned3 = true;
        }

        if(Time.fixedTime - gameStart >= randTime)
        {
            if (Random.Range(0, 2) == 0)
                spawnEnemy1();
            else
                spawnEnemy2();
            randTime = Random.Range(Time.fixedTime + 40, Time.fixedTime + 60);
        }


    }

    private void spawnEnemy1()
    {

        int xPos = sg.getSpawnX();
        int zPos = 25;

        GameObject instance = (GameObject)Instantiate(enemy1, sg.tiles[xPos, zPos].transform.position + new Vector3(0, 0.75f, 0), transform.rotation);

        pa.addEnemy(instance);

        EnemyMovement em = instance.GetComponent<EnemyMovement>();
        em.xPos = xPos;
        em.zPos = zPos;
        if (xPos == 1)
            em.direction = 1;
        else
            em.direction = 3;

    }
    private void spawnEnemy2()
    {

        int xPos = sg.getSpawnX();
        int zPos = 25;

        GameObject instance = (GameObject)Instantiate(enemy2, sg.tiles[xPos, zPos].transform.position + new Vector3(0, 0.75f, 0), transform.rotation);

        pa.addEnemy(instance);

        EnemyMovement2 em = instance.GetComponent<EnemyMovement2>();
        em.xPos = xPos;
        em.zPos = zPos;
        if (xPos == 1)
            em.direction = 1;
        else
            em.direction = 3;
    }
    private void spawnEnemy3()
    {

        int xPos = sg.getSpawnX();
        int zPos = 25;

        GameObject instance = (GameObject)Instantiate(enemy3, sg.tiles[xPos, zPos].transform.position + new Vector3(0, 0.75f, 0), transform.rotation);

        pa.addEnemy(instance);

        EnemyMovement3 em = instance.GetComponent<EnemyMovement3>();
        em.xPos = xPos;
        em.zPos = zPos;
        if (xPos == 1)
            em.direction = 1;
        else
            em.direction = 3;
    }

}
