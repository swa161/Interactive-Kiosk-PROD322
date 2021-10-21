using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Chimeforest.TabManager
{
#if UNITY_EDITOR
    [CustomEditor(typeof(TabManager))]
    public class TabManInspector : Editor
    {
        //TabManager[] allController;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TabManager tabMan = (TabManager)target;

            if (GUILayout.Button("Reskin Tabs"))
            {
                List<UnityEngine.Object> undos = new List<UnityEngine.Object>();
                foreach (GameObject go in tabMan.panels)
                {
                    undos.Add(go.GetComponent<Image>());
                }
                foreach (GameObject go in tabMan.buttons)
                {
                    undos.Add(go.GetComponent<Image>());
                    undos.Add(go.transform.Find("Text").gameObject.GetComponent<Text>());
                }
                
                int undoGroup = Undo.GetCurrentGroup();

                tabMan.ReskinAllTabs(tabMan.prefabButton, tabMan.prefabPanel);

                Undo.SetCurrentGroupName("Reskin Tabs");
                Undo.CollapseUndoOperations(undoGroup);
            }
            GUILayout.Label("This button changes the images and text \nstyling of the tab buttons and panels to \nmatch the above prefabs.");
        }
    }

    [CustomEditor(typeof(TabUtilMenu))]
    public class TabUtilInspector : Editor
    {
        //TabManager[] allController;

        public override void OnInspectorGUI()
        {
            TabManager[] allControllers;
            allControllers = SceneAsset.FindObjectsOfType<TabManager>();
            foreach (TabManager tm in allControllers)
            {
                //if more than one tab is active, or the number of children doesn't match up, fix it.
                if (tm.CountActivePanels() != 1 || Helper.GetChildren(tm.TabPanels).Count != tm.panels.Count)
                {
                    tm.ReNumberTabs();
                    tm.SwitchToTab(tm.currentTab, false);
                }
                //catches undo Move
                if (tm.TabPanelIndexesMatch() == false)
                {
                    tm.SetTabsIndexViaPanels();
                    tm.ReNumberTabs();
                    tm.SwitchToTab(tm.currentTab, false);
                }
            }

            base.OnInspectorGUI();
            TabUtilMenu tabum = (TabUtilMenu)target;

            //buttons for adding a tab at the beginning or end of the tabs
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Beginning Tab", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tabum.TabManager.AddTabBegin();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Tab at End", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tabum.TabManager.AddTabEnd();
            }
            GUILayout.EndHorizontal();

            //buttons for removing a tab at the beginning or end of the tabs
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove First Tab", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tabum.TabManager.RemoveTabBegin();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove Last Tab", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tabum.TabManager.RemoveTabEnd();
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Use these buttons to add and remove tabs.\nDo NOT do it manually!\nClick on an individual tab for more options.");

            //give the user the option to renumber the tabs
            if (GUILayout.Button("Force Renumber and Switch"))
            {
                tabum.TabManager.ReNumberTabs();
                tabum.TabManager.SwitchToTab(tabum.TabManager.currentTab, false);
            }
            GUILayout.Label("Use Force Renumber if something \ngoes wonky, like an object's name not \nmatching it's tab number.");
        }
    }

    [CustomEditor(typeof(Tab))]
    public class TabInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            Tab tab = (Tab)target;
            TabManager[] allControllers;
            allControllers = SceneAsset.FindObjectsOfType<TabManager>();
            foreach (TabManager tm in allControllers)
            {
                //if more than one tab is active, or the number of children doesn't match up, fix it.
                if (tm.CountActivePanels() != 1 || Helper.GetChildren(tm.TabPanels).Count != tm.panels.Count)
                {
                    tm.ReNumberTabs();
                    tm.SwitchToTab(tm.currentTab, false);
                }
                //catches undo Move
                if (tm.TabPanelIndexesMatch() == false)
                {
                    tm.SetTabsIndexViaPanels();
                    tm.ReNumberTabs();
                    tm.SwitchToTab(tm.currentTab, false);
                }
            }
            
            base.OnInspectorGUI();
            //button for selecting the current tab
            if (GUILayout.Button("Select Tab"))
            {
                tab.TabManager().SwitchToTab(tab.GetComponent<Tab>().GetTab());
            }

            //button for removing the current tab
            if (GUILayout.Button("Remove Tab"))
            {
                tab.TabManager().RemoveTab(tab.GetComponent<Tab>().GetTab());
            }

            //buttons for adding a tab before or after the current one
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Insert Tab Before", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tab.TabManager().InsertTab(tab.GetTab());
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Insert Tab After", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tab.TabManager().InsertTab(tab.GetTab()+1);
            }
            GUILayout.EndHorizontal();

            //buttons for moving a tab up or down in the list
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Move Tab Up", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tab.TabManager().MoveTabUp(tab.GetTab());
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Move Tab Down", GUILayout.Width(Screen.width / 2 - 11)))
            {
                tab.TabManager().MoveTabDown(tab.GetTab());
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Use these buttons to add, remove, and move tabs.\nDo NOT do it manually");
        }
    }
#endif
}