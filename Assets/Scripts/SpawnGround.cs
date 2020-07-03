using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGround : MonoBehaviour {

    public int sizeX = 50;
    public int sizeZ = 50;
    public Tile tilePrefab;
    public Tile[,] tiles;
    public int[,] tileInfo;

    public int getSpawnX()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            return 1;
        }
        else
            return 48;
        
    }

    private void Awake()
    {
        Generate();
    }
    // Use this for initialization
    public void Generate () {
        tiles = new Tile[sizeX, sizeZ];
        tileInfo = new int[sizeX, sizeZ];
		for(int i = 0; i < sizeX; i++)
        {
            for(int j = 0; j < sizeZ; j++)
            {
                CreateTile(i, j);
            }
        }


        //make neighbor of walls unwalkable
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                if(tileInfo[i, j] == 0)
                {
                    int[] dirX = { -1, -1, -1, 0, 0, 1, 1, 1 };
                    int[] dirZ = { -1, 0, 1, -1, 1, -1, 0, 1 };
                        
                    for(int d = 0; d < 8; d++)
                    {
                        int newX = i + dirX[d];
                        int newZ = j + dirZ[d];

                        if(newX >= 0 && newX < sizeX)
                        {
                            if(newZ >=0 && newZ < sizeZ)
                            {
                                if(tileInfo[newX, newZ] == 1)
                                {
                                    tileInfo[newX, newZ] = 2;
                                }
                            }
                        }
                    }
                }
            }
        }

        //portals
        tileInfo[0, sizeZ / 2] = 2;
        tileInfo[sizeX - 1, sizeZ / 2] = 2;

        Color greenish = new Color(75f/255f, 225f/255f, 48f/255f);

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 1; j < sizeZ; j++)
            {
                if (tileInfo[i, j] == 2)
                {
                    Renderer r = tiles[i, j].GetComponent<Renderer>();
                    r.material.color = greenish;
                }
            }
        }



        //make a safe zone
        int safeSizeX = 5;
        int safeSizeZ = 5;
        for (int i = -safeSizeX + 1; i < safeSizeX; i ++)
        {
            for (int j = -safeSizeZ; j < safeSizeZ + 1; j++)
            {
                tileInfo[sizeX / 2 + i, sizeZ / 2 + j] = 3;
            }
        }

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 1; j < sizeZ; j++)
            {
                if (tileInfo[i, j] == 3)
                {
                    Renderer r = tiles[i, j].GetComponent<Renderer>();
                    //r.material.color = Color.blue;
                }
            }
        }
    }

    private void CreateTile(int x, int z)
    {
        Vector3 tilePos = new Vector3(x - sizeX / 2, 0f, z - sizeZ / 2);
        Tile newTile = Instantiate(tilePrefab) as Tile;
        tiles[x, z] = newTile;
        newTile.name = "Tile " + x + ", " + z;
        newTile.transform.parent = transform;
        newTile.transform.localPosition = tilePos;
        tileInfo[x, z] = 1;

        bool underWall = Physics.Linecast(tilePos, tilePos + Vector3.up);

        if (underWall)
        {
            Renderer r = newTile.GetComponent<Renderer>();
            r.material.color = Color.gray;
            tileInfo[x, z] = 0;
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
