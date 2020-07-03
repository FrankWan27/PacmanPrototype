using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public bool stunned = false;
    public float stunnedStart = 0.0f;
    public float accelStart = 0.0f;
    public int direction;
    public int prevdir;
    public Rigidbody rb;
    public float speed = 1f;

    public FollowPlayer camera;
    public GameObject stunnedUI;
    public Health health;
    private GameObject go;
    private SpawnGround sg;
    public int xPos = 25;
    public int zPos = 25;

    private bool aKey = false;
    private bool dKey = false;

    private bool queueIncDir = false;
    private bool queueDecDir = false;

    private bool justTurned = false;
    private bool inMotion = true;

    private int prepTurn = -1;
    private float speedMult = 1;
    private bool accel = false;
    // Use this for initialization
    void Start () {
        go = GameObject.Find("SpawnGround");
        sg = go.GetComponent(typeof(SpawnGround)) as SpawnGround;
    }
	
    public void respawn()
    {
        xPos = 25;
        zPos = 25;
        direction = 0;
        transform.position = new Vector3(0, 1, 0);
        camera.changeDirection(direction);
    }

    public void accelerate()
    {
        accel = true;
        accelStart = Time.fixedTime;
        speedMult *= 2;  
    }

	// Update is called once per frame
	void FixedUpdate ()
    {

        if(speedMult > 1 && Time.fixedTime >= accelStart + 2)
        {
            speedMult = 1;
            accel = false;


        }

        if (stunned)
        {
            if (Time.fixedTime >= stunnedStart + 1)
            {
                stunned = false;
                stunnedUI.SetActive(false);
                if (justTurned)
                {
                    //direction = prevdir;
                    //changeDirection();
                }
            }
       
        }
        else
        {
            bool turned = false;

            if (Input.GetKey("d"))
            {
                if (!dKey)
                {
                    dKey = true;
                    queueIncDir = true;
                }
            }
            else
            {
                dKey = false;
            }


            if (Input.GetKey("a"))
            {
                if (!aKey)
                {
                    aKey = true;
                    queueDecDir = true;
                }
            }
            else
            {
                aKey = false;
            }



            if (queueIncDir)
            {
                queueIncDir = false;
                prevdir = direction;

                direction++;
//                changeDirection();
                turned = true;
            }

            if (queueDecDir)
            {
                queueDecDir = false;
                prevdir = direction;
                direction--;
                if (direction < 0) direction += 4;
//                changeDirection();
                turned = true;
            }

            if (turned) justTurned = true;

            //rb.AddForce(forwardForce * Time.deltaTime * DirectionToVector(direction), ForceMode.VelocityChange);
            //transform.Translate(DirectionToVector(direction) * forwardForce * Time.deltaTime, Space.World);
            Vector3 dest = sg.tiles[xPos, zPos].transform.position;
            dest.y = transform.position.y;

            if(dest != transform.position)
            {
                inMotion = true;
                Vector3 p = Vector3.MoveTowards(transform.position, dest, speed * speedMult);
                GetComponent<Rigidbody>().MovePosition(p);

            }
            else
            {

                if (valid(xPos, zPos))
                {
                    justTurned = false;

                    xPos += (int)DirectionToVector(direction).x;
                    zPos += (int)DirectionToVector(direction).z;
                    if (prepTurn != -1)
                    {
                        Debug.Log("prevdir " + direction + " direction " + direction);

                        prevdir = direction;
                        direction = prepTurn;
                        prepTurn = -1;
                        justTurned = true;
                    }
                }
                else
                {
                    //check for window
                    int newX = xPos + (int)DirectionToVector(prevdir).x;
                    int newZ = zPos + (int)DirectionToVector(prevdir).z;
                    if (justTurned && valid(newX, newZ))
                    {
                        Debug.Log("prpturn, direction was " + prevdir + " tried to go " + direction);

                        prepTurn = direction;
                        direction = prevdir;
                    }
                    else if (inMotion && !accel)
                    {
                        stunned = true;
                        stunnedUI.SetActive(true);
                        stunnedStart = Time.fixedTime;
                    }
                    else if(inMotion && accel)
                    {
                        //check all left and right and turn automatically

                        int left = direction - 1;
                        if (left < 0) left = 3;
                        left = left % 4;

                        int right = (direction + 1) % 4;

                        int dirX = (int)DirectionToVector(left).x;
                        int dirZ = (int)DirectionToVector(left).z;

                        if(sg.tileInfo[xPos + dirX, zPos + dirZ] == 1 || sg.tileInfo[xPos + dirX, zPos + dirZ] == 3)
                        {
                            direction = left;  
                        }

                        dirX = (int)DirectionToVector(right).x;
                        dirZ = (int)DirectionToVector(right).z;

                        if (sg.tileInfo[xPos + dirX, zPos + dirZ] == 1 || sg.tileInfo[xPos + dirX, zPos + dirZ] == 3)
                        {
                            direction = right;
                        }
                    }
                    inMotion = false;
                }

            }
            if(prepTurn == -1)
                changeDirection();

        }
    }

    private bool IsThisInteger(float f)
    {
        float diff = Mathf.Abs(Mathf.RoundToInt(f) - f);
        return diff < 0.1;
    }

    private bool valid(int x, int z)
    {
        int dirX = (int)DirectionToVector(direction).x;
        int dirZ = (int)DirectionToVector(direction).z;

        return sg.tileInfo[x + dirX, z + dirZ] == 1 || sg.tileInfo[x + dirX, z + dirZ] == 3;
    }

    //for buttons
    public void decDir()
    {
        queueDecDir = true;
    }
    public void incDir()
    {
        queueIncDir = true;
    }

    void changeDirection()
    {
        camera.changeDirection(direction);
    }

    private Vector3 DirectionToVector(int direction)
    {
        Vector3 result;
        switch(direction%4)
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
