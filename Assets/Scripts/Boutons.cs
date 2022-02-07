using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boutons : MonoBehaviour
{
    public bool mode_axes;
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
        mode_axes = false;
        axe = 0;
        pas_angle_continu = 0.5f;
        pas_pos_continu = 0.5f;
        pas_angle = 2;
        pas_pos = 2;
        
        posX = torche.transform.position.x;
        posY = torche.transform.position.y;
        posZ = torche.transform.position.z;
        angle0 = axe0.transform.rotation.y;
        angle1 = axe1.transform.rotation.z;
        angle2 = axe2.transform.rotation.z;
        angle3 = axe3.transform.rotation.y;
        angle4 = axe4.transform.rotation.z;
        angle5 = axe5.transform.rotation.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        string bouton = this.name; 
        switch (bouton)
        {
            case "Bouton bas":
                axe -= 1;
                if (axe < 0 && mode_axes) { axe = 5; }
                else if (axe < 0) { axe = 2; }
                break;
            case "Bouton haut":
                axe += 1;
                if ((axe > 5 && mode_axes) || (axe > 2 && !mode_axes)) { axe = 0; }
                break;
            case "Bouton mode":
                mode_axes = !mode_axes;
                axe = 0;
                break;
            case "Bouton moins":
                switch (axe)
                {
                    case 0:
                        if (mode_axes) { angle0 -= pas_angle; }
                        else { posX -= pas_pos; }
                        break;
                    case 1:
                        if (mode_axes) { angle1 -= pas_angle; }
                        else { posY -= pas_pos; }
                        break;
                    case 2:
                        if (mode_axes) { angle2 -= pas_angle; }
                        else { posZ -= pas_pos; }
                        break;
                    case 3:
                        angle3 -= pas_angle;
                        break;
                    case 4:
                        angle4 -= pas_angle;
                        break;
                    case 5:
                        angle5 -= pas_angle;
                        break;
                    default:
                        break;
                }
                break;
            case "Bouton plus":
                switch (axe)
                {
                    case 0:
                        if (mode_axes) { angle0 += pas_angle; }
                        else { posX += pas_pos; }
                        break;
                    case 1:
                        if (mode_axes) { angle1 += pas_angle; }
                        else { posY += pas_pos; }
                        break;
                    case 2:
                        if (mode_axes) { angle2 += pas_angle; }
                        else { posZ += pas_pos; }
                        break;
                    case 3:
                        angle3 += pas_angle;
                        break;
                    case 4:
                        angle4 += pas_angle;
                        break;
                    case 5:
                        angle5 += pas_angle;
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
                        if (mode_axes) { angle0 -= pas_angle_continu; }
                        else { posX -= pas_pos_continu; }
                        break;
                    case 1:
                        if (mode_axes) { angle1 -= pas_angle_continu; }
                        else { posY -= pas_pos_continu; }
                        break;
                    case 2:
                        if (mode_axes) { angle2 -= pas_angle_continu; }
                        else { posZ -= pas_pos_continu; }
                        break;
                    case 3:
                        angle3 -= pas_angle_continu;
                        break;
                    case 4:
                        angle4 -= pas_angle_continu;
                        break;
                    case 5:
                        angle5 -= pas_angle_continu;
                        break;
                    default:
                        break;
                }
                break;
            case "Bouton plus":
                switch (axe)
                {
                    case 0:
                        if (mode_axes) { angle0 += pas_angle_continu; }
                        else { posX += pas_pos_continu; }
                        break;
                    case 1:
                        if (mode_axes) { angle1 += pas_angle_continu; }
                        else { posY += pas_pos_continu; }
                        break;
                    case 2:
                        if (mode_axes) { angle2 += pas_angle_continu; }
                        else { posZ += pas_pos_continu; }
                        break;
                    case 3:
                        angle3 += pas_angle_continu;
                        break;
                    case 4:
                        angle4 += pas_angle_continu;
                        break;
                    case 5:
                        angle5 += pas_angle_continu;
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
        if (mode_axes)
        {
            axe0.transform.rotation.y = angle0;
            axe1.transform.rotation.z = angle1;
            axe2.transform.rotation.z = angle2;
            axe3.transform.rotation.y = angle3;
            axe4.transform.rotation.z = angle4;
            axe5.transform.rotation.y = angle5;

            angle0 = axe0.transform.rotation.y;
            angle1 = axe1.transform.rotation.z;
            angle2 = axe2.transform.rotation.z;
            angle3 = axe3.transform.rotation.y;
            angle4 = axe4.transform.rotation.z;
            angle5 = axe5.transform.rotation.y;
        }
        else
        {
            torche.transform.position.x = posX;
            torche.transform.position.y = posY;
            torche.transform.position.z = posZ;

            posX = torche.transform.position.x;
            posY = torche.transform.position.y;
            posZ = torche.transform.position.z;
        }
    }
}
