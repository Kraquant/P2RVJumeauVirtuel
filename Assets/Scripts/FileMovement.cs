using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class FileMovement : MonoBehaviour
{
    public TextAsset trajectoryFile;

    private File trajectory;
    // Start is called before the first frame update
    void Start()
    {
        if (trajectoryFile != null)
        {
            trajectory = new File(trajectoryFile);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeFile(TextAsset newFile) //CodeMoi
    {
        //Threading...
    }
}
