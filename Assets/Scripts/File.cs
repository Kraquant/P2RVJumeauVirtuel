using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

public class File
{
    #region Private attributes
    private List<Point> _pointList;
    private int _duration;
    private string _name;
    private string _fileText;
    private Thread threadReading;
    private int[] _readingStatus;
    private int _maxLevel;
    #endregion

    #region Public attributes
    public int Duration
    {
        get {
            if (threadReading.IsAlive)
            {
                Debug.LogError("Error: You tried to access Duration but the value is currently edited on a separate thread.");
                return 0;
            } else
            {
                return _duration;
            }
        }
    }
    public string Name
    {
        get
        {
            if (threadReading.IsAlive)
            {
                Debug.LogError("Error: You tried to access Name but the value is currently edited on a separate thread.");
                return null;
            }
            else
            {
                return _name;
            }
        }
    }
    
    /// <summary>
    /// Returns an array with the points contained in the file
    /// </summary>
    public Point[] Points {
        get
        {
            if (threadReading.IsAlive)
            {
                Debug.LogError("Error: You tried to access Points but the value is currently edited on a separate thread.");
                return null;
            }
            else
            {
                return _pointList.ToArray();
            }
        }
    }
    
    /// <summary>
    /// Get reading status of the file. Reading is done on a separate thread.
    /// </summary>
    public bool IsReading
    {
        get
        {
            return threadReading.IsAlive;
        }
    }
    
    /// <summary>
    /// Return the reading status of the file [current line, number of lines]
    /// </summary>
    public int[] readingStatus
    {
        get
        {
            return _readingStatus;
        }
    }
    public int maxLevel {
        get
        {
            if (threadReading.IsAlive)
            {
                Debug.LogError("Error: You tried to access maxLevel but the value is currently edited on a separate thread.");
                return 0;
            }
            else
            {
                return _maxLevel;
            }
        }
    }
    #endregion
    
    #region Constructors
    public File()
    {
        _pointList = new List<Point>();
        _name = "Undefined name";
        _duration = 0;
        _maxLevel = 0;
        _readingStatus = new int[2] { 0, 0 };
    }

    public File(TextAsset file)
    {
        _pointList = new List<Point>();
        _name = "Undefined name";
        _duration = 0;
        _maxLevel = 0;
        _readingStatus = new int[2] { 0, 0 };

        _fileText = file.text;

        threadReading = new Thread(readFile);
        threadReading.Start();
    }

