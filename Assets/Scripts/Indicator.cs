using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {
    private GameObject gm;
    private PlayerStatus ps;
    public RectTransform rt;
    public float offset = 10;

    // Use this for initialization
    void Start()
    {
        gm = GameObject.Find("GameManager");
        ps = gm.GetComponent(typeof(PlayerStatus)) as PlayerStatus;
    }


    // Update is called once per frame
    void Update () {
        float height = ps.charge + PlayerStatus.CHARGEAMOUNT * ps.packages + offset;
        if (height >= 700) height = 699;
        rt.localPosition = new Vector3(0, height, 0);
		
	}
}
