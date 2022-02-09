using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Diagnostics;

public class FileMovement : MonoBehaviour
{
    //Public variables
    public TextAsset trajectoryFile;
    public float scale = 0.001f;
    public bool loop;

    //Private variables
    private float _duration;
    private float _tInit;
    private int _stepMax;
    private Vector3 _initPos;
    private bool _isActive;

    private File trajectory;
    // Start is called before the first frame update
    void Start()
    {
        _duration = 0.0f;
        _tInit = Time.time;
        _stepMax = 0;
        _initPos = transform.position;
        _isActive = false;

        if (trajectoryFile != null)
        {
            trajectory = new File(trajectoryFile);
            trajectory.waitEndReading();

            _duration = trajectory.Duration * 60;
            _stepMax = trajectory.Points.Length;
            _isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive) return;
        moveKuka();
    }

    private void moveKuka()
    {
        float elapsedTime = Time.time - _tInit;
        if (elapsedTime > _duration)
        {
            if (loop)
            {
                _tInit = Time.time;
                elapsedTime = 0.0f;
            }
            else
            {
                toggleActive();
            }
        }

        float timeStep = _stepMax * elapsedTime / _duration;
        int step = Mathf.FloorToInt(timeStep);
        if (step == _stepMax)
        {
            //transform.position = Vector3.Lerp();
        } else
        {
            //transform.position = Vector3.Lerp();
        }


    }

    public void toggleActive()
    {
        _isActive = false;
    }
}
