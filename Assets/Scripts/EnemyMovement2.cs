using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement2 : MonoBehaviour
{

    public int xPos = 18;
    public int zPos = 18;
    private GameObject go;
    private SpawnGround sg;
    public float speed = 0.1f;
    private Movement player;
    private GameObject gm;
    private Health health;
    private Navigation nav;
    private bool stunned = false;
    private float stunnedStart;


    public int direction;

    // Use this for initialization
    void Start()
    {
        gm = GameObject.Find("GameManager");
        health = gm.GetComponent(typeof(Health)) as Health;
        nav = gm.GetComponent<Navigation>();

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
            
        if (xPos == player.xPos && zPos == player.zPos)
        {
            health.health--;
            // gameObject.SetActive(false);
            player.respawn();
        }

        if (dest != transform.position)
        {
            Vector3 p = Vector3.MoveTowards(transform.position, dest, speed);
            GetComponent<Rigidbody>().MovePosition(p);

        }
        else
        {
            if (stunned)
            {
                if (Time.fixedTime >= stunnedStart + 0.5f)
                {
                    stunned = false;
                }
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
                    int[,] prev = nav.shortestPath(xPos, zPos, player.xPos, player.zPos);
                 
                    int endX = player.xPos;
                    int endZ = player.zPos;
                    int nextDir = -1;

                    while (prev[endX, endZ] != (zPos * sg.sizeX + xPos))
                    { 
                        int prevIndex = prev[endX, endZ];
                        endX = prevIndex % sg.sizeX;
                        endZ = prevIndex / sg.sizeX;
                        if (prevIndex == -1)
                        {
                            nextDir = direction;
                            break;
                        }
                    }
                    //endX,Z is now the next tile it must go to
                    for (int i = 0; i < 4; i++)
                    {
                        int nextX = xPos + (int)DirectionToVector(i).x;
                        int nextZ = zPos + (int)DirectionToVector(i).z;

                        if (nextX == endX && nextZ == endZ)
                            nextDir = i;
                    }

                    if (direction != nextDir)
                    {
                        direction = nextDir;
                        stunned = true;
                        stunnedStart = Time.fixedTime;
                    }
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