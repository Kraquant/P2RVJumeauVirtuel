using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Pendant : MonoBehaviour
{
    public List<string> trajectoires;
    public int axe;
    public float pas_angle;
    public float pas_pos;
    public float pas_angle_rap;
    public float pas_pos_rap;

    public float smoothTime;
    public Vector3 velocity;

    private float posX;
    private float posY;
    private float posZ;

    private float angle0;
    private float angle1;
    private float angle2;
    private float angle3;
    private float angle4;
    private float angle5;

    private GameObject finalIK;
    private GameObject cible;
    private GameObject axe0;
    private GameObject axe1;
    private GameObject axe2;
    private GameObject axe3;
    private GameObject axe4;
    private GameObject axe5;

    private float pressTime;
    private float delay;
    private float pas_pos_current;
    private float pas_angle_current;

    public enum Mode { COORDS, AXES, AUTO }
    public Mode mode;

    public float PosX { get => posX; set => posX = value; }
    public float PosY { get => posY; set => posY = value; }
    public float PosZ { get => posZ; set => posZ = value; }

    public float Angle0 { get => angle0; set => angle0 = value; }
    public float Angle1 { get => angle1; set => angle1 = value; }
    public float Angle2 { get => angle2; set => angle2 = value; }
    public float Angle3 { get => angle3; set => angle3 = value; }
    public float Angle4 { get => angle4; set => angle4 = value; }
    public float Angle5 { get => angle5; set => angle5 = value; }

    // Start is called before the first frame update
    void Start()
    {
        finalIK = GameObject.Find("Bati");
        cible = GameObject.Find("Sphere");
        axe0 = GameObject.Find("OsBras1");
        axe1 = GameObject.Find("OsBras2");
        axe2 = GameObject.Find("OsBras3");
        axe3 = GameObject.Find("OsBras4");
        axe4 = GameObject.Find("OsBras5");
        axe5 = GameObject.Find("OsBras6");

        mode = Mode.COORDS;
        axe = 0;
        pas_angle_rap = 3;
        pas_pos_rap = 0.15f;
        pas_angle = 1;
        pas_pos = 0.05f;

        pressTime = 0;
        delay = 1;

        posX = 0;
        posY = 0;
        posZ = 0;

        angle0 = 0;
        angle1 = 0;
        angle2 = 0;
        angle3 = 0;
        angle4 = 0;
        angle5 = 0;
    }

    public void OnBBasTriggerEnter()
    {
        if (mode != Mode.AUTO)
        {
            axe -= 1;
            if (axe < 0)
            {
                if (mode == Mode.COORDS) { axe = 2; }
                else { axe = 5; }
            }
        }
        else
        {
            axe += 1;
            if (axe > trajectoires.Count - 1) { axe = 0; }
        }
    }

    public void OnBHautTriggerEnter()
    {
        if (mode != Mode.AUTO)
        {
            axe += 1;
            if ((axe > 5 && mode == Mode.AXES) || (axe > 2 && mode == Mode.COORDS)) { axe = 0; }
        }
        else
        {
            axe -= 1;
            if (axe < 0) { axe = trajectoires.Count - 1; }
        }
    }

    public void OnBModeTriggerEnter()
    {
        if (mode == Mode.COORDS)
        {
            finalIK.GetComponent<CCDIK>().enabled = false;
            cible.transform.SetParent(axe5.transform);

            angle0 = 0;
            angle1 = 0;
            angle2 = 0;
            angle3 = 0;
            angle4 = 0;
            angle5 = 0;

            mode = Mode.AXES;
        }
        else if (mode == Mode.AXES)
        {
            cible.transform.SetParent(null);
            finalIK.GetComponent<CCDIK>().enabled = true;

            mode = Mode.AUTO;
        }
        else
        {
            posX = 0;
            posY = 0;
            posZ = 0;

            mode = Mode.COORDS;
        }
        axe = 0;
    }

    public void OnBMoinsTriggerEnter()
    {
        pressTime = Time.time;
    }

    public void OnBPlusTriggerEnter()
    {
        pressTime = Time.time;
    }

    public void OnBMoinsTriggerStay()
    {
        Debug.Log(Time.time - pressTime);
        if (Time.time - pressTime < delay)
        {
            pas_pos_current = pas_pos;
            pas_angle_current = pas_angle;
        }
        else
        {
            pas_pos_current = pas_pos_rap;
            pas_angle_current = pas_angle_rap;
        }
        switch (axe)
        {
            case 0:
                if (mode == Mode.COORDS) { posX = -pas_pos_current; }
                else if (mode == Mode.AXES) { angle0 = -pas_angle_current; }
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
                if (mode == Mode.AXES) { angle3 = -pas_angle_current; }
                break;
            case 4:
                if (mode == Mode.AXES) { angle4 = -pas_angle_current; }
                break;
            case 5:
                if (mode == Mode.AXES) { angle5 = -pas_angle_current; }
                break;
            default:
                break;
        }
    }

    public void OnBPlusTriggerStay()
    {
        Debug.Log(Time.time - pressTime);
        if (Time.time - pressTime < delay)
        {
            pas_pos_current = pas_pos;
            pas_angle_current = pas_angle;
        }
        else
        {
            pas_pos_current = pas_pos_rap;
            pas_angle_current = pas_angle_rap;
        }
        switch (axe)
        {
            case 0:
                if (mode == Mode.COORDS) { posX = pas_pos_current; }
                else if (mode == Mode.AXES) { angle0 = pas_angle_current; }
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
                if (mode == Mode.AXES) { angle3 = pas_angle_current; }
                break;
            case 4:
                if (mode == Mode.AXES) { angle4 = pas_angle_current; }
                break;
            case 5:
                if (mode == Mode.AXES) { angle5 = pas_angle_current; }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == Mode.COORDS)
        {
            cible.transform.position = Vector3.SmoothDamp(cible.transform.position, cible.transform.position + new Vector3(posX, posY, posZ), ref velocity, smoothTime);

            posX = 0;
            posY = 0;
            posZ = 0;
        }
        else if (mode == Mode.AXES)
        {
            if (angle1 < -50 + pas_angle) { angle1 = 0; }
            if (angle1 > 160 - pas_angle) { angle1 = 0; }
            if (angle2 < -155 + pas_angle) { angle2 = 0; }
            if (angle2 > 165 - pas_angle) { angle2 = 0; }

            axe0.transform.localEulerAngles = Vector3.Lerp(axe0.transform.localEulerAngles, axe0.transform.localEulerAngles + new Vector3(0, angle0, 0), Time.deltaTime * 10);
            axe1.transform.localEulerAngles = Vector3.Lerp(axe1.transform.localEulerAngles, axe1.transform.localEulerAngles + new Vector3(0, 0, angle1), Time.deltaTime * 10);
            axe2.transform.localEulerAngles = Vector3.Lerp(axe2.transform.localEulerAngles, axe2.transform.localEulerAngles + new Vector3(0, 0, angle2), Time.deltaTime * 10);
            axe3.transform.localEulerAngles = Vector3.Lerp(axe3.transform.localEulerAngles, axe3.transform.localEulerAngles + new Vector3(0, angle3, 0), Time.deltaTime * 10);
            axe4.transform.localEulerAngles = Vector3.Lerp(axe4.transform.localEulerAngles, axe4.transform.localEulerAngles + new Vector3(0, 0, angle4), Time.deltaTime * 10);
            axe5.transform.localEulerAngles = Vector3.Lerp(axe5.transform.localEulerAngles, axe5.transform.localEulerAngles + new Vector3(0, angle5, 0), Time.deltaTime * 10);

            angle0 = 0;
            angle1 = 0;
            angle2 = 0;
            angle3 = 0;
            angle4 = 0;
            angle5 = 0;
        }
    }
}

