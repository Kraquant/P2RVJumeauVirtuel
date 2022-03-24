using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System.IO;

public class Pendant : MonoBehaviour
{
    public string path;
    public FileInfo[] trajectoires; // Liste des fichiers trajectoires
    public int axe; // Indice de l'axe, de la trajectoire ou du bras courant 
    public float pas_angle; // Pas de deplacement en angle par frame
    public float pas_pos; // Pas de deplacement en distance par frame
    public float pas_angle_rap; // Pas de deplacement en angle par frame pendant un appui prolonge
    public float pas_pos_rap; // Pas de deplacement en angle par frame pendant un appui prolonge

    public float smoothTime; // Delai de deplacement
    public Vector3 velocity; // Vitesse de deplacement

    // Coordonnees de la torche
    private float posX;
    private float posY;
    private float posZ;

    // Modifications à apporter aux angles des bras du robot
    private float angle0;
    private float angle1;
    private float angle2;
    private float angle3;
    private float angle4;
    private float angle5;
    private float angle6;
    private float angle7;

    // Variables permettant le calcul des limites d'angles des bras 2 et 3
    private float limit1;
    private float limit2;
    private float limit6;

    // Elements permettant les calculs de cinematique inverse
    private GameObject finalIK;
    private CCDIK[] IKs;
    private GameObject motionManager;
    public FileMovement mvmtScript;

    // Composants du robot
    private GameObject cible;
    private GameObject axe0;
    private GameObject axe1;
    private GameObject axe2;
    private GameObject axe3;
    private GameObject axe4;
    private GameObject axe5;
    private GameObject axe6;
    private GameObject axe7;
    private GameObject torche;

    // Textures des boutons du pendant
    public Material bPlus;
    public Material bPlusL;
    public Material bMoins;
    public Material bMoinsL;
    public Material bPlay;
    public Material bFF;
    public Material bFFL;
    public Material bFFG;
    public Material bMode;
    public Material bModeG;
    public Material bStop;
    public Material bStopG;
    public Material bArrUp;
    public Material bArrUpG;
    public Material bArrDo;
    public Material bArrDoG;

    // Boutons du pendant
    private GameObject boutonDroit;
    private GameObject boutonGauche;
    private GameObject boutonMode;
    private GameObject boutonStop;
    private GameObject boutonArrUp;
    private GameObject boutonArrDo;
    public bool trajOFF; // false : une trajectoire est en cours de lecture (sinon, false)

    // Variables de gestion des delais d'appui prolonge
    private float pressTime; // Date d'appui
    private int readingInit; // Lancement de la lecture d'un fichier :
                             // 0 = pas en cours, 1 = en cours, 2 = fini, 3 = trajectoire en cours de lecture
    private float delay; // Temps minimum d'appui pour le considerer comme "prolonge"
    private float pas_pos_current; // Pas en distance courant
    private float pas_angle_current; // Pas en angle courant

    // Modes du pendant
    public enum Mode { COORDS, AXES, AUTO }
    public Mode mode;

    // Accesseurs pour les variables privees utiles ailleurs
    public float PosX { get => posX; set => posX = value; }
    public float PosY { get => posY; set => posY = value; }
    public float PosZ { get => posZ; set => posZ = value; }

    public float Angle0 { get => angle0; set => angle0 = value; }
    public float Angle1 { get => angle1; set => angle1 = value; }
    public float Angle2 { get => angle2; set => angle2 = value; }
    public float Angle3 { get => angle3; set => angle3 = value; }
    public float Angle4 { get => angle4; set => angle4 = value; }
    public float Angle5 { get => angle5; set => angle5 = value; }
    public float Angle6 { get => angle5; set => angle5 = value; }
    public float Angle7 { get => angle5; set => angle5 = value; }

    // Au lancement :
    void Start()
    {
        // Initialisation de toutes les variables
        // et recuperation des parametres exterieurs

        finalIK = GameObject.Find("Bati");
        IKs = finalIK.GetComponents<CCDIK>();
        IKs[0].enabled = true;
        IKs[1].enabled = false;
        IKs[2].enabled = false;
        motionManager = GameObject.Find("MotionManager");
        mvmtScript = motionManager.GetComponent<FileMovement>();

        cible = GameObject.Find("Sphere");
        axe0 = GameObject.Find("OsBras1");
        axe1 = GameObject.Find("OsBras2");
        axe2 = GameObject.Find("OsBras3");
        axe3 = GameObject.Find("OsBras4");
        axe4 = GameObject.Find("OsBras5");
        axe5 = GameObject.Find("OsBras6");
        torche = GameObject.Find("torcheTresExtreme");
        cible.transform.SetParent(null);
        axe6 = GameObject.Find("OsPlateau7");
        axe7 = GameObject.Find("OsPlateau8");

        boutonDroit = GameObject.Find("ContextualRight");
        boutonGauche = GameObject.Find("ContextualLeft");
        boutonMode = GameObject.Find("ModeButton");
        boutonArrUp = GameObject.Find("ArrowUp");
        boutonArrDo = GameObject.Find("ArrowDown");
        boutonStop = GameObject.Find("StopButton");
        trajOFF = true;

        // Chemin relatif au dossier des trajectoires (en l'etat il faut mettre "\Trajectoires")
        path = (Application.dataPath + path).Replace("/", "\\");
        DirectoryInfo info = new DirectoryInfo(path);
        trajectoires = info.GetFiles();
        // On retire les fichiers META de la liste des trajectoires
        FileInfo[] newTrajectoires = new FileInfo[trajectoires.Length / 2];
        int j = 0;
        foreach (FileInfo trajectoire in trajectoires)
        {
            if (!trajectoire.Name.Contains(".meta"))
            {
                newTrajectoires[j] = trajectoire;
                j++;
            }
        }
        trajectoires = newTrajectoires;

        boutonGauche.tag = "Minus";
        boutonDroit.tag = "Plus";

        boutonStop.GetComponent<MeshRenderer>().material = bStopG;
        boutonGauche.GetComponent<MeshRenderer>().material = bMoins;
        boutonDroit.GetComponent<MeshRenderer>().material = bPlus;
        boutonArrUp.GetComponent<MeshRenderer>().material = bArrUp;
        boutonArrDo.GetComponent<MeshRenderer>().material = bArrDo;
        boutonMode.GetComponent<MeshRenderer>().material = bMode;

        mode = Mode.COORDS;
        axe = 0;
        pas_angle_rap = 3;
        pas_pos_rap = 0.15f;
        pas_angle = 1;
        pas_pos = 0.05f;
        smoothTime = 0.3f;

        pressTime = 0;
        delay = 1;
        readingInit = 0;

        posX = 0;
        posY = 0;
        posZ = 0;

        angle0 = 0;
        angle1 = 0;
        angle2 = 0;
        angle3 = 0;
        angle4 = 0;
        angle5 = 0;
        angle6 = 0;
        angle7 = 0;

        limit1 = 0;
        limit2 = 0;
        limit6 = 0;
    }

    void EndTrajectory()
    {
        readingInit = 0;
        trajOFF = true;
        // On met a jour l'aspect des boutons actifs et inactifs
        boutonArrUp.GetComponent<MeshRenderer>().material = bArrUp;
        boutonArrDo.GetComponent<MeshRenderer>().material = bArrDo;
        boutonDroit.GetComponent<MeshRenderer>().material = bFFG;
        boutonStop.GetComponent<MeshRenderer>().material = bStopG;
        boutonMode.GetComponent<MeshRenderer>().material = bMode;

        // On arrete la lecture du fichier
        // et on quitte le controle via un fichier trajectoire
        cible.transform.position = motionManager.transform.position;
        mvmtScript.stopPlaying();
        IKs[1].enabled = false;
        IKs[2].enabled = false;
        IKs[0].enabled = true;
    }

    // A l'appui sur le bouton "Fleche Bas"
    public void OnBDownTriggerEnter()
    {
        // En mode COORDS ou AXES :
        if (mode != Mode.AUTO)
        {
            axe -= 1;
            if (axe < 0)
            {
                // En mode COORDS, il y a 3 "axes" differents (X, Y, Z)
                if (mode == Mode.COORDS) { axe = 2; }
                // En mode AXES, il y en a 6 (bras 1 a 6)
                else { axe = 7; } 
            }
        }
        // En mode AUTO sans trajectoire en cours de lecture :
        else if (trajOFF) 
        {
            axe += 1;
            if (axe > trajectoires.Length - 1) { axe = 0; }
        }
    }

    // A l'appui sur le bouton "Fleche Haut"
    public void OnBUpTriggerEnter()
    {
        // En mode COORDS ou AXES :
        if (mode != Mode.AUTO) 
        {
            axe += 1;
            // Le mode AXES a 6 "axes", le mode COORDS en a 3
            if ((axe > 7 && mode == Mode.AXES) || (axe > 2 && mode == Mode.COORDS)) { axe = 0; }
        }
        // En mode AUTO sans trajectoire en cours de lecture :
        else if (trajOFF) 
        {
            axe -= 1;
            if (axe < 0) { axe = trajectoires.Length - 1; }
        }
    }

    // A l'appui sur le bouton "Mode"
    public void OnBModeTriggerEnter()
    {
        // En mode COORDS, on passe au mode AXES
        if (mode == Mode.COORDS)
        {
            // Le controle ne se fait plus en suivant la cible
            IKs[0].enabled = false;
            cible.transform.SetParent(torche.transform);

            // On elimine les angles obsoletes
            angle0 = 0;
            angle1 = 0;
            angle2 = 0;
            angle3 = 0;
            angle4 = 0;
            angle5 = 0;
            angle6 = 0;
            angle7 = 0;

            mode = Mode.AXES;
        }
        // En mode AXES, on passe au mode AUTO
        else if (mode == Mode.AXES)
        {
            // On change l'aspect des boutons sous l'ecran
            boutonDroit.GetComponent<MeshRenderer>().material = bFFG;
            boutonDroit.tag = "FF";
            boutonGauche.GetComponent<MeshRenderer>().material = bPlay;
            boutonGauche.tag = "Play";
            cible.transform.SetParent(null);

            mode = Mode.AUTO;
        }
        // En mode AUTO, le bouton "Mode" n'est actif que si aucune trajectoire n'est en cours de lecture
        // Si changement il peut y avoir, on passe au mode COORDS
        else if (trajOFF)
        {
            // On elimine les coordonnees obsoletes
            posX = 0;
            posY = 0;
            posZ = 0;

            // On change l'aspect des boutons sous l'ecran
            boutonDroit.GetComponent<MeshRenderer>().material = bPlus;
            boutonDroit.tag = "Plus";
            boutonGauche.GetComponent<MeshRenderer>().material = bMoins;
            boutonGauche.tag = "Minus";

            mode = Mode.COORDS;

            // On passe en controle par suivi de la cible
            IKs[1].enabled = false;
            IKs[2].enabled = false;
            IKs[0].enabled = true;
        }
        // On reinitialise la valeur de l'iterateur
        axe = 0;
    }

    // A l'appui sur le bouton "Moins"
    public void OnBMinusTriggerEnter()
    {
        // On lance le chronometre
        pressTime = Time.time;
        // On met à jour l'apparence du bouton
        boutonGauche.GetComponent<MeshRenderer>().material = bMoinsL;
    }

    // A l'appui sur le bouton "Plus"
    public void OnBPlusTriggerEnter()
    {
        // On lance le chronometre
        pressTime = Time.time;
        // On met à jour l'apparence du bouton
        boutonDroit.GetComponent<MeshRenderer>().material = bPlusL;
    }

    // A l'appui sur le bouton "Lecture/Pause"
    public void OnBPlayTriggerEnter()
    {
        // Si aucune trajectoire n'est en cours de lecture :
        if (trajOFF)
        {
            trajOFF = false;

            // On met a jour l'aspect des boutons actifs et inactifs
            boutonArrUp.GetComponent<MeshRenderer>().material = bArrUpG;
            boutonArrDo.GetComponent<MeshRenderer>().material = bArrDoG;
            boutonDroit.GetComponent<MeshRenderer>().material = bFF;
            boutonStop.GetComponent<MeshRenderer>().material = bStop;
            boutonMode.GetComponent<MeshRenderer>().material = bModeG;

            // On lance la lecture du fichier trajectoire
            readingInit = 1;
            IKs[0].enabled = true;
        }
        // Si une trajectoire est deja en cours de lecture :
        else
        {
            // Si la lecture etait en pause, on la relance
            if (mvmtScript.speed == 0) { mvmtScript.speed = 1; }
            // Sinon, on la met en pause
            else { mvmtScript.speed = 0; }
        }
    }

    // A l'appui sur le bouton "Avance Rapide"
    public void OnBFFTriggerEnter()
    {
        // On lance le chronometre
        pressTime = Time.time;
        // On multiplie la vitesse de lecture par 2 (appui bref)
        mvmtScript.speed *= 2;
        // Si la vitesse etait deja au plus haut, on la reinitialise
        if (mvmtScript.speed > 128) { mvmtScript.speed = 1; }
        // On met à jour l'apparence du bouton
        boutonDroit.GetComponent<MeshRenderer>().material = bFFL;
    }

    // A l'appui sur le bouton "Stop"
    public void OnBSTOPTriggerEnter()
    {
        // Le bouton n'est actif que si une trajectoire est en cours de lecture
        if (!trajOFF) { EndTrajectory(); }
    }

    #region OnTriggerStays

    // Apres un appui prolonge sur le bouton "Moins"
    public void OnBMinusTriggerStay()
    {
        // Si l'appui est bref ou avant le depassement du delai :
        if (Time.time - pressTime < delay)
        {
            // On utilise les petits pas
            pas_pos_current = pas_pos;
            pas_angle_current = pas_angle;
        }
        // Si le temps d'appui est long :
        else
        {
            // On utilise les grands pas
            pas_pos_current = pas_pos_rap;
            pas_angle_current = pas_angle_rap;
        }
        switch (axe)
        {
            case 0:
                // En mode COORDS, on modifie la position de la cible
                if (mode == Mode.COORDS) { posX = -pas_pos_current; }
                // En mode AXES, on modifie les angles des bras du robot
                else if (mode == Mode.AXES) { angle0 = -pas_angle_current; }
                // En mode AUTO, le bouton est remplac� par le bouton "Lecture/Pause"
                break;
            case 1:
                if (mode == Mode.COORDS) { posY = -pas_pos_current; }
                else if (mode == Mode.AXES) { angle1 = -pas_angle_current; }
                break;
            case 2:
                if (mode == Mode.COORDS) { posZ = -pas_pos_current; }
                else if (mode == Mode.AXES) { angle2 = -pas_angle_current; }
                break;
            case 3:
                // En mode COORDS, il n'y a que 3 "axes" (X, Y, Z)
                if (mode == Mode.AXES) { angle3 = -pas_angle_current; }
                break;
            case 4:
                if (mode == Mode.AXES) { angle4 = -pas_angle_current; }
                break;
            case 5:
                if (mode == Mode.AXES) { angle5 = -pas_angle_current; }
                break;
            case 6:
                if (mode == Mode.AXES) { angle6 = -pas_angle_current; }
                break;
            case 7:
                if (mode == Mode.AXES) { angle7 = -pas_angle_current; }
                break;
            default:
                break;
        }
    }

    // Apres un appui prolonge sur le bouton "Plus"
    public void OnBPlusTriggerStay()
    {
        // Si l'appui est bref ou avant le depassement du delai :
        if (Time.time - pressTime < delay)
        {
            // On utilise les petits pas
            pas_pos_current = pas_pos;
            pas_angle_current = pas_angle;
        }
        // Si le temps d'appui est long :
        else
        {
            // On utilise les grands pas
            pas_pos_current = pas_pos_rap;
            pas_angle_current = pas_angle_rap;
        }
        switch (axe)
        {
            case 0:
                // En mode COORDS, on modifie la position de la cible
                if (mode == Mode.COORDS) { posX = pas_pos_current; }
                // En mode AXES, on modifie les angles des bras du robot
                else if (mode == Mode.AXES) { angle0 = pas_angle_current; }
                // En mode AUTO, le bouton est remplace par le bouton "Avance Rapide"
                break;
            case 1:
                if (mode == Mode.COORDS) { posY = pas_pos_current; }
                else if (mode == Mode.AXES) { angle1 = pas_angle_current; }
                break;
            case 2:
                if (mode == Mode.COORDS) { posZ = pas_pos_current; }
                else if (mode == Mode.AXES) { angle2 = pas_angle_current; }
                break;
            case 3:
                // En mode COORDS, il n'y a que 3 "axes" (X, Y, Z)
                if (mode == Mode.AXES) { angle3 = pas_angle_current; }
                break;
            case 4:
                if (mode == Mode.AXES) { angle4 = pas_angle_current; }
                break;
            case 5:
                if (mode == Mode.AXES) { angle5 = pas_angle_current; }
                break;
            case 6:
                if (mode == Mode.AXES) { angle6 = pas_angle_current; }
                break;
            case 7:
                if (mode == Mode.AXES) { angle7 = pas_angle_current; }
                break;
            default:
                break;
        }
    }

    // Apres un appui prolonge sur le bouton "Avance Rapide"
    public void OnBFFTriggerStay()
    {
        // Si le temps d'appui est long, on lit la trajectoire en vitesse tres rapide
        if (Time.time - pressTime > delay) { mvmtScript.speed = 32; }
    }

    #endregion

    #region OnTriggerRelease

    // Quand on relache le bouton "Avance Rapide"
    public void OnBFFTriggerExit()
    {
        // Si c'etait un appui long, on reinitialise la vitesse de lecture
        if (Time.time - pressTime > delay) { mvmtScript.speed = 1; }
        // On met à jour l'apparence du bouton
        boutonDroit.GetComponent<MeshRenderer>().material = bFF;
    }

    public void OnBMinusTriggerExit()
    {
        // On met à jour l'apparence du bouton
        boutonGauche.GetComponent<MeshRenderer>().material = bMoins;
    }

    public void OnBPlusTriggerExit()
    {
        // On met à jour l'apparence du bouton
        boutonDroit.GetComponent<MeshRenderer>().material = bPlus;
    }

    #endregion

    // A chaque frame :
    void Update()
    {
        // En mode COORDS :
        if (mode == Mode.COORDS)
        {
            // On deplace la cible vers sa nouvelle position
            cible.transform.position = Vector3.SmoothDamp(cible.transform.position, cible.transform.position + new Vector3(posX, posY, posZ), ref velocity, smoothTime);

            // On reinitialise les pas en position
            posX = 0;
            posY = 0;
            posZ = 0;
        }
        // En mode AXES :
        else if (mode == Mode.AXES)
        {
            // On stock les valeurs d'angles courantes des bras 2 et 3
            limit1 = axe1.transform.localEulerAngles.z;
            limit2 = axe2.transform.localEulerAngles.z;
            limit6 = axe6.transform.localEulerAngles.z;

            // Si le bras 2 depasse ses limites de mouvements :
            if (limit1 + angle1 < 309 && limit1 + angle1 > 161)
            {
                // On le tourne vers la limite d'angle la plus proche de la valeur d'angle attendue
                if (limit1 + angle1 > 235) { axe1.transform.localEulerAngles = Vector3.Lerp(axe1.transform.localEulerAngles, new Vector3(0, 0, 310), Time.deltaTime * 10); }
                else { axe1.transform.localEulerAngles = Vector3.Lerp(axe1.transform.localEulerAngles, new Vector3(0, 0, 160), Time.deltaTime * 10); }
            }
            // Sinon, on le tourne de la valeur d'angle attendue
            else { axe1.transform.localEulerAngles = Vector3.Lerp(axe1.transform.localEulerAngles, axe1.transform.localEulerAngles + new Vector3(0, 0, angle1), Time.deltaTime * 10); }

            // Si le bras 3 depasse ses limites de mouvements :
            if (limit2 + angle2 < 206 && limit2 + angle2 > 164)
            {
                // On le tourne vers la limite d'angle la plus proche de la valeur d'angle attendue
                if (limit2 + angle2 > 185) { axe2.transform.localEulerAngles = Vector3.Lerp(axe2.transform.localEulerAngles, new Vector3(0, 0, 205), Time.deltaTime * 10); }
                else { axe2.transform.localEulerAngles = Vector3.Lerp(axe2.transform.localEulerAngles, new Vector3(0, 0, 165), Time.deltaTime * 10); }
            }
            // Sinon, on le tourne de la valeur d'angle attendue
            else { axe2.transform.localEulerAngles = Vector3.Lerp(axe2.transform.localEulerAngles, axe2.transform.localEulerAngles + new Vector3(0, 0, angle2), Time.deltaTime * 10); }

            // Si le bras 7 depasse ses limites de mouvements :
            if (limit6 + angle6 < 266 && limit6 + angle6 > 4)
            {
                // On le tourne vers la limite d'angle la plus proche de la valeur d'angle attendue
                if (limit6 + angle6 > 135) { axe6.transform.localEulerAngles = Vector3.Lerp(axe6.transform.localEulerAngles, new Vector3(0, 0, 265), Time.deltaTime * 10); }
                else { axe6.transform.localEulerAngles = Vector3.Lerp(axe6.transform.localEulerAngles, new Vector3(0, 0, 5), Time.deltaTime * 10); }
            }
            // Sinon, on le tourne de la valeur d'angle attendue
            else { axe6.transform.localEulerAngles = Vector3.Lerp(axe6.transform.localEulerAngles, axe6.transform.localEulerAngles + new Vector3(0, 0, angle6), Time.deltaTime * 10); }


            // On effectue la rotation des autres bras (ils n'ont pas de limites d'angle propres)
            axe0.transform.localEulerAngles = Vector3.Lerp(axe0.transform.localEulerAngles, axe0.transform.localEulerAngles + new Vector3(0, angle0, 0), Time.deltaTime * 10);
            axe3.transform.localEulerAngles = Vector3.Lerp(axe3.transform.localEulerAngles, axe3.transform.localEulerAngles + new Vector3(0, angle3, 0), Time.deltaTime * 10);
            axe4.transform.localEulerAngles = Vector3.Lerp(axe4.transform.localEulerAngles, axe4.transform.localEulerAngles + new Vector3(0, 0, angle4), Time.deltaTime * 10);
            axe5.transform.localEulerAngles = Vector3.Lerp(axe5.transform.localEulerAngles, axe5.transform.localEulerAngles + new Vector3(0, angle5, 0), Time.deltaTime * 10);
            axe7.transform.localEulerAngles = Vector3.Lerp(axe7.transform.localEulerAngles, axe7.transform.localEulerAngles + new Vector3(0, angle7, 0), Time.deltaTime * 10);

            // On reinitialise les pas d'angles
            angle0 = 0;
            angle1 = 0;
            angle2 = 0;
            angle3 = 0;
            angle4 = 0;
            angle5 = 0;
            angle6 = 0;
            angle7 = 0;
        }
        // En mode AUTO :
        // Avant de lancer la lecture d'un fichier trajectoire
        else if (readingInit == 1)
        {
            // On déplace la torche vers la position de départ du fichier
            cible.transform.position = Vector3.SmoothDamp(cible.transform.position, motionManager.transform.position, ref velocity, smoothTime);
            if (cible.transform.position.x - motionManager.transform.position.x < 0.05f && cible.transform.position.y - motionManager.transform.position.y < 0.05f && cible.transform.position.z - motionManager.transform.position.z < 0.05f)
            {
                // Quand la position est atteinte, on lance la lecture du fichier
                readingInit = 2;
            }
        }
        // Pour lancer la lecture du fichier
        else if (readingInit == 2)
        {
            // On passe en controle via un fichier trajectoire
            IKs[0].enabled = false;
            IKs[1].enabled = true;
            IKs[2].enabled = true;
            readingInit = 3;

            // Lancement de la lecture du fichier
            mvmtScript.speed = 1;
            string trajectoire = path + "\\" + trajectoires[axe].Name;
            mvmtScript.loadNewFile(trajectoire);
            mvmtScript.togglePlaying();
        }
        // Pour arreter la lecture du fichier
        else if (readingInit == 3 && !mvmtScript.isActive) { EndTrajectory(); }
    }
}
