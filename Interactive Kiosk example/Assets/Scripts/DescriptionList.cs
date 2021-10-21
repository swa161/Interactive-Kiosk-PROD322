using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionList : MonoBehaviour
{
    public Text displayText; //The current text thats visible
    [TextArea(3,10)]
    public string[] description; //store all your the description in here at design time
    public Button prevBtn; // Button to play the next description
    public Button NextBtn; // Button to play the previous description
    public int i = 0; //Will control where in the array you are
    public void BtnNext()
    {
        if (i > description.Length)
        {
            i = 0;
            print(i);
        }
        else if (i + 1 < description.Length)
        {
            i++;
            print(i);
        }
    }

    public void BtnPrev()
    {


        if (i - 1 > 0)
        {
            i--;
            print(i);
        }
        else
        {
            i = 0;
            print(i);
        }
    }

    void Update()
    {
        displayText.text = description[i];
    }
}
