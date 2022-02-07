using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class FileInteraction : MonoBehaviour
{
    public TextAsset trajectoireFile;
    public bool loop = true;
    public float duration = 5.0f;
    public float scale = 1.0f;
    public bool showBoundary;
    public Material workingZoneMaterial;

    private List<Vector3> trajectoireFileCoord;
    private List<Vector3> trajectoireCoord;
    private Vector3 initPos;
    private float tInit;
    private int stepMax;
    private bool isActive = true;

    private void Start()
    {
        trajectoireFileCoord = new List<Vector3>();
        trajectoireCoord = new List<Vector3>();
        initPos = transform.position;
        readFile();
        calculateWorkingZone();
        //drawWorkingZone();
        tInit = Time.time;
        stepMax = trajectoireCoord.Count;
    }

    private void Update()
    {
        float elapsedTime = Time.time - tInit;
        if (elapsedTime > duration)
        {
            if (loop)
            {
                tInit = Time.time;
                elapsedTime = 0.0f;
            }
            else
            {
                isActive = false;
            }
        }
        if (isActive)
        {
            float timeStep = stepMax * elapsedTime / duration;
            int step = Mathf.RoundToInt(timeStep);

            if (step == stepMax)
            {
                transform.position = Vector3.Lerp(trajectoireCoord[stepMax], trajectoireCoord[0], timeStep - step);

            }
            else
            {
                transform.position = Vector3.Lerp(trajectoireCoord[step], trajectoireCoord[step + 1], timeStep - step);
            }



        }

    }

    private void readFile()
    {
        Vector3 reference = new Vector3(0.0f, 0.0f, 0.0f);

        string fileText = trajectoireFile.text;
        string[] fileLines = Regex.Split(fileText, "\\n|\\r|\\r\\n");

        string patternX = "(?<=X)[\\d\\D]?\\w+[\\.]?\\w+(?=\\s)";
        string patternY = "(?<=Y)[\\d\\D]?\\w+[\\.]?\\w+(?=\\s)";
        string patternZ = "(?<=Z)[\\d\\D]?\\w+[\\.]?\\w+(?=\\s)";

        foreach (string line in fileLines)
        {

            Match xLine = Regex.Match(line, patternX);
            Match yLine = Regex.Match(line, patternY);
            Match zLine = Regex.Match(line, patternZ);
            if (xLine.Success && yLine.Success && zLine.Success)
            {
                float xF, yF, zF;
                string xS = xLine.Value.Replace('.', ',');
                string yS = yLine.Value.Replace('.', ',');
                string zS = zLine.Value.Replace('.', ',');

                float.TryParse(xS, out xF);
                float.TryParse(yS, out yF);
                float.TryParse(zS, out zF);
                trajectoireFileCoord.Add(new Vector3(xF, yF, zF));
            }

        }
    }

    private void calculateWorkingZone()
    {
        Vector3 reference = trajectoireFileCoord[0];
        foreach (Vector3 coord in trajectoireFileCoord)
        {
            Vector3 Coord = initPos + (reference - coord) * scale;
            trajectoireCoord.Add(Coord);
        }
    }

    private void drawWorkingZone()
    {
        int n = trajectoireCoord.Count;
        float[] xValues = new float[n];
        float[] yValues = new float[n];
        float[] zValues = new float[n];

        for (int i = 0; i < n; i++)
        {
            xValues[i] = trajectoireCoord[i].x;
            yValues[i] = trajectoireCoord[i].y;
            zValues[i] = trajectoireCoord[i].z;
        }

        Vector3 pointMin = new Vector3(Mathf.Min(xValues), Mathf.Min(yValues), Mathf.Min(zValues));
        Vector3 pointMax = new Vector3(Mathf.Max(xValues), Mathf.Max(yValues), Mathf.Max(zValues));

        //Assuming this is run on a unit cube.
        Vector3 between = pointMax - pointMin;
        float distance = between.magnitude;

        GameObject boundingCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boundingCube.transform.localScale = new Vector3(distance, distance, distance);
        boundingCube.transform.position = pointMin + (between / 2.0f);
        boundingCube.transform.LookAt(pointMax);
        boundingCube.GetComponent<Renderer>().material = workingZoneMaterial;

    }
}
