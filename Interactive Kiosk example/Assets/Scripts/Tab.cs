using System;
using System.Collections;
using UnityEngine;

namespace TabSystem.TabManager
{
    [Serializable]
    public class Tab : MonoBehaviour
    {
        //This class is used as a hook for an inspector menu
        

        //This variable is used to get and record the current tab, this is done automatically.
        //Do not use "SetTab" while programming, it could mess up the tab system.
        [SerializeField]
        int tab;

        public int GetTab() { return tab; }
        public void SetTab(int tabIndex) { tab = tabIndex; }

        public bool IsSelected()
        {
            return this.transform.GetComponentInParent<TabUtilMenu>().TabManager.currentTab == tab;
        }

        public TabManager TabManager()
        {
            return this.transform.GetComponentInParent<TabUtilMenu>().TabManager;
        }
    }
}
