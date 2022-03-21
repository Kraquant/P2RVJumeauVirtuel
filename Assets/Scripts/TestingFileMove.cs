using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingFileMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject fileInteractionSphere;
    [SerializeField] TextMesh Texte3D;

    [SerializeField] bool callReadFile;
    [SerializeField] bool callStopPlaying;
    [SerializeField] bool callTogglePlaying;
    


    private FileMovement controller;
    void Start()
    {
        controller = fileInteractionSphere.GetComponent<FileMovement>();
    }

    void Update()
    {
        Texte3D.text = "Progression de la lecture des points: " + controller.playingProgress[0].ToString() + "/" + controller.playingProgress[1].ToString();
        //if (controller.trajectory != null) Debug.Log(controller.trajectory.readingStatus[0] + "/" + controller.trajectory.readingStatus[1]);
        if (callReadFile)
        {
            controller.loadNewFile((Application.dataPath + "/Trajectoires/3D Tests.txt").Replace("/", "\\"));
            callReadFile = false;
        }

        if(callStopPlaying)
        {
            controller.stopPlaying();
            callStopPlaying = false;
        }

        if(callTogglePlaying)
        {
            controller.togglePlaying();
            callTogglePlaying = false;
        }
    }
}
