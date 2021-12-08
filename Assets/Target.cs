using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//====================================================================
// Class: Squads
// Desc : Spawn Units from (public int unitCount = 4;)
//====================================================================
public class Target : MonoBehaviour
{
    // CIRCLE CENTER POSITION
    public Vector3 center = Vector3.zero;

    void Awake()
    {
        var obj = Resources.Load<GameObject>("Prefabs/Unit");

        var unit = GameObject.Instantiate(obj);
        unit.transform.parent = this.transform;
        unit.transform.position = Vector3.zero;
    }


    //======================================
    // CIRCLE MOVE
    //======================================
    void Update()
    {
        var pi = (int)(Time.timeSinceLevelLoad * 100) % 360;
        var q = Quaternion.AngleAxis((float)pi, Vector3.up);
        var pos = center  + q * Vector3.right * 20;
        this.transform.position = pos;
    }

}
