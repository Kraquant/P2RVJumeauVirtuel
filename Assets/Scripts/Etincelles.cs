using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etincelles : MonoBehaviour
{
    private ParticleSystem.MainModule main;
    private Pendant pendant;
    private bool isInTrajectory;
    private bool isReading;
    private bool isPrinting;

    // Start is called before the first frame update
    void Start()
    {
        main = this.GetComponent<ParticleSystem>().main;
        main.maxParticles = 0;
        pendant = GameObject.Find("Pendant").GetComponent<Pendant>();
        isInTrajectory = false;
        isReading = false;
        isPrinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        isInTrajectory = pendant.mvmtScript.trajectory != null;
        if (isInTrajectory)
        {
            isReading = !pendant.mvmtScript.trajectory.IsReading && pendant.mvmtScript.playingProgress[1] != 0 && pendant.mvmtScript.speed != 0;
            isPrinting = true;
            if (isReading && isPrinting)
            {
                main.maxParticles = 50;
            }
            else
            {
                main.maxParticles = 0;
            }
        }
        else
        {
            main.maxParticles = 0;
        }
    }
}
