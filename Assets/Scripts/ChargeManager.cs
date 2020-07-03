using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeManager : MonoBehaviour {
    private GameObject gm;
    private PlayerStatus ps;
    public RectTransform rt;
    public Animator anim;


    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager");
        ps = gm.GetComponent(typeof(PlayerStatus)) as PlayerStatus;
    }
	
	// Update is called once per frame
	void Update () {

        float height = ps.charge;
        if (height >= 700)
        {
            height = 700;
            anim.SetTrigger("Win");
        }
        if (height < 0)
        {

            anim.SetTrigger("GameOver");
            ps.gameOver();

        }

        rt.sizeDelta = new Vector2(30, height);
    }
}
