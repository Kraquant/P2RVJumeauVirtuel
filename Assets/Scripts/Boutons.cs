using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boutons : MonoBehaviour
{
    public GameObject pendant;

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
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
