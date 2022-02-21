using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boutons : MonoBehaviour
{
    public GameObject pendant;


    // Pour l'interaction 2D uniquement **********************************
    public void OnClick()
    {
        Collider mouse = new Collider();
        OnTriggerEnter(mouse);
        //int loop = 0;
        //while (!Input.GetMouseButtonUp(0) && loop < 1000) { Debug.Log(loop);  OnTriggerStay(mouse); loop += 1; }
    }

    public void OnLongClick()
    {
        Collider mouse = new Collider();
        OnTriggerStay(mouse);
    }

    // Interactions 2D et 3D *********************************

    private void OnTriggerEnter(Collider other)
    {
        string bouton = this.name;
        Debug.Log(bouton);
        switch (bouton)
        {
            case "Bouton haut":
                pendant.GetComponent<Pendant>().OnBHautTriggerEnter();
                break;
            case "Bouton bas":
                pendant.GetComponent<Pendant>().OnBBasTriggerEnter();
                break;
            case "Bouton mode":
                pendant.GetComponent<Pendant>().OnBModeTriggerEnter();
                break;
            case "Bouton plus":
                pendant.GetComponent<Pendant>().OnBPlusTriggerEnter();
                break;
            case "Bouton moins":
                pendant.GetComponent<Pendant>().OnBMoinsTriggerEnter();
                break;
            case "Bouton play":
                pendant.GetComponent<Pendant>().OnBPlayTriggerEnter();
                break;
            case "Bouton fforward":
                pendant.GetComponent<Pendant>().OnBFForwardTriggerEnter();
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        string bouton = this.name;
        switch (bouton)
        {
            case "Bouton plus":
                pendant.GetComponent<Pendant>().OnBPlusTriggerStay();
                break;
            case "Bouton moins":
                pendant.GetComponent<Pendant>().OnBMoinsTriggerStay();
                break;
            case "Bouton fforward":
                pendant.GetComponent<Pendant>().OnBFForwardTriggerStay();
                break;
        }
    }
}
