using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boutons : MonoBehaviour
{
    public int axe;
    public float pas_angle;
    public float pas_pos;
    public float pas_angle_continu;
    public float pas_pos_continu;
    private float posX;
    private float posY;
    private float posZ;
    private float angle0;
    private float angle1;
    private float angle2;
    private float angle3;
    private float angle4;
    private float angle5;
    private GameObject torche = GameObject.Find("Bras6");
    private GameObject axe0 = GameObject.Find("OsBras1");
    private GameObject axe1 = GameObject.Find("OsBras2");
    private GameObject axe2 = GameObject.Find("OsBras3");
    private GameObject axe3 = GameObject.Find("OsBras4");
    private GameObject axe4 = GameObject.Find("OsBras5");
    private GameObject axe5 = GameObject.Find(".......");

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
        mode = Mode.COORDS;
        axe = 0;
        pas_angle_continu = 0.5f;
        pas_pos_continu = 0.5f;
        pas_angle = 2;
        pas_pos = 2;
        
        posX = torche.transform.position.x;
        posY = torche.transform.position.y;
        posZ = torche.transform.position.z;
        angle0 = axe0.transform.eulerAngles.y;
        angle1 = axe1.transform.eulerAngles.z;
        angle2 = axe2.transform.eulerAngles.z;
        angle3 = axe3.transform.eulerAngles.y;
        angle4 = axe4.transform.eulerAngles.z;
        angle5 = axe5.transform.eulerAngles.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        string bouton = this.name; 
        switch (bouton)
        {
            case "Bouton bas":
                axe -= 1;
                if (axe < 0 && mode == Mode.COORDS) { axe = 2; }
                else if (axe < 0 && mode == Mode.AXES) { axe = 5; }
                break;
            case "Bouton haut":
                axe += 1;
                if ((axe > 5 && mode == Mode.AXES) || (axe > 2 && mode == Mode.COORDS)) { axe = 0; }
                break;
            case "Bouton mode":
                if (mode == Mode.COORDS) { mode = Mode.AXES; }
                else if (mode == Mode.AXES) { mode = Mode.AUTO; }
                else { mode = Mode.COORDS; }
                axe = 0;
                break;
            case "Bouton moins":
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
                break;
            case "Bouton plus":
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
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        string bouton = this.name;
        switch (bouton)
        {
            case "Bouton moins":
                switch (axe)
                {
                    case 0:
                        if (mode == Mode.COORDS) { posX -= pas_pos_continu; }
                        else if (mode == Mode.AXES) { angle0 += pas_angle_continu; }
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
                break;
            case "Bouton plus":
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
            torche.transform.position = new Vector3(posX, posY, posZ);

            posX = torche.transform.position.x;
            posY = torche.transform.position.y;
            posZ = torche.transform.position.z;
        }
        else if (mode == Mode.AXES)
        {
            axe0.transform.eulerAngles = new Vector3(axe0.transform.eulerAngles.x, angle0, axe0.transform.eulerAngles.z);
            axe1.transform.eulerAngles = new Vector3(axe1.transform.eulerAngles.x, axe1.transform.eulerAngles.y, angle1);
            axe2.transform.eulerAngles = new Vector3(axe2.transform.eulerAngles.x, axe2.transform.eulerAngles.y, angle2);
            axe3.transform.eulerAngles = new Vector3(axe3.transform.eulerAngles.x, angle3, axe3.transform.eulerAngles.z);
            axe4.transform.eulerAngles = new Vector3(axe4.transform.eulerAngles.x, axe4.transform.eulerAngles.y, angle4);
            axe5.transform.eulerAngles = new Vector3(axe5.transform.eulerAngles.x, angle5, axe5.transform.eulerAngles.z);

            angle0 = axe0.transform.eulerAngles.y;
            angle1 = axe1.transform.eulerAngles.z;
            angle2 = axe2.transform.eulerAngles.z;
            angle3 = axe3.transform.eulerAngles.y;
            angle4 = axe4.transform.eulerAngles.z;
            angle5 = axe5.transform.eulerAngles.y;
        }
    }
}
