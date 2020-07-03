using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement3 : MonoBehaviour
{

    public int xPos = 45;
    public int zPos = 12;
    private GameObject go;
    private SpawnGround sg;
    public float speed = 0.05f;
    private Movement player;
    private GameObject gm;
    private PlayerStatus ps;
    private Navigation nav;
    private Health health;
    public bool charged;

    public int direction;

    // Use this for initialization
    void Start()
    {
        charged = false;
        gm = GameObject.Find("GameManager");
        health = gm.GetComponent(typeof(Health)) as Health;
        nav = gm.GetComponent<Navigation>();
        ps = gm.GetComponent<PlayerStatus>();
        go = GameObject.Find("SpawnGround");
        sg = go.GetComponent(typeof(SpawnGround)) as SpawnGround;
        go = GameObject.Find("Player");
        player = go.GetComponent<Movement>();
    }
    public void respawn()
    {
        xPos = 5;
        zPos = 24;
        direction = 1;
        transform.position = new Vector3(-11.5f, 1, 7.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dest = sg.tiles[xPos, zPos].transform.position;
        dest.y = transform.position.y;
        if (charged)
            speed = 0.1f;
        else
            speed = 0.05f;

        if (xPos == player.xPos && zPos == player.zPos)
        {
            //health.health--;
            // gameObject.SetActive(false);
            //player.respawn();
        }
        if(zPos == 25 && (xPos == 1 || xPos == 48))
        {
            charged = false;
            Renderer r = GetComponent<Renderer>();
            r.material.color = new Color(0.7169812f, 0.7169812f, 0.3415807f);
            direction += 2;
        }

        if (dest != transform.position)
        {
            Vector3 p = Vector3.MoveTowards(transform.position, dest, speed);
            GetComponent<Rigidbody>().MovePosition(p);

        }
        else
        {
            if (valid(direction))
            {
                xPos += (int)DirectionToVector(direction).x;
                zPos += (int)DirectionToVector(direction).z;
            }

            //check at intersection
            int routes = 0;
            for (int i = 0; i < 4; i++)
                if (i != direction)
                    if (valid(i))
                        routes++;




            if (routes >= 2)
            {
                int targetX = 0;
                int targetZ = 0;
                if (charged)
                {
                    //go to closest exit
                    float distLeft = Mathf.Sqrt(Mathf.Pow(xPos - 0, 2) + Mathf.Pow(zPos - 25, 2));
                    float distRight = Mathf.Sqrt(Mathf.Pow(xPos - 49, 2) + Mathf.Pow(zPos - 25, 2));

                    targetZ = 25;

                    if (distLeft < distRight)
                        targetX = 1;
                    else
                        targetX = 48;
                }
                else
                {
                    //go to closest energy
                    PlayerStatus.Energy closest = new PlayerStatus.Energy();
                    float dist = float.MaxValue;
                    foreach (PlayerStatus.Energy energy in ps.energies)
                    {
                        float newdist = Mathf.Sqrt(Mathf.Pow(xPos - energy.xPos, 2) + Mathf.Pow(zPos - energy.zPos, 2));
                        if (newdist < dist)
                        {
                            dist = newdist;
                            closest = energy;
                        }
                    }

                    targetX = closest.xPos;
                    targetZ = closest.zPos;
                }


                int[,] prev = nav.shortestPath(xPos, zPos, targetX, targetZ);

                int endX = targetX;
                int endZ = targetZ;
                while (prev[endX, endZ] != (zPos * sg.sizeX + xPos))
                {

                    int prevIndex = prev[endX, endZ];
                    endX = prevIndex % sg.sizeX;
                    endZ = prevIndex / sg.sizeX;
                }
                //endX,Z is now the next tile it must go to

                int nextDir = -1;
                for (int i = 0; i < 4; i++)
                {
                    int nextX = xPos + (int)DirectionToVector(i).x;
                    int nextZ = zPos + (int)DirectionToVector(i).z;

                    if (nextX == endX && nextZ == endZ)
                        direction = i;
                }
            }
        }
    }

    private bool valid(int direction)
    {
        int dirX = (int)DirectionToVector(direction).x;
        int dirZ = (int)DirectionToVector(direction).z;

        return sg.tileInfo[xPos + dirX, zPos + dirZ] == 1;
    }

    Vector3 DirectionToVector(int direction)
    {
        Vector3 result;
        switch (direction % 4)
        {
            case 0:
                result = Vector3.forward;
                break;
            case 1:
                result = Vector3.right;
                break;
            case 2:
                result = Vector3.back;
                break;
            case 3:
                result = Vector3.left;
                break;
            default:
                result = Vector3.zero;
                break;
        }
        return result;
    }
}