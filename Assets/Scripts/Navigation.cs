using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Navigation : MonoBehaviour {
    private GameObject go;
    private SpawnGround sg;
    private int endX;
    private int endZ;

    private void Start()
    {
        go = GameObject.Find("SpawnGround");
        sg = go.GetComponent(typeof(SpawnGround)) as SpawnGround;
    }

    public int[,] shortestPath(int startX, int startZ, int endXX, int endZZ)
    {
        endX = endXX;
        endZ = endZZ;
        int[,] dist = new int[sg.sizeX, sg.sizeZ];
        int[,] prev = new int[sg.sizeX, sg.sizeZ];
        int[] dirX = { 0, 1, 0, -1 };
        int[] dirZ = { 1, 0, -1, 0 };

        // int = currZ * sg.sizeX + currX
        SimplePriorityQueue<int> pq = new SimplePriorityQueue<int>();   

        for(int i = 0; i < sg.sizeX; i++)
        {
            for(int j = 0; j < sg.sizeZ; j++)
            {
                dist[i, j] = int.MaxValue;
                prev[i, j] = -1;
            }
        }


        pq.Enqueue(startZ * sg.sizeX + startX, calcHeuristic(0, startX, startZ));

        dist[startX, startZ] = 0;
        while (pq.Count != 0)
        {
            int curr = pq.Dequeue();
            int currX = curr % sg.sizeX;
            int currZ = curr / sg.sizeX;
            //check neighbors
            for (int i = 0; i < 4; i++)
            {
                int nextX = currX + dirX[i];
                int nextZ = currZ + dirZ[i];
                if (sg.tileInfo[nextX, nextZ] == 1)
                {
                    int tempDist = dist[currX, currZ] + 1;
                    if(tempDist < dist[nextX, nextZ])
                    {
                        dist[nextX, nextZ] = tempDist;
                        prev[nextX, nextZ] = currZ * sg.sizeX + currX;
                        pq.Enqueue(nextZ * sg.sizeX + nextX, calcHeuristic(tempDist, nextX, nextZ));
                    }
                }
            }
        }

         
        //for (int i = 0; i < sg.sizeX; i++)
        //{
        //    for (int j = 0; j < sg.sizeZ; j++)
        //    {
        //        if(sg.tileInfo[i, j] == 1)
        //        {
        //            Renderer r = sg.tiles[i, j].GetComponent<Renderer>();
        //            r.material.color = new Color(0.4502378f, 1f, 0.3066038f);
        //        }
        //    }
        //}


        while (prev[endXX, endZZ] != -1)
        {
            //Renderer r = sg.tiles[endXX, endZZ].GetComponent<Renderer>();
            //r.material.color = Color.cyan;

            int prevIndex = prev[endXX, endZZ];
            endXX = prevIndex % sg.sizeX;
            endZZ = prevIndex / sg.sizeX;
        }

        return prev;

    }

    public float calcHeuristic(int dist, int startX, int startZ)
    {
        //calculate geodesic distance
        int distX = Mathf.Abs(startX - endX);
        int distZ = Mathf.Abs(startZ - endZ);
        float diag = Mathf.Sqrt(distX * distX + distZ * distZ);
        return diag + dist;
        

    }
}
