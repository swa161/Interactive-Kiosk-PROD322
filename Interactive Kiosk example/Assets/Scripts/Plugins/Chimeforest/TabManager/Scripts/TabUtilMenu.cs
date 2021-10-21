using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

//namespace chimeforest.TabManager
//{

//This class is placed on the TabContainer, as well as the TabButtons and TabPanels objects.
//It's main purpse is for the TabUtilMenu Inspector
namespace Chimeforest.TabManager
{
    class TabUtilMenu : MonoBehaviour
    {
        //lets you check what type of object this script is attached to, assigned at creation, options are tabContainer, tabButtons, or tabPanels
        public string type = "tabContainer";
        //A reference to the TabManager Script which controls this script and it's tabs, assigned at creation
        public TabManager TabManager = null;
    }
}
//}
