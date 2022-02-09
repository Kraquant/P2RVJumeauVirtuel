using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Pendant : MonoBehaviour
{
    public int axe;
    public float pas_angle;
    public float pas_pos;
    public float pas_angle_continu;
    public float pas_pos_continu;

    private float offset;

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
    private GameObject torche;
    private GameObject cible;
    private GameObject axe0;
    private GameObject axe1;
    private GameObject axe2;
    private GameObject axe3;
    private GameObject axe4;
    private GameObject axe5;

    public enum Mode { AXES, COORDS, AUTO }
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
        torche = GameObject.Find("Bras6");
        cible = GameObject.Find("Sphere");
        axe0 = GameObject.Find("OsBras1");
        axe1 = GameObject.Find("OsBras2");
        axe2 = GameObject.Find("OsBras3");
        axe3 = GameObject.Find("OsBras4");
        axe4 = GameObject.Find("OsBras5");
        axe5 = GameObject.Find("OsBras6");

        mode = Mode.COORDS;
        axe = 0;
        pas_angle_continu = 0.5f;
        pas_pos_continu = 0.5f;
        pas_angle = 2;
        pas_pos = 2;
        offset = Vector3.Distance(cible.transform.position, torche.transform.position);
        
        posX = cible.transform.position.x;
        posY = cible.transform.position.y;
        posZ = cible.transform.position.z;

        angle0 = axe0.transform.localEulerAngles.y;
        angle1 = axe1.transform.localEulerAngles.z;
        angle2 = axe2.transform.localEulerAngles.z;
        angle3 = axe3.transform.localEulerAngles.y;
        angle4 = axe4.transform.localEulerAngles.z;
        angle5 = axe5.transform.localEulerAngles.y;
    }

    public void OnBBasTriggerEnter()
    {
        axe -= 1;
        if (axe < 0 && mode == Mode.COORDS) { axe = 2; }
        else if (axe < 0 && mode == Mode.AXES) { axe = 5; }
    }

    public void OnBHautTriggerEnter()
    {
        axe += 1;
        if ((axe > 5 && mode == Mode.AXES) || (axe > 2 && mode == Mode.COORDS)) { axe = 0; }
    }

    public void OnBModeTriggerEnter()
    {
        if (mode == Mode.COORDS)
        {
            angle0 = axe0.transform.localEulerAngles.y;
            angle1 = axe1.transform.localEulerAngles.z;
            angle2 = axe2.transform.localEulerAngles.z;
            angle3 = axe3.transform.localEulerAngles.y;
            angle4 = axe4.transform.localEulerAngles.z;
            angle5 = axe5.transform.localEulerAngles.y;

            mode = Mode.AXES;
            finalIK.GetComponent<CCDIK>().enabled = false;
        }
        else if (mode == Mode.AXES)
        {
            mode = Mode.AUTO;
            //cible.transform.position = offset*torche.transform.eulerAngles
            finalIK.GetComponent<CCDIK>().enabled = true;
        }
        else
        {
            posX = cible.transform.position.x;
            posY = cible.transform.position.y;
            posZ = cible.transform.position.z;

            mode = Mode.COORDS;
        }
        axe = 0;
    }

    public void OnBMoinsTriggerEnter()
    {
        switch (axe)
        {
            case 0:
                if (mode == Mode.COORDS) { posX -= pas_pos; }
                else if (mode == Mode.AXES) { angle0 -= pas_angle; }
                break;
            case 1:
                if (mode == Mode.COORDS) { posY -= pas_pos; }
                else if (mode == Mode.AXES) { angle1 -= pas_angle; }
                break;
            case 2:
                if (mode == Mode.COORDS) { posZ -= pas_pos; }
                else if (mode == Mode.AXES) { angle2 -= pas_angle; }
                break;
            case 3:
                if (mode == Mode.AXES) { angle3 -= pas_angle; }
                break;
            case 4:
                if (mode == Mode.AXES) { angle4 -= pas_angle; }
                break;
            case 5:
                if (mode == Mode.AXES) { angle5 -= pas_angle; }
                break;
            default:
                break;
        }
    }

    public void OnBPlusTriggerEnter()
    {
        switch (axe)
                {
                    case 0:
                        if (mode == Mode.COORDS) { posX += pas_pos; }
                        else if (mode == Mode.AXES) { angle0 += pas_angle; }
                        break;
                    case 1:
                        if (mode == Mode.COORDS) { posY += pas_pos; }
                        else if (mode == Mode.AXES) { angle1 += pas_angle; }
                        break;
                    case 2:
                        if (mode == Mode.COORDS) { posZ += pas_pos; }
                        else if (mode == Mode.AXES) { angle2 += pas_angle; }
                        break;
                    case 3:
                        if (mode == Mode.AXES) { angle3 += pas_angle; }
                        break;
                    case 4:
                        if (mode == Mode.AXES) { angle4 += pas_angle; }
                        break;
                    case 5:
                        if (mode == Mode.AXES) { angle5 += pas_angle; }
                        break;
                    default:
                        break;
                }
    }

    public void OnBMoinsTriggerStay()
    {
        switch (axe)
        {
            case 0:
                if (mode == Mode.COORDS) { posX -= pas_pos_continu; }
                else if (mode == Mode.AXES) { angle0 -= pas_angle_continu; }
                break;
            case 1:
                if (mode == Mode.COORDS) { posY -= pas_pos_continu; }
                else if (mode == Mode.AXES) { angle1 -= pas_angle_continu; }
                break;
            case 2:
                if (mode == Mode.COORDS) { posZ -= pas_pos_continu; }
                else if (mode == Mode.AXES) { angle2 -= pas_angle_continu; }
                break;
            case 3:
                if (mode == Mode.AXES) { angle3 -= pas_angle_continu; }
                break;
            case 4:
                if (mode == Mode.AXES) { angle4 -= pas_angle_continu; }
                break;
            case 5:
                if (mode == Mode.AXES) { angle5 -= pas_angle_continu; }
                break;
            default:
                break;
        }
    }

    public void OnBPlusTriggerStay()
    {
        switch (axe)
                {
                    case 0:
                        if (mode == Mode.COORDS) { posX += pas_pos_continu; }
                        else if (mode == Mode.AXES) { angle0 += pas_angle_continu; }
                        break;
                    case 1:
                        if (mode == Mode.COORDS) { posY += pas_pos_continu; }
                        else if (mode == Mode.AXES) { angle1 += pas_angle_continu; }
                        break;
                    case 2:
                        if (mode == Mode.COORDS) { posZ += pas_pos_continu; }
                        else if (mode == Mode.AXES) { angle2 += pas_angle_continu; }
                        break;
                    case 3:
                        if (mode == Mode.AXES) { angle3 += pas_angle_continu; }
                        break;
                    case 4:
                        if (mode == Mode.AXES) { angle4 += pas_angle_continu; }
                        break;
                    case 5:
                        if (mode == Mode.AXES) { angle5 += pas_angle_continu; }
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
            cible.transform.position = new Vector3(posX, posY, posZ);

            posX = cible.transform.position.x;
            posY = cible.transform.position.y;
            posZ = cible.transform.position.z;
        }
        else if (mode == Mode.AXES)
        {
            axe0.transform.localEulerAngles = new Vector3(axe0.transform.localEulerAngles.x, angle0, axe0.transform.localEulerAngles.z);
            axe1.transform.localEulerAngles = new Vector3(axe1.transform.localEulerAngles.x, axe1.transform.localEulerAngles.y, angle1);
            axe2.transform.localEulerAngles = new Vector3(axe2.transform.localEulerAngles.x, axe2.transform.localEulerAngles.y, angle2);
            axe3.transform.localEulerAngles = new Vector3(axe3.transform.localEulerAngles.x, angle3, axe3.transform.localEulerAngles.z);
            axe4.transform.localEulerAngles = new Vector3(axe4.transform.localEulerAngles.x, axe4.transform.localEulerAngles.y, angle4);
            axe5.transform.localEulerAngles = new Vector3(axe5.transform.localEulerAngles.x, angle5, axe5.transform.localEulerAngles.z);

            angle0 = axe0.transform.localEulerAngles.y;
            angle1 = axe1.transform.localEulerAngles.z;
            angle2 = axe2.transform.localEulerAngles.z;
            angle3 = axe3.transform.localEulerAngles.y;
            angle4 = axe4.transform.localEulerAngles.z;
            angle5 = axe5.transform.localEulerAngles.y;
        }
    }
}
