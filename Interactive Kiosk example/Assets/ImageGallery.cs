using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class ImageGallery : MonoBehaviour
{ 
    public Sprite[] gallery; //store all your images in here at design time
    public Image displayImage; //The current image thats visible
    public Button nextImg; //Button to view next image
    public Button prevImg; //Button to view previous image
    public int i = 0; //Will control where in the array you are

    public void BtnNext()
    {
        if (i > gallery.Length)
        {
            i = 0;
            print(i);
        }
        else if (i + 1 < gallery.Length)
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
        displayImage.sprite = gallery[i];
    }
}