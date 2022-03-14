using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boutons : MonoBehaviour
{
    public GameObject pendant;


    // Pour l'interaction 2D uniquement (utile au débug)
    // Déclenchement d'un appui avec la souris
    public void OnClick()
    {
        Collider mouse = new Collider();
        OnTriggerEnter(mouse);
    }

    // Appui prolongé avec la souris
    public void OnLongClick()
    {
        Collider mouse = new Collider();
        OnTriggerStay(mouse);
    }

    // Interactions 2D et 3D *********************************

    // Lorsqu'on appuie sur un bouton
    private void OnTriggerEnter(Collider other)
    {
        string bouton = this.tag;
        Debug.Log(bouton);
        // On appelle la fonction d'appui correspondante
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

    // Pendant un appuie prolongé sur un bouton
    private void OnTriggerStay(Collider other)
    {
        string bouton = this.tag;
        // On appelle la fonction d'appui prolongé correspondante
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

    // Quand on relâche un bouton
    private void OnTriggerRelease(Collider other)
    {
        // Si le bouton est "Avance Rapide", on appelle sa fonction de relâchement
        if (this.tag == "FF") { pendant.GetComponent<Pendant>().OnBFFTriggerStay(); }
    }
}
