using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Diagnostics;
using System.IO;

public class FileMovement : MonoBehaviour
{

    // test merge// 
    //Public variables ##########################################
    [Range(0, 4.0f)] public float speed = 1.0f;
    public float scale = 0.001f;
    public bool loop = true;
    public Color dotMainCol = Color.white;
    public int pointResolution = 10; //Draw one point every pointResolution
    public File trajectory { get; private set; }
    public int[] playingProgress { get { return _playingProgress; } }

    //Private variables #########################################
    private bool _isActive;
    [SerializeField] float _duration;
    private float _tInit;
    private int _stepMax;
    private Vector3 _initPos;
    private Vector3 _reference;
    private bool _loadingFile;

    //private float _Bras6Len;
    private float _TorcheLen;
    [SerializeField] GameObject Target2;
    [SerializeField] GameObject OsBras5;
    //[SerializeField] GameObject OsBras6;
    [SerializeField] GameObject Torche;

    //Variables for point drawing
    [SerializeField] GameObject _billBoard;
    [SerializeField] float _pointScale = 10.0f;
    private GameObject _pointsHolder;

    private int[] _playingProgress;

    private int _targetStep;

    void Start()
    {
        //Setup intern variables
        _initPos = transform.position;
        _playingProgress = new int[2] { 0, 0 };
        _TorcheLen = (Torche.transform.position - OsBras5.transform.position).magnitude;
        stopPlaying(); //Set/Reset every component      
    }

    // Update is called once per frame
    void Update()
    {

        // Checking Reading Status
        if (_loadingFile)
        {
            if(!trajectory.IsReading)
            {
                _loadingFile = false;
                OnFileDoneReading();
            }
        }
        else if (_isActive)
        {
            moveKukaWithSpeed();
        }
        else if (trajectory == null && Vector3.Distance(this.transform.position, _initPos)  > Vector3.kEpsilon) //Returning to origin
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _initPos, speed * Time.deltaTime);
        }
    }


    private void moveKukaWithSpeed()
    {
        bool distanceReached = false;
        float remainingDistance = speed * Time.deltaTime;
        Vector3 currentPos = this.transform.position;
        Vector3 _targetCoord = scaledCoords(trajectory.Points[_targetStep]);

        bool endReading = false;
        while (!distanceReached)
        {
            float distanceToNextTarget = Vector3.Distance(currentPos, _targetCoord);
            if (remainingDistance > distanceToNextTarget)
            {
                remainingDistance -= distanceToNextTarget;
                currentPos = _targetCoord;
                _targetStep++;
                if(_targetStep >= _stepMax) {
                    _targetStep = 0;
                    if(!loop)
                    {
                        distanceReached = true;
                        endReading = true;
                    }
                }
                _playingProgress[0] = _targetStep;
                _targetCoord = scaledCoords(trajectory.Points[_targetStep]);
            }

            else
            {
                distanceReached = true;
            }
        }

        this.transform.position = Vector3.MoveTowards(currentPos, _targetCoord, remainingDistance);
        Target2.transform.position = this.transform.position - (trajectory.Points[_targetStep]._normal).normalized * _TorcheLen;

        if (endReading) stopPlaying();
    }

    private void moveKukaWithSpeedSave()
    {
        bool distanceReached = false;
        float remainingDistance = speed * Time.deltaTime;
        Vector3 currentPos = this.transform.position;
        Vector3 _targetCoord = scaledCoords(trajectory.Points[_targetStep]);
        while (!distanceReached)
        {
            float distanceToNextTarget = Vector3.Distance(currentPos, _targetCoord);
            if (remainingDistance > distanceToNextTarget)
            {
                remainingDistance -= distanceToNextTarget;
                currentPos = _targetCoord;
                _targetStep++;
                if (_targetStep >= _stepMax)
                {
                    _targetStep = 0;
                }
                _playingProgress[0] = _targetStep;
                _targetCoord = scaledCoords(trajectory.Points[_targetStep]);
            }

            else
            {
                distanceReached = true;
            }
        }

        this.transform.position = Vector3.MoveTowards(currentPos, _targetCoord, remainingDistance);
        Target2.transform.position = this.transform.position - (trajectory.Points[_targetStep]._normal).normalized * _TorcheLen;

    }

    private Vector3 scaledCoords(Point point)
    {
        return _initPos + (_reference - point._coords) * scale;
    }

    private void clearPoints()
    {
        Destroy(_pointsHolder);
        _pointsHolder = new GameObject();
        _pointsHolder.name = "CoordinatesPoints";
    }
    public void instantiatePoints()
    {
        //Before anything, we need to clear the previous points...
        clearPoints();
        //Then we create new ones
        float H, S, V;
        Color.RGBToHSV(dotMainCol, out H, out S, out V);

        GameObject[] levels = new GameObject[trajectory.maxLevel];
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject iLevel = new GameObject();
            iLevel.name = "Soudure niveau " + (i+1).ToString();
            iLevel.transform.SetParent(_pointsHolder.transform);
            levels[i] = iLevel;
        }
        int filter = 0;
        foreach(Point point in trajectory.Points)
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

    public void togglePlaying()
    {
        _isActive = !_isActive;
    }

    public void stopPlaying()
    {
        trajectory = null;
        _tInit = 0.0f;
        _duration = 0.0f;
        _stepMax = 0;
        _isActive = false;
        _loadingFile = false;
        clearPoints();
    }

    public void loadNewFile(string path)
    {
        stopPlaying();
        if (path != null)
        {
            trajectory = new File(path);
            _loadingFile = true;
        }
    }

    private void OnFileDoneReading()
    {
        if (trajectory.IsReading)
        {
            UnityEngine.Debug.LogError("File Access Denied");
        }
        else
        {
            //Setting up new values for the variables.
            _duration = trajectory.Duration * 60;
            _stepMax = trajectory.Points.Length;
            playingProgress[1] = _stepMax;
            _isActive = true;
            _reference = trajectory.Points[0]._coords;
            _tInit = Time.deltaTime;
            _targetStep = 0;
            
            instantiatePoints();
        }
    }

    private float computeSpeed() //Very Slow please do not use
    {
        if (trajectory == null) { return 0.0f; }
        else
        {
            float totalDistance = 0.0f;
            for (int i = 0; i < trajectory.Points.Length -1 ; i++)
            {
                totalDistance += Vector3.Distance(trajectory.Points[i]._coords * scale, trajectory.Points[i+1]._coords * scale);
            }

            return totalDistance / (trajectory.Duration * 60);
        }
        
    }
}
