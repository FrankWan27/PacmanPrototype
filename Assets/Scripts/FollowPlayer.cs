using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;
    private float vDist = 5 * Mathf.Sqrt(2);
    private float hDist = 5;
    private Vector3 targetOffset;

    // LateUpdate is called once per frame

    private void Start()
    {
        targetOffset = new Vector3(0, vDist, -hDist);

    }
    void LateUpdate ()
    {
        offset = Vector3.RotateTowards(offset, targetOffset, 0.05f, 10000);

        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }

    public void changeDirection(int direction)
    {
        switch (direction % 4)
        {
            case 0:
                targetOffset = new Vector3(0, vDist, -hDist);
                break;
            case 1:
                targetOffset = new Vector3(-hDist, vDist, 0);
                break;
            case 2:
                targetOffset = new Vector3(0 , vDist, hDist);
                break;
            case 3:
                targetOffset = new Vector3(hDist, vDist, 0);
                break;
            default:
                offset = new Vector3(0, 0, 0);
                break;
        }
    }
}
