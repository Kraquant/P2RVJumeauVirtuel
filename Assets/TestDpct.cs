using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDpct : MonoBehaviour
{
    public GameObject Boule;
    public GameObject Bras6;
    public GameObject Bras5;
    Vector3 Vector;
    Vector3 Bras6Len;

    // Start is called before the first frame update
    void Start()
    {
        Bras6Len = Bras6.transform.position - Bras5.transform.position;
        Debug.Log(Bras6Len.x);
    }

    // Update is called once per frame
    void Update()
    {
        Vector = Boule.transform.position;
        Debug.Log(Bras6Len.x);
        Debug.Log(Vector.x);
        this.transform.position = Vector - Bras6Len;
        Bras6.transform.position = Vector;
        Bras6.transform.LookAt(Bras5.transform);
        Debug.Log(this.transform.position.x);
    }
}
