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
    }

    public void OnLongClick()
    {
        Collider mouse = new Collider();
        OnTriggerStay(mouse);
    }

    // Interactions 2D et 3D *********************************

    private void OnTriggerEnter(Collider other)
    {
        string bouton = this.tag;
        Debug.Log(bouton);
        switch (bouton)
        {
            case "Up":
                pendant.GetComponent<Pendant>().OnBUpTriggerEnter();
                break;
            case "Down":
                pendant.GetComponent<Pendant>().OnBDownTriggerEnter();
                break;
            case "Mode":
                pendant.GetComponent<Pendant>().OnBModeTriggerEnter();
                break;
            case "Plus":
                pendant.GetComponent<Pendant>().OnBPlusTriggerEnter();
                break;
            case "Minus":
                pendant.GetComponent<Pendant>().OnBMinusTriggerEnter();
                break;
            case "Play":
                pendant.GetComponent<Pendant>().OnBPlayTriggerEnter();
                break;
            case "FF":
                pendant.GetComponent<Pendant>().OnBFFTriggerEnter();
                break;
            case "STOP":
                pendant.GetComponent<Pendant>().OnBSTOPTriggerEnter();
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        string bouton = this.tag;
        switch (bouton)
        {
            case "Plus":
                pendant.GetComponent<Pendant>().OnBPlusTriggerStay();
                break;
            case "Minus":
                pendant.GetComponent<Pendant>().OnBMinusTriggerStay();
                break;
            case "FF":
                pendant.GetComponent<Pendant>().OnBFFTriggerStay();
                break;
        }
    }
}
