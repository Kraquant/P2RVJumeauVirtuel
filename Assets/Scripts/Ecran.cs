using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Ecran : MonoBehaviour
{
    public GameObject pendantObject;

    [HideInInspector]
    // Script du pendant
    public Pendant pendant;

    // Texte affich� � l'�cran
    public Text txtMode;
    public Text txtCoords;

    // Param�tres de position de la torche
    private float angle;
    private float posX;
    private float posY;
    private float posZ;

    // Composants du robot
    private GameObject cible;
    private GameObject axe0;
    private GameObject axe1;
    private GameObject axe2;
    private GameObject axe3;
    private GameObject axe4;
    private GameObject axe5;

    // Mode et param�tres du pendant
    private Mode mode;
    private int axe;
    private bool trajOFF;
    private enum Mode { COORDS, AXES, AUTO }

    // Au lancement :
    void Start()
    {
        // Initialisation de toutes les variables
        // et r�cup�ration des param�tres ext�rieurs

        pendant = pendantObject.GetComponent<Pendant>();
        trajOFF = pendant.trajOFF;
        
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

        // "angle" prend la valeur d'angle de l'axe courant
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

    // Une fois par frame :
    void Update()
    {
        // Mise � jour du mode du pendant
        mode = (Ecran.Mode)pendant.mode;
        trajOFF = pendant.trajOFF;
        axe = pendant.axe;

        // Mise � jour de la position de la cible
        posX = cible.transform.position.x;
        posY = cible.transform.position.y;
        posZ = cible.transform.position.z;

        switch (mode)
        {
            // En mode COORDS :
            case Mode.COORDS:
                // On affiche la valeur de position de la torche selon la dimension courante
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

                // et la position de la torche
                txtCoords.text = "\n\nX : " + posX + "\nY : " + posY + "\nZ : " + posZ;
                break;
            // En mode AXES :
            case Mode.AXES:
                // Mise � jour de la valeur d'"angle"
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
                // On affiche la valeur d'angle de l'axe courant
                txtMode.text = "Mode\nAXES\n\n\n\nAxe n�" + (axe + 1) + "\nValeur : " + angle;

                // et la position de la torche
                txtCoords.text = "\n\nX : " + posX + "\nY : " + posY + "\nZ : " + posZ;
                break;
            // En mode AUTO :
            case Mode.AUTO:

                // Si aucune trajectoire n'est lue :
                if (trajOFF)
                {

                    // On affiche le curseur de s�lection du fichier trajectoire
                    txtMode.text = "Mode\nAUTO";
                    for (int i = 0; i < axe + 1; i++)
                    {
                        txtMode.text += "\n";
                    }
                    txtMode.text += "=>";

                    txtCoords.text = "\n";
                    for (int i = 0; i < pendant.trajectoires.Length; i++)
                        txtCoords.text += "\n" + pendant.trajectoires[i].Name;
                }
                // Si une trajectoire est en cours de lecture :
                else
                {
                    // On affiche la position de la torche
                    txtMode.text = "Mode\nAUTO";
                    txtCoords.text = "\n\nX : " + posX + "\nY : " + posY + "\nZ : " + posZ;
                }
                break;
        }
    }
}
