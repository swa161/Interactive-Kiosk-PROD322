using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonImage : MonoBehaviour
{
    public Sprite OffSprite;
    public Sprite OnSprite;
    public Button Btn;
    public void ChangeImage()
    {
        if (Btn.image.sprite == OnSprite)
            Btn.image.sprite = OffSprite;
        else
        {
            Btn.image.sprite = OnSprite;
        }
    }
}