========================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ecran : MonoBehaviour
{
    public GameObject pendantObject;
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

    private enum Mode { COORDS, AXES, AUTO}

    // Start is called before the first frame update
    void Start()
    {
        Pendant pendant = pendantObject.GetComponent<Pendant>();
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
        Pendant pendant = pendantObject.GetComponent<Pendant>();
        mode = (Ecran.Mode)pendant.mode;
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
                        txtMode.text = "Mode\nCOORDS\n\n\nDim. : X\nValeur : " + posX;
                        break;
                    case 1:
                        txtMode.text = "Mode\nCOORDS\n\n\nDim. : Y\nValeur : " + posY;
                        break;
                    case 2:
                        txtMode.text = "Mode\nCOORDS\n\n\nDim. : Z\nValeur : " + posZ;
                        break;
                }

                txtCoords.text = "X : " + posX + "\nY : " + posY + "\nZ : " + posZ;
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
                txtMode.text = "Mode\nAXES\n\n\nAxe n°" + (axe + 1) + "\nValeur : " + angle;

                txtCoords.text = "X : " + posX + "\nY : " + posY + "\nZ : " + posZ;
                break;
            case Mode.AUTO:
                txtMode.text = "Mode\nAUTO";
                for (int i = 0; i < axe + 1; i++)
                {
                    txtMode.text += "\n";
                }
                txtMode.text += "=>";

                txtCoords.text = "";
                for (int i = 0; i < trajectoires.Count; i++)
                    txtCoords.text += "\n" + trajectoires[i];
                break;
        }
    }
}
