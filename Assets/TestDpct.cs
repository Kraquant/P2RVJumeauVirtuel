using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDpct : MonoBehaviour
{
    public GameObject Boule;
    public GameObject Bras6;
    public GameObject Bras5;
    public GameObject Bras5Arm;
    Vector3 Vector;
    //Quaternion Rotate;
    Vector3 Bras6Len;
    public int X = 0;
    public int Y = 0;
    public int Z = 0;

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
        //Bras6.transform.position = Vector;
        //Bras6.transform.LookAt(Bras5.transform);
        //Bras6.transform.Rotate(new Vector3(90, 0, 0));
        //Bras5Arm.transform.LookAt(Bras6.transform);
        //Bras5Arm.transform.Rotate(new Vector3(X, Y, Z));
        Debug.Log(this.transform.position.x);
    }
}
