using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class File
{
    class Point{
        public int _lineID { get; set; }
        public Vector3 _coords { get; set; }
        public Vector3 _normal { get; set; }
        public Vector2 _positionneur { get; set; }
        public int _level { get; set; }
        public bool _arc { get; set; }
        public float _speed { get; set; }


        public Point()
        {
            _coords = new Vector3();
            _normal = new Vector3();
            _positionneur = new Vector2();
            _level = 0;
            _arc = false;
            _lineID = 0;
        }
        public Point(int lineID, int level, bool arc, Vector3 coords, Vector3 normal, Vector2 positionneur, float speed)
        {
            _lineID = lineID;
            _level = 0;
            _arc = false;
            _coords = coords;
            _normal = normal;
            _positionneur = positionneur;
            _speed = speed;
              
        }
    }

    private List<Point> pointList;
    public int _duration { get; private set; }
    public string _name { get; private set; }

    public File()
    {
        pointList = new List<Point>();
        _duration = 0;
    }

    public File(TextAsset file)
    {
    }

    private void readFile(TextAsset file)
    {
        Vector3 reference = new Vector3(0.0f, 0.0f, 0.0f);

        string fileText = file.text;
        string[] fileLines = Regex.Split(fileText, "\\n|\\r|\\r\\n");

        string patternX = "(?<=X=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";
        string patternY = "(?<=Y=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";
        string patternZ = "(?<=Z=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";

        string patternA3 = "(?<=A3=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";
        string patternB3 = "(?<=B3=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";
        string patternC3 = "(?<=C3=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";

        string patternE1 = "(?<=E1=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";
        string patternE2 = "(?<=E2=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";

        string patternN = "(?<=N=)[\\-]?\\d+[\\.]?\\d+(?=\\s)";
        string patternArc = "(?<=H45=)\\w+(?=\n)";
        string patternF = "(?<=F)[\\-]?\\d+[\\.]?\\d+(?=\\.)";
        string patternLevel = "(?<= Niv)\\d + (?=:)";
        string patternTime = "(?<=Temps programme : )\\w+(?= min)";
        string patternName = "(?<=Program file name = )[\\w| |.]+(?=\n)";

        float FState = 0.0f;
        bool arcState = false;
        int levelState = 0;

        foreach (string line in fileLines)
        {

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

            Match timeLine = Regex.Match(line, patternTime);
            Match levelLine = Regex.Match(line, patternLevel);
            Match nameLine = Regex.Match(line, patternName);

            bool isPoint = xLine.Success && yLine.Success && zLine.Success && A3Line.Success && B3Line.Success && C3Line.Success && E1Line.Success && E2Line.Success;
            bool couldBePoint = xLine.Success || yLine.Success || zLine.Success || A3Line.Success || B3Line.Success || C3Line.Success || E1Line.Success || E2Line.Success;
            if (couldBePoint)
            {
                if (NLine.Success)
                {
                    Debug.LogWarning("A point might not have been added. REF N:" + NLine.Value);
                }
                else
                {
                    Debug.LogWarning("A point might not have been added. REF not set");
                }
             
            }



            if (nameLine.Success)
            {
                _name = nameLine.Value;
            } 
            else if (timeLine.Success)
            {
                int timeINT;
                int.TryParse(timeLine.Value, out timeINT);
                _duration = timeINT;
            }
            else if (levelLine.Success)
            {
                int.TryParse(levelLine.Value, out levelState);
            }
            else if (arcLine.Success)
            {
                switch (arcLine.Value)
                {
                    case "ON":
                        arcState = true;
                        break;
                    case "OFF":
                        arcState = false;
                        break;
                    default:
                        arcState = false;
                        break;
                }
            }
            else if (FLine.Success)
            {
                float.TryParse(FLine.Value, out FState);
            }


            if (isPoint)
            {

                //Getting float values *************************************************************************
                //Converting string to correct type
                float xF, yF, zF;
                float aF, bF, cF;
                float e1F, e2F;

                //Replacing dots with comma

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

                
                Vector3 coords = new Vector3(xF, yF, zF);
                Vector3 normal = new Vector3(aF, bF, cF);
                Vector2 positionneur = new Vector2(e1F, e2F);
                pointList.Add(new Point(nInt, levelState, arcState, coords, normal, positionneur, FState));


            }

        }
    }
}
