using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Diagnostics;
using System.IO;

public class FileMovement1 : MonoBehaviour
{
    //Public variables
    public TextAsset trajectoryFile;
    public float scale = 0.001f;
    public bool loop;
    public Color dotMainCol = Color.white;
    public int pointResolution = 10;

    //Private variables
    private bool _isActive;
    [SerializeField] float _duration;
    private float _tInit;
    private int _stepMax;
    private Vector3 _initPos;
    private Vector3 _reference;
    [SerializeField] GameObject _billBoard;
    [SerializeField] float _pointScale = 10.0f;

    private float _Bras6Len;


    private File _trajectory;
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
            _trajectory = new File(trajectoryFile);
            _trajectory.waitEndReading();

            _duration = _trajectory.Duration * 60;
            _stepMax = _trajectory.Points.Length;
            _isActive = true;
            _reference = _trajectory.Points[0]._coords;
            instantiatePoints();
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
            Vector3 nextCoord = scaledCoords(_trajectory.Points[0]);
            Vector3 previousCoord = scaledCoords(_trajectory.Points[_stepMax]);
            transform.position = Vector3.Lerp(previousCoord, nextCoord, timeStep - step);
        }
        else
        {
            Vector3 nextCoord = scaledCoords(_trajectory.Points[step + 1]);
            Vector3 previousCoord = scaledCoords(_trajectory.Points[step]);
            transform.position = Vector3.Lerp(previousCoord, nextCoord, timeStep - step);
        }
    }

    public void instantiatePoints()
    {
        float H, S, V;

        Color.RGBToHSV(dotMainCol, out H, out S, out V);

        GameObject pointsHolder = new GameObject();
        pointsHolder.name = "CoordinatesPoints";

        GameObject[] levels = new GameObject[_trajectory.maxLevel];
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject iLevel = new GameObject();
            iLevel.name = "Soudure niveau " + (i + 1).ToString();
            iLevel.transform.SetParent(pointsHolder.transform);
            levels[i] = iLevel;
        }
        int filter = 0;
        foreach (Point point in _trajectory.Points)
        {
            if (filter % pointResolution == 0)
            {

                GameObject pointObject = UnityEngine.Object.Instantiate(_billBoard);
                pointObject.name = "PointID_" + point._lineID;
                pointObject.transform.position = scaledCoords(point);
                pointObject.transform.localScale = new Vector3(_pointScale, _pointScale, _pointScale);
                pointObject.transform.SetParent(levels[point._level - 1].transform);

                //Changin material color
                float newSaturation = 1 - ((float)point._level - 1) / ((float)levels.Length - 1);
                Color newCol = Color.HSVToRGB(H, newSaturation, 1.0f, true);

                pointObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetColor("_MainCol", newCol);
            }
            filter++;
        }
    }

    public void toggleActive()
    {
        _isActive = false;
    }

    private Vector3 scaledCoords(Point point)
    {
        return _initPos + (_reference - point._coords) * scale;
    }
}
