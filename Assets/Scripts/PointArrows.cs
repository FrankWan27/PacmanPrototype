using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointArrows : MonoBehaviour
{


    private GameObject go;
    private Movement player;
    public Sprite arrow;
    public GameObject arrowPrefab;
    private GameObject arrowParent;

    public List<GameObject> enemies;
    public List<GameObject> arrows;
    // Use this for initialization
    void Start()
    {
        go = GameObject.Find("Player");
        player = go.GetComponent<Movement>();

        arrowParent = GameObject.Find("ArrowIndicator");

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 vector = enemies[i].transform.position - go.transform.position;

            float dist = vector.magnitude;
            

            float dot = Vector3.forward.x * vector.x + Vector3.forward.z * vector.z;
            float det = Vector3.forward.x * vector.z + Vector3.forward.z * vector.x;

            float angle = -Mathf.Atan2(det, dot) * Mathf.Rad2Deg;

            angle += 90f * player.direction;
            angle = angle % 360;

            Debug.Log(vector + " and " + angle);
            //arrows[i].transform.localPosition = new Vector3(vector.normalized.x, vector.normalized.z, 0) * 200;
            arrows[i].transform.eulerAngles = new Vector3(0, 0, angle);
            //arrows[i].transform.localScale = new Vector3(10 / dist, 10 / dist, 1);
        }

    }

    public void addEnemy(GameObject enemy)
    {
        enemies.Add(enemy);

        GameObject instance = (GameObject)Instantiate(arrowPrefab, new Vector3(0, 0, 0), transform.rotation);
        instance.transform.SetParent(arrowParent.transform, false);

        arrows.Add(instance);
    }

    public void removeEnemy(GameObject enemy)
    {
        int i = enemies.IndexOf(enemy);
        enemies.RemoveAt(i);
        arrows[i].SetActive(false);
        arrows.RemoveAt(i);
    }
}
