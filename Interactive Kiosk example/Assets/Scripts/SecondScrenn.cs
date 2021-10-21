using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondScrenn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Display.displays[1].Activate(1280,1024, 60);
        Display.displays[2].Activate(1920,1200,60);
       
    }

   
}