    public File(string path)
    {
        _pointList = new List<Point>();
        _name = "Undefined name";
        _duration = 0;
        _maxLevel = 0;
        _readingStatus = new int[2] { 0, 0 };

        _fileText = System.IO.File.ReadAllText(path);

        threadReading = new Thread(readFile);
        threadReading.Start();
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Shows the different properties of the file after reading
    /// </summary>
    public void printStats()
    {
        Debug.Log("File object properties:\nName: " + _name + "\nDuration: " + _duration + " minutes\nNumberOfPoints: " + _pointList.Count);
    }

    /// <summary>
    /// Stop the code until the current file is done reading
    /// </summary>
    public void waitEndReading()
    {
        if (threadReading.IsAlive)
        {
            threadReading.Join();
        }
    }

    /// <summary>
    /// Change the data file stored in the current object
    /// </summary>
    /// <param name="file">File asset</param>
    public void readNewFile(TextAsset file)
    {
        _pointList = new List<Point>();
        _name = "Undefined name";
        _duration = 0;
        _maxLevel = 0;
        _fileText = file.text;
        _readingStatus = new int[2] { 0, 0 };

        threadReading = new Thread(readFile);
        threadReading.Start();
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Method used to read the trajectory file
    /// </summary>
    private void readFile()
    {
        Debug.Log("Reading File...");

        string[] fileLines =Regex.Split(_fileText, "\\n");
        _readingStatus[1] = fileLines.Length;
        

        //Regex patterns #################################################################################
        string patternX = "(?<=X)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.))";
        string patternY = "(?<=Y)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.))";
        string patternZ = "(?<=Z)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.))";

        string patternA3 = "(?<=A3=)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.))";
        string patternB3 = "(?<=B3=)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.))";
        string patternC3 = "(?<=C3=)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.))";

        string patternE1 = "(?<=E1=)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.))";
        string patternE2 = "(?<=E2=)[\\d\\-]?\\w+[\\.]?(\\w+[\\.]?(?=\\s)|(?=\\.)|$)"; //Different pattern because E2 can be at the end of the line

        string patternN = "(?<=N)[\\-]?\\w+[\\.]?\\w+(?=\\s)";
        string patternArc = "(?<=H45=)\\w+((?=\\s)|$)";
        string patternF = "(?<=F)[\\-]?\\w+[\\.]?\\w+(?=\\.)";
        string patternLevel = "(?<=Niv)\\w+(?=:)";
        string patternTime = "(?<=Temps programme : )\\w+(?= min)";
        string patternName = "(?<=Program file name = )[\\w| |.]+";

        float FState = 0.0f;
        bool arcState = false;
        int levelState = 0;

        bool foundName = false;
        bool foundTime = false;

        foreach (string line in fileLines)
        {
            _readingStatus[0] += 1;
            //Searching patterns in the Line
            Match xLine = Regex.Match(line, patternX);
            Match yLine = Regex.Match(line, patternY);
            Match zLine = Regex.Match(line, patternZ);

            Match A3Line = Regex.Match(line, patternA3);
            Match B3Line = Regex.Match(line, patternB3);
            Match C3Line = Regex.Match(line, patternC3);

            Match NLine = Regex.Match(line, patternN);

            Match arcLine = Regex.Match(line, patternArc);
            Match FLine = Regex.Match(line, patternF);

            Match E1Line = Regex.Match(line, patternE1);
            Match E2Line = Regex.Match(line, patternE2);

            
            Match levelLine = Regex.Match(line, patternLevel);

            if (!foundName)
            {
                Match nameLine = Regex.Match(line, patternName);
                if (nameLine.Success)
                {
                    _name = nameLine.Value;
                    foundName = true;
                }
            }
            if (!foundTime)
            {
                Match timeLine = Regex.Match(line, patternTime);
                if (timeLine.Success)
                {
                    int timeINT;
                    int.TryParse(timeLine.Value, out timeINT);
                    _duration = timeINT;
                    foundTime = true;
                }
            }
            
            if (levelLine.Success)
            {
                int.TryParse(levelLine.Value, out levelState);
                if (levelState > _maxLevel) _maxLevel = levelState;
            }
            if (arcLine.Success)
            {
                if (arcLine.Value.Contains("ON"))
                {
                    arcState = true;
                }
                else if (arcLine.Value.Contains("OFF"))
                {
                    arcState = false;
                } else 
                {
                    Debug.LogWarning("Error while reading Arc status. Line:\n" + line);
                }
            }
            if (FLine.Success)
            {
                float.TryParse(FLine.Value, out FState);
            }

            bool isPoint = xLine.Success && yLine.Success && zLine.Success && A3Line.Success && B3Line.Success && C3Line.Success && E1Line.Success && E2Line.Success;
            bool couldBePoint = xLine.Success || yLine.Success || zLine.Success || A3Line.Success || B3Line.Success || C3Line.Success || E1Line.Success || E2Line.Success;

            if (couldBePoint && !isPoint)
            {
                if (NLine.Success)
                {
                    Debug.Log(xLine.Success);
                    Debug.Log(yLine.Success);
                    Debug.Log(zLine.Success);
                    Debug.Log(A3Line.Success);
                    Debug.Log(B3Line.Success);
                    Debug.Log(C3Line.Success);
                    Debug.Log(E1Line.Success);
                    Debug.Log(E2Line.Success);
                    Debug.LogWarning("A point might not have been added. REF N:" + NLine.Value + " at line:\n" + line);
                }
                else
                {
                    Debug.LogWarning("A point might not have been added. Line:\n" + line);
                }

            }

            if (isPoint)
            {

                //Getting float values *************************************************************************
                //Converting string to correct type
                float xF, yF, zF;
                float aF, bF, cF;
                float e1F, e2F;

                //Replacing dots with comm
                //a

                string xS = xLine.Value.Replace('.', ',');
                string yS = yLine.Value.Replace('.', ',');
                string zS = zLine.Value.Replace('.', ',');

                string aS = A3Line.Value.Replace('.', ',');
                string bS = B3Line.Value.Replace('.', ',');
                string cS = C3Line.Value.Replace('.', ',');

                string e1S = E1Line.Value.Replace('.', ',');
                string e2S = E2Line.Value.Replace('.', ',');

                //Parsing

                float.TryParse(xS, out xF);
                float.TryParse(yS, out yF);
                float.TryParse(zS, out zF);

                float.TryParse(aS, out aF);
                float.TryParse(bS, out bF);
                float.TryParse(cS, out cF);

                float.TryParse(e1S, out e1F);
                float.TryParse(e2S, out e2F);

                int nInt;
                int.TryParse(NLine.Value, out nInt);

                
                Vector3 coords = new Vector3(xF, -zF, yF);
                Vector3 normal = new Vector3(aF, -cF, bF);
                Vector2 positionneur = new Vector2(e1F, e2F);
                _pointList.Add(new Point(nInt, levelState, arcState, coords, normal, positionneur, FState));


            }

        }
        Debug.Log("Finished reading file !");
        printStats();

        _readingStatus[0] = 0;
        _readingStatus[1] = 0;
    }
    #endregion
}
