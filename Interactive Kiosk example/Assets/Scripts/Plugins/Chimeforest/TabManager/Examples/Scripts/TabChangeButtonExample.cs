using Chimeforest.TabManager;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TabChangeButtonExample : MonoBehaviour {
    //Assign these in the editor, or program them from another script, or edit this script with the images you want.
    public Sprite tabImageSelected;
    public Sprite tabImageDeSelected;
	
	// Update is called once per frame
	void Update ()
    {
        if (this.GetComponent<Tab>().IsSelected())
        {
            this.GetComponent<Image>().sprite = tabImageSelected;
        }
        else
        {
            this.GetComponent<Image>().sprite = tabImageDeSelected;
        }
	}
}
