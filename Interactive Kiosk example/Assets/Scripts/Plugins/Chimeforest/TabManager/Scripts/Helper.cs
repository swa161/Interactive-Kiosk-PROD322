using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chimeforest.TabManager
{
    public class Helper
    {
        //Centers and Stretches a RectTransform so that it fills the object is is in
        public static void CenterAndStrecthRectTransform(GameObject gameobject)
        {
            CenterAndStrecthRectTransform(gameobject.GetComponent<RectTransform>());
        }
        public static void CenterAndStrecthRectTransform(RectTransform rectTransf)
        {
            rectTransf.GetComponent<RectTransform>().anchorMin = new Vector2();
            rectTransf.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            rectTransf.GetComponent<RectTransform>().localPosition = new Vector3();
            rectTransf.GetComponent<RectTransform>().sizeDelta = new Vector2();
        }

        //Gets all the children gameobjects and returns them as a list
        public static List<GameObject> GetChildren(GameObject gameObj)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform tran in gameObj.transform)
            {
                children.Add(tran.gameObject);
                //Debug.Log("Object " + tran.gameObject.name + " added.");
            }
            return children;
        }
    }
}
