using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Chimeforest.TabManager
{

    [Serializable]
    public class TabManager : MonoBehaviour
    {
        const bool FORCE_RENAME = TabConfig.manager_ForeceRename;
        const bool SWITCH_TO_NEW_TAB = TabConfig.manager_SwitchToNewTab;

        public GameObject TabPanels;
        public GameObject TabButtons;
        public List<GameObject> panels;
        public List<GameObject> buttons;
        public int currentTab;

        public GameObject prefabButton;
        public GameObject prefabPanel;

        // Use this for initialization
        void Start()
        {
            ReNumberTabs();
        }

        //Fill the panels/buttons lists with all the panels and buttons.
        public void SetLists()
        {
            panels = Helper.GetChildren(TabPanels);
            buttons = Helper.GetChildren(TabButtons);
        }


        //Switch focus from one tab to another
        public void SwitchToTab(int tabNum, bool recordUndo = true)
        {
#if UNITY_EDITOR
            //record an undo if needed and if in the editor.
            if (recordUndo)
            {
                List<UnityEngine.Object> undoGOs = new List<UnityEngine.Object>();
                undoGOs.Add(this);       //record currentTab
                foreach (GameObject go in panels)   //record which panels are active
                {
                    undoGOs.Add(go);
                }
                Undo.RecordObjects(undoGOs.ToArray(), "Switch Tab");
            }
#endif
            //Debug.Log("Switching to tab " + tabNum);
            if (tabNum >= 0 || tabNum < panels.Count)
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    if (i == tabNum)
                    {
                        //Set the correct panel to be active and store the currentTab id
                        panels[i].SetActive(true);
                        currentTab = i;
                        //Debug.Log(i + " is NOW active.");
                    }
                    else
                    {
                        //All other tab panels are hidden.
                        panels[i].SetActive(false);
                        //Debug.Log(i + " is not active.");
                    }
                }
            }
            else
            {
                //give an error if out of bounds.
                Debug.Log(tabNum + " is not between 0 and " + panels.Count);
            }
        }


        //Deletes a single tab
        public void RemoveTab(int tabNum, bool forceRemoveLastTab = false)
        {
            //put tabNum within the correct bounds
            if (tabNum < 0) { tabNum = 0; }
            if (tabNum > panels.Count) { tabNum = panels.Count; }
            int tempNum = tabNum;// TODO is this line needed anymore?

#if UNITY_EDITOR
            List<UnityEngine.Object> undoGOs = new List<UnityEngine.Object>();
            undoGOs.Add(this);                  //record currentTab
            foreach (GameObject go in panels)   //record which panels are active
            {
                undoGOs.Add(go);
            }
            //Undo.IncrementCurrentGroup();
            //Undo.SetCurrentGroupName("Remove Tab");
            //int undoGroup = Undo.GetCurrentGroup();
            Undo.RecordObjects(undoGOs.ToArray(), "Remove Tab");
#endif
            
            //If a user tried to delete the last tab, don't let them. However a programmer, during gameplay, can.
            if (panels.Count == 1 && !forceRemoveLastTab)
            {
                Debug.Log("Can't remove last tab, delete tab container instead.");
            }
            else
            {
                //If in the editor, destory immediately, else, just destory.
                if (Application.isEditor)
                {
#if UNITY_EDITOR
                    Undo.DestroyObjectImmediate(buttons[tempNum]);
                    Undo.DestroyObjectImmediate(panels[tempNum]);
                    //Undo.CollapseUndoOperations(undoGroup);
#endif
                }
                else
                {
                    DestroyImmediate(buttons[tempNum]);
                    DestroyImmediate(panels[tempNum]);
                }
                ReNumberTabs();

                //If the deleted tab == the current tab, switch to another tab
                if (tempNum == currentTab)
                {
                    //if the deleted tab was the last tab, switch to the new last tab
                    if (tempNum != panels.Count)
                    {
                        SwitchToTab(tempNum,false);
#if UNITY_EDITOR
                        Selection.activeObject = panels[tempNum];
#endif
                    }
                    //if there are no tabs, do not switch.
                    else if (panels.Count == 0) { }
                    //else, switch 
                    else
                    {
                        SwitchToTab(panels.Count - 1,false);
#if UNITY_EDITOR
                        Selection.activeObject = panels[panels.Count - 1];
#endif
                    }
                }
            }
        }
        // Removes all Tabs
        public void RemoveAllTabs()
        {
            for(int i = panels.Count -1; i >= 0; i--)
            {
                RemoveTab(i, true);
            }
        }


        //Adds a tab at the specified index, optionally using GAMEOBJECTS as prefabs for panel and button
        public void InsertTabWithObjects(int tabNum, string btnTxt = "TabButton", GameObject prefab_Button = null, GameObject prefab_Panel = null, bool switchToNewTab = SWITCH_TO_NEW_TAB)
        {
#if UNITY_EDITOR
            //start the undo records
            List<UnityEngine.Object> undoGOs = new List<UnityEngine.Object>();
            undoGOs.Add(this);                  //record currentTab
            foreach (GameObject go in panels)   //record which panels are active
            {
                undoGOs.Add(go);
            }
            //Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Add Tab");
            int undoGroup = Undo.GetCurrentGroup();
            Undo.RecordObjects(undoGOs.ToArray(), "Remove Tab");
#endif

            //put the tab num within the correct bounds
            if (tabNum < 0) { tabNum = 0; }
            if (tabNum > panels.Count) { tabNum = panels.Count; }

            //create button and panel from prefabs
            GameObject tabBtn;
            GameObject tabPan;

            //if the path to the button prefab is not empty, use it to make a new button
            if (prefab_Button != null)
            {
                tabBtn = Instantiate<GameObject>(prefab_Button);
            }
            //else, if the prefab is not empty, use it
            else if (prefabButton != null)
            {
                tabBtn = Instantiate<GameObject>(prefabButton);
            }
            //else use the default
            else
            {
                tabBtn = Instantiate<GameObject>(Resources.Load<GameObject>(TabConfig.prefabPath_Button));
            }

            //if the path to the panel prefab is not empty, use it to make a new button
            if (prefab_Panel != null)
            {
                tabPan = Instantiate<GameObject>(prefab_Panel);
            }
            //else, if the prefab is not empty, use it
            else if (prefabButton != null)
            {
                tabPan = Instantiate<GameObject>(prefabPanel);
            }
            //else use the default
            else
            {
                tabPan = Instantiate<GameObject>(Resources.Load<GameObject>(TabConfig.prefabPath_Panel));
            }

            //Set Parents
            tabBtn.transform.SetParent(this.transform.Find("TabButtons"));
            tabPan.transform.SetParent(this.transform.Find("TabPanels"));

            //Correct for Screen Space Canvas
            tabBtn.GetComponent<RectTransform>().localRotation = new Quaternion();
            tabBtn.GetComponent<RectTransform>().localPosition = new Vector3(tabBtn.GetComponent<RectTransform>().localPosition.x, tabBtn.GetComponent<RectTransform>().localPosition.y,0);
            tabPan.GetComponent<RectTransform>().localRotation = new Quaternion();
            tabPan.GetComponent<RectTransform>().localPosition = new Vector3(tabPan.GetComponent<RectTransform>().localPosition.x, tabPan.GetComponent<RectTransform>().localPosition.y, 0);
            
            //change button text, or delete if no text
            if (btnTxt != "" && btnTxt != null)
            {
                tabBtn.transform.GetChild(0).GetComponent<Text>().text = btnTxt;
            }
            else
            {
                Destroy(tabBtn.transform.GetChild(0));
            }

            //set scale
            tabBtn.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            tabPan.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            if (tabBtn.transform.GetChild(0) != null)
            {
                tabBtn.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }

            //put tab compenents at the right index
            tabBtn.transform.SetSiblingIndex(tabNum);
            tabPan.transform.SetSiblingIndex(tabNum);

            //renumber the tabs to register the new one.
            ReNumberTabs();

#if UNITY_EDITOR
            //finish up adding undo
            Undo.RegisterCreatedObjectUndo(tabBtn, "Add Tab");
            Undo.RegisterCreatedObjectUndo(tabPan, "Add Tab");
            Undo.CollapseUndoOperations(undoGroup);
#endif
            if (switchToNewTab)
            {
                SwitchToTab(tabNum);
#if UNITY_EDITOR
                Selection.activeObject = tabPan;

#endif
            }
            else
            {
                SwitchToTab(currentTab);
            }
        }
        
        //Adds a tab at the specified index, optionally using STRINGS as prefabs for panel and button
        public void InsertTabWithResoucePaths(int tabNum, string btnTxt = "TabButton", string prefabPath_Button = "", string prefabPath_Panel = "", bool switchToNewTab = SWITCH_TO_NEW_TAB)
        {
            InsertTabWithObjects(tabNum, btnTxt, Resources.Load<GameObject>(prefabPath_Button), Resources.Load<GameObject>(prefabPath_Panel), switchToNewTab);
        }

        //Adds a tab at the specified index, no prefabs
        public void InsertTab(int tabNum, string btnTxt = "TabButton")
        {
            InsertTabWithObjects(tabNum,  btnTxt);
        }

        //Moves a tab up in the list
        public void MoveTabUp(int tabNum)
        {
            //Debug.Log(panels[tabNum].GetComponentsInParent<GameObject>().Length);
            //Debug.Log(tabNum);
            if (tabNum > 0)
            {
#if UNITY_EDITOR
                List<UnityEngine.Object> undoGOs = new List<UnityEngine.Object>();
                undoGOs.Add(this);                  //record currentTab
                foreach (GameObject go in panels)   //record which panels are active
                {
                    undoGOs.Add(go);
                    undoGOs.Add(go.transform);
                    undoGOs.Add(go.GetComponent<Tab>());
                }
                Undo.RecordObjects(undoGOs.ToArray(), "Move Tab Up");
                //BUG this doesn't undo correctly
#endif
                panels[tabNum].transform.SetSiblingIndex(tabNum - 1);
                buttons[tabNum].transform.SetSiblingIndex(tabNum - 1);
                ReNumberTabs();
                SwitchToTab(tabNum - 1);
            }
            else
            {
                Debug.Log("Can't move tab further in that direction.");
            }
        }
        
        //Moves a tab down in the list
        public void MoveTabDown(int tabNum)
        {
            if (tabNum < panels.Count - 1)
            {
#if UNITY_EDITOR
                List<UnityEngine.Object> undoGOs = new List<UnityEngine.Object>();
                undoGOs.Add(this);                  //record currentTab
                foreach (GameObject go in buttons)   //record which buttons are affected
                {
                    undoGOs.Add(go);
                    undoGOs.Add(go.transform);
                    undoGOs.Add(go.GetComponent<Tab>());
                }
                foreach (GameObject go in panels)   //record which panels are active
                {
                    undoGOs.Add(go);
                    undoGOs.Add(go.transform);
                    undoGOs.Add(go.GetComponent<Tab>());
                }
                Undo.RecordObjects(undoGOs.ToArray(), "Move Tab Down");
                //BUG this doesn't undo correctly
#endif
                panels[tabNum].transform.SetSiblingIndex(tabNum + 1);
                buttons[tabNum].transform.SetSiblingIndex(tabNum + 1);
                ReNumberTabs();
                SwitchToTab(tabNum + 1);
            }
            else
            {
                Debug.Log("Can't move tab further in that direction.");
            }
        }

        //Some functions for adding and removing tabs
        public void AddTabBegin(string btnTxt = "TabButton")
        {
            InsertTab(0, btnTxt);
        }
        public void AddTabEnd(string btnTxt = "TabButton")
        {
            InsertTab(panels.Count, btnTxt);
        }
        public void AddTabBeginbWithResoucePaths(string btnTxt = "TabButton", string resource_ButtonPath = "", string resouce_PanelPath = "")
        {
            InsertTabWithResoucePaths(0, btnTxt, resource_ButtonPath, resouce_PanelPath);
        }
        public void AddTabEndbWithResoucePaths(string btnTxt = "TabButton", string resource_ButtonPath = "", string resouce_PanelPath = "")
        {
            InsertTabWithResoucePaths(panels.Count, btnTxt, resource_ButtonPath, resouce_PanelPath);
        }
        public void AddTabBeginbWithObjects(string btnTxt = "TabButton", GameObject resource_ButtonPrefab = null, GameObject resouce_PanelPrefab = null)
        {
            InsertTabWithObjects(0, btnTxt, resource_ButtonPrefab, resouce_PanelPrefab);
        }
        public void AddTabEndWithObjects(string btnTxt = "TabButton", GameObject resource_ButtonPrefab = null, GameObject resouce_PanelPrefab = null)
        {
            InsertTabWithObjects(panels.Count, btnTxt, resource_ButtonPrefab, resouce_PanelPrefab);
        }
        public void RemoveTabBegin()
        {
            RemoveTab(0);
        }
        public void RemoveTabEnd()
        {
            RemoveTab(panels.Count-1);
        }

        
        //Change the texture of buttons and panels, first checks for prefabPath, then prefab Object, then uses default. Just like InsertTab.
        public void ReskinButton(int tabNum, string prefabPath_Button = "")
        {
            //if the path to the button prefab is not empty, use it to make a new button
            if (prefabPath_Button != "" && prefabPath_Button != null)
            {
                ReskinButton(tabNum, Resources.Load<GameObject>(prefabPath_Button));
            }
            //else, if the prefab is not empty, use it
            else if (prefabButton != null)
            {
                ReskinButton(tabNum, prefabButton);
            }
        }
        public void ReskinButton(int tabNum, GameObject prefab_Button)
        {
            //check that prefab is not null
            if (prefab_Button != null)
            {
                //replace the image on the button
                this.buttons[tabNum].ReplaceComponent<Image>(prefab_Button.GetComponent<Image>());
                this.buttons[tabNum].GetComponent<Button>().targetGraphic = this.buttons[tabNum].GetComponent<Image>();

                //Check that the prefab and the tab both have text before trying to reskin the text.
                if (this.buttons[tabNum].GetComponentInChildren<Text>() != null && prefab_Button.GetComponentInChildren<Text>() != null)
                {
                    //Replace the Text of the button, but still keep the words. This allows changing of font,size, etc.
                    string text = this.buttons[tabNum].GetComponentInChildren<Text>().text;
                    Color color = prefab_Button.GetComponentInChildren<Text>().color;
                    this.buttons[tabNum].transform.Find("Text").gameObject.ReplaceComponent<Text>(prefab_Button.transform.Find("Text").GetComponent<Text>());
                    this.buttons[tabNum].GetComponentInChildren<Text>().text = text;
                    this.buttons[tabNum].GetComponentInChildren<Text>().color = color;
                }
            }
        }
        public void ReskinPanel(int tabNum, string prefabPath_Panel = "")
        {
            //if the path to the button prefab is not empty, use it to make a new button
            if (prefabPath_Panel != "" && prefabPath_Panel != null)
            {
                ReskinPanel(tabNum, Resources.Load<GameObject>(prefabPath_Panel));
            }
            //else, if the prefab is not empty, use it
            else if (prefabButton != null)
            {
                ReskinPanel(tabNum, prefabButton);
            }
        }
        public void ReskinPanel(int tabNum, GameObject prefab_Panel)
        {
            //check that prefab is not null
            if (prefab_Panel != null)
            {
                //replace the image on the panel
                this.panels[tabNum].ReplaceComponent<Image>(prefab_Panel.GetComponent<Image>());
            }
        }
        public void ReskinTab(int tabNum, string prefabPath_Button = "", string prefabPath_Panel = "")
        {
            ReskinButton(tabNum, prefabPath_Button);
            ReskinPanel(tabNum, prefabPath_Panel);
        }
        public void ReskinTab(int tabNum, GameObject prefab_Button = null, GameObject prefab_Panel = null)
        {
            ReskinButton(tabNum, prefab_Button);
            ReskinPanel(tabNum, prefab_Panel);
        }
        public void ReskinAllTabs(string prefabPath_Button = "", string prefabPath_Panel = "")
        {
            for (int i = 0; i <= this.panels.Count-1; i++)
            {
                ReskinTab(i, prefabPath_Button, prefabPath_Panel);
            }
        }
        public void ReskinAllTabs(GameObject prefab_Button = null, GameObject prefab_Panel = null)
        {
            for (int i = 0; i <= this.panels.Count - 1; i++)
            {
                //Debug.Log("Reskinning Tab: " + i);
                ReskinTab(i, prefab_Button, prefab_Panel);
                //Debug.Log("Tab Reskined: " + i);
            }
        }
        public void ReskinAllPanels(string prefabPath_Panel = "")
        {
            for (int i = 0; i <= this.panels.Count - 1; i++)
            {
                ReskinPanel(i, prefabPath_Panel);
            }
        }
        public void ReskinAllPanels(GameObject prefab_Panel = null)
        {
            for (int i = 0; i <= this.panels.Count - 1; i++)
            {
                ReskinPanel(i, prefab_Panel);
            }
        }
        public void ReskinAllButtons(string prefabPath_Button = "")
        {
            for (int i = 0; i <= this.buttons.Count - 1; i++)
            {
                ReskinButton(i, prefabPath_Button);
            }
        }
        public void ReskinAllButtons(GameObject prefab_Button = null)
        {
            for (int i = 0; i <= this.buttons.Count - 1; i++)
            {
                ReskinButton(i, prefab_Button);
            }
        }

        //Set Tab.tab to the correct number for each button and panel
        public void ReNumberTabs()
        {
            SetLists();
            //Debug.Log("Renumbering Tabs");
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].GetComponent<Tab>().SetTab(i);
                buttons[i].GetComponent<Tab>().SetTab(i);
                if (FORCE_RENAME)
                {
                    panels[i].name = "Panel" + i;
                    buttons[i].name = "Button" + i;
                }
            }
        }
        
        //returns how many panels are active. In the editor, onInspectorGUI uses this class to check if something when screwy with an undo.
        public int CountActivePanels()
        {
            int cnt = 0;

            foreach (GameObject go in panels)
            {
                if (go.activeInHierarchy) { cnt++; }
            }

            return cnt;
        }

        public bool TabPanelIndexesMatch()
        {
            bool value = true;
            foreach (GameObject go in panels)
            {
                if (go.transform.GetSiblingIndex() != go.GetComponent<Tab>().GetTab())
                {
                    value = false;
                    break;
                }
            }

            return value;
        }

        public void SetTabsIndexViaPanels()
        {
            for(int i = 0; i < panels.Count; i++)
            {
                panels[i].transform.SetSiblingIndex(panels[i].GetComponent<Tab>().GetTab());
                buttons[i].transform.SetSiblingIndex(panels[i].GetComponent<Tab>().GetTab());
            }
            //ReNumberTabs();
        }
        
        
        
    }
}
