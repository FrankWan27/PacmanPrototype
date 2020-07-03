using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEnemy : MonoBehaviour
{
    private GameObject gm;
    private PlayerStatus ps;
    private PointArrows pa;
    private float spawnTime;
    // Use this for initialization

    private void Start()
    {
        spawnTime = Time.fixedTime;
        gm = GameObject.Find("GameManager");
        ps = gm.GetComponent(typeof(PlayerStatus)) as PlayerStatus;

        gm = GameObject.Find("ArrowIndicator");
        pa = gm.GetComponent<PointArrows>();
    }

    private void Update()
    {
        if(Time.fixedTime >= spawnTime + 10)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            gameObject.SetActive(false);
            pa.removeEnemy(other.gameObject);
            other.gameObject.SetActive(false);

        }
        if (other.tag == "enemy3")
        {
            EnemyMovement3 mv = other.GetComponent<EnemyMovement3>();
            if (mv.charged)
            {
                ps.spawnEnergy(mv.xPos, mv.zPos, other.transform.position + new Vector3(0, 0.25f, 0));
            }
            gameObject.SetActive(false);
            pa.removeEnemy(other.gameObject);
            other.gameObject.SetActive(false);
        }

    }
}
