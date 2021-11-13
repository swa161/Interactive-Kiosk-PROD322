using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
//using UnityEditor.UI;
#endif

namespace TabSystem.TabManager
{
#if UNITY_EDITOR
    public class TabContext : Editor
    {

        const bool CONTAINER_STRETCHFILL = TabConfig.context_ContainerStretchFill;

#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Tab Container", false, 0)]
        static void AddTabContainerToMenu(MenuCommand menuCommand)
        {
            // Create the tabContainer, this object manages all the tabs, panels, and buttons for this TabContainer
            GameObject tabContainer = TabContainer.CreateTabContainer();

            //check for canvas, if none exsists, create one and add tabcontainer to it.
            if (FindObjectOfType<Canvas>() == null)
            {
                GameObject can = new GameObject("Canvas");
                can.AddComponent<RectTransform>();
                can.AddComponent<Canvas>();
                can.AddComponent<CanvasScaler>();
                can.AddComponent<GraphicRaycaster>();
                can.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

                tabContainer.transform.SetParent(can.transform);
                tabContainer.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                Undo.RegisterCreatedObjectUndo(can, "Create Canvas");
            }
            //check for eventsystem, if none exsists, create one.
            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject EvnSys = new GameObject("EventSystem");
                EvnSys.AddComponent<EventSystem>();
                EvnSys.AddComponent<StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(EvnSys, "Create EventSystem");
            }

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(tabContainer, menuCommand.context as GameObject);

            //IF selected, have tab container fill whatever parent it was placed in, else it remains the default of 100x100.
            if (CONTAINER_STRETCHFILL)
            {
                Helper.CenterAndStrecthRectTransform(tabContainer);
            }

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(tabContainer, "Create " + tabContainer.name);
            Selection.activeObject = tabContainer;
        }

        [MenuItem("GameObject/UI/Tab", false, 0)]
        static void AddTabToMenu(MenuCommand menuCommand)
        {
            GameObject go = menuCommand.context as GameObject;
            //tabs can only be added to tabContainers or parts thereof
            if (go as GameObject != null)
            {
                if (go.GetComponent<TabUtilMenu>() != null)
                {
                    //they clicked on a tabContainer, tabButton, or tabPanels gameObject
                    go.GetComponent<TabUtilMenu>().TabManager.AddTabEnd();
                }
                else if (go.GetComponent<Tab>() != null)
                {
                    //they clicked on a Tab
                    go.GetComponent<Tab>().TabManager().AddTabEnd();
                }
                else
                {
                    Debug.Log("Tabs can only be added to a TabContainer");
                }
            }
            else
            {
                Debug.Log("Tabs can only be added to a TabContainer");
            }
        }
#endif
    }
#endif
}

