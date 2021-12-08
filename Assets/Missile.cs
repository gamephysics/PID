using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//====================================================================
// Class: Arrive 
// Desc : set position dest gameObjects & Order Unit Move to destination
//====================================================================
public class Missile : MonoBehaviour
{
    // UNIT
    public Target targetObj = null;

    //======================================
    // Destination GameObject Initial Position
    //======================================
    private void Awake()
    {
        var obj = Resources.Load<GameObject>("Prefabs/Center");

        var missleObj = GameObject.Instantiate(obj);
        missleObj.transform.parent = this.transform;
        missleObj.transform.position = Vector3.zero;

        _prevTime = Time.timeSinceLevelLoad;
    }

    Vector3      _previous_error = Vector3.zero;


    public float m_kP       = 0.3f;
    public float m_kI       = 0.0001f;
    public float m_kD       = 0.3f;
    public float m_MaxSpeed = 0.5f;

    [ReadOnly]
    public Vector3 m_intE   = Vector3.zero;
    [ReadOnly]
    public Vector3 m_dE     = Vector3.zero;
    [ReadOnly]
    public Vector3 m_MV     = Vector3.zero;

    private float _prevTime = 0;
    
    void Update()
    {
        //======================================
        // MOVE TARGET POSITION
        //======================================
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                m_intE = Vector3.zero;
                targetObj.center = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }

        //======================================
        // DELTA TIME
        //======================================
        var deltaTime = Time.timeSinceLevelLoad - _prevTime;
        if (deltaTime < 0.01f)
            return;
        
        _prevTime = Time.timeSinceLevelLoad;

        //======================================
        // FOLLOW TARGET
        //======================================
        if (targetObj != null)
        {
            var _missile_curpos = this.transform.position;
            var _target_curpos  = targetObj.transform.position;

            var _current_error = (_target_curpos - _missile_curpos);

            // COLLIDED ÀÎÁ¤
            if(_current_error.magnitude <= 1.5f)
            {
                // INIT
                this.transform.position = _target_curpos;
                m_intE   = Vector3.zero;
                m_dE     = Vector3.zero;
                m_MV     = Vector3.zero;
            }
            else
            {
                //======================================
                // PID
                //======================================
                m_intE += _current_error * deltaTime;
                m_dE = (_current_error - _previous_error) / deltaTime;
                m_MV = m_kP * (_current_error) + m_kI * (m_intE) + m_kD * (m_dE);
                _previous_error = _current_error;

                var _Force = Vector3.ClampMagnitude(m_MV, m_MaxSpeed);
                this.transform.position += _Force;
            }
        }

    }
}
