using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ecran : MonoBehaviour
{
    public GameObject pendantObject;
    public Pendant pendant;
    public Text txtMode;
    public Text txtCoords;

    private List<string> trajectoires;

    private float angle;

    private GameObject cible;
    private GameObject axe0;
    private GameObject axe1;
    private GameObject axe2;
    private GameObject axe3;
    private GameObject axe4;
    private GameObject axe5;

    private float posX;
    private float posY;
    private float posZ;

    private Mode mode;
    private int axe;
    private bool trajOFF;

    private enum Mode { COORDS, AXES, AUTO}

    // Start is called before the first frame update
    void Start()
    {
        pendant = pendantObject.GetComponent<Pendant>();
        trajOFF = pendant.trajOFF;
        trajectoires = pendant.trajectoires;

        cible = GameObject.Find("Sphere");
        axe0 = GameObject.Find("OsBras1");
        axe1 = GameObject.Find("OsBras2");
        axe2 = GameObject.Find("OsBras3");
        axe3 = GameObject.Find("OsBras4");
        axe4 = GameObject.Find("OsBras5");
        axe5 = GameObject.Find("OsBras6");

        posX = cible.transform.position.x;
        posY = cible.transform.position.y;
        posZ = cible.transform.position.z;

        mode = (Ecran.Mode)pendant.mode;
        axe = pendant.axe;

        switch (axe)
        {
            case 0:
                angle = axe0.transform.localEulerAngles.y;
                break;
            case 1:
                angle = axe1.transform.localEulerAngles.z;
                break;
            case 2:
                angle = axe2.transform.localEulerAngles.z;
                break;
            case 3:
                angle = axe3.transform.localEulerAngles.y;
                break;
            case 4:
                angle = axe4.transform.localEulerAngles.z;
                break;
            case 5:
                angle = axe5.transform.localEulerAngles.y;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mode = (Ecran.Mode)pendant.mode;
        trajOFF = pendant.trajOFF;
        axe = pendant.axe;

        posX = cible.transform.position.x;
        posY = cible.transform.position.y;
        posZ = cible.transform.position.z;

        switch (mode)
        {
            case Mode.COORDS:
                switch (axe)
                {
                    case 0:
                        txtMode.text = "Mode\nCOORDS\n\n\n\nDim. : X\nValeur : " + posX;
                        break;
                    case 1:
                        txtMode.text = "Mode\nCOORDS\n\n\n\nDim. : Y\nValeur : " + posY;
                        break;
                    case 2:
                        txtMode.text = "Mode\nCOORDS\n\n\n\nDim. : Z\nValeur : " + posZ;
                        break;
                }

                txtCoords.text = "\n\nX : " + posX + "\nY : " + posY + "\nZ : " + posZ;
                break;
            case Mode.AXES:
                switch (axe)
                {
                    case 0:
                        angle = axe0.transform.localEulerAngles.y;
                        break;
                    case 1:
                        angle = axe1.transform.localEulerAngles.z;
                        break;
                    case 2:
                        angle = axe2.transform.localEulerAngles.z;
                        break;
                    case 3:
                        angle = axe3.transform.localEulerAngles.y;
                        break;
                    case 4:
                        angle = axe4.transform.localEulerAngles.z;
                        break;
                    case 5:
                        angle = axe5.transform.localEulerAngles.y;
                        break;
                }
                txtMode.text = "Mode\nAXES\n\n\n\nAxe n°" + (axe + 1) + "\nValeur : " + angle;

                txtCoords.text = "\n\nX : " + posX + "\nY : " + posY + "\nZ : " + posZ;
                break;
            case Mode.AUTO:
                if (trajOFF)
                {
                    txtMode.text = "Mode\nAUTO";
                    for (int i = 0; i < axe + 1; i++)
                    {
                        txtMode.text += "\n";
                    }
                    txtMode.text += "=>";

                    txtCoords.text = "\n";
                    for (int i = 0; i < trajectoires.Count; i++)
                        txtCoords.text += "\n" + trajectoires[i];
                }
                else
                {
                    txtMode.text = "Mode\nAUTO";

                    txtCoords.text = "\n\nX : " + posX + "\nY : " + posY + "\nZ : " + posZ;
                }
                break;
        }
    }
}
