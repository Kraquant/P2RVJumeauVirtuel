using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class FileInteraction : MonoBehaviour
{
    public TextAsset trajectoireFile;
    public bool loop = true;
    public float duration = 10000f; //Temps de l'animation
    public float scale = 0.001f; //Ecart entre les points
    public float pointScale = 1.0f;
    public GameObject billBoard;
    public Material drawnMaterial;
    public Material undrawnMaterial;

    private List<Vector3> trajectoireFileCoord; //Vecteur des coordonnees du fichier
    private List<Vector3> trajectoireCoord; //Trajectoire mise a l'echelle pour unity
    private Vector3 initPos; //Position de départ de la trajectoire
    private float tInit;
    private int stepMax;
    private bool isActive = true;
    private List<GameObject> billBoardList;
    private int previousStep;

    private void Start()
    {
        //Initializing variables
        billBoardList = new List<GameObject>();
        trajectoireFileCoord = new List<Vector3>();
        trajectoireCoord = new List<Vector3>();
        initPos = transform.position;
        previousStep = 0;

        //Reading file and adapting coordinates to unity
        readFile();
        calculateWorkingZone();
        //drawWorkingZone();
        showTrajectoryPoints();

        //Setting up time
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
                previousStep = 0;
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
            int step = Mathf.FloorToInt(timeStep);
            if (step == stepMax)
            {
         
                transform.position = Vector3.Lerp(trajectoireCoord[stepMax], trajectoireCoord[0], timeStep - step);
                //reset Materials
                resetMaterial();
            }
            else
            {
                transform.position = Vector3.Lerp(trajectoireCoord[step], trajectoireCoord[step + 1], timeStep - step);
                
                //Change Material points
                colorPoints(previousStep, step);

                previousStep = step;
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
                trajectoireFileCoord.Add(new Vector3(xF, -zF, yF));
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

    private void showTrajectoryPoints()
    {
        GameObject pointsHolder = new GameObject();
        pointsHolder.name = "CoordinatesPoints";
        int count = 0;
        foreach(Vector3 pointCoord in trajectoireCoord)
        {
            if (count % 10 == 0)
            {
                GameObject point = Object.Instantiate(billBoard);
                point.transform.position = pointCoord;
                point.transform.localScale = new Vector3(pointScale, pointScale, pointScale);
                point.transform.SetParent(pointsHolder.transform);
                billBoardList.Add(point);
            }
            count++;
            

        }
    }

    private void colorPoints(int previousStep, int step)
    {
        for (int i = previousStep; i < step; i++)
        {
            if (i % 10 == 0)
            {
                billBoardList[i%10].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = drawnMaterial;
            }
        }
    }

    private void resetMaterial()
    {
        foreach(GameObject billBoard in billBoardList)
        {
            
            billBoard.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = undrawnMaterial;
        }
    }
}
