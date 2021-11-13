using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TabSystem.TabManager
{
    public class TabContainer : MonoBehaviour
    {
        //TODO Add option to set button and panel prefabs
        public static GameObject CreateTabContainerWithObjects(GameObject prefab_Button = null, GameObject prefab_Panel = null)
        {
            //create the tabContainer from a prefab
            GameObject tabContainer = Instantiate<GameObject>(Resources.Load<GameObject>(TabConfig.prefabPath_Container));
            tabContainer.name = "TabContainer";

            //Set prefabs if supplied
            if (prefab_Button != null)
            {
                tabContainer.GetComponent<TabManager>().prefabButton = prefab_Button;
            }
            if (prefab_Panel != null)
            {
                tabContainer.GetComponent<TabManager>().prefabPanel = prefab_Panel;
            }

            //Add a new tab
            tabContainer.GetComponent<TabManager>().AddTabEnd();

            //Sets up the tabContainer for first time use.
            tabContainer.GetComponent<TabManager>().ReNumberTabs();

            return tabContainer;
        }

        public static GameObject CreateTabContainerWithResourcePaths(string prefabPath_Button = "", string prefabPath_Panel = "")
        {
            return CreateTabContainerWithObjects(Resources.Load<GameObject>(prefabPath_Button), Resources.Load<GameObject>(prefabPath_Panel));
        }

        public static GameObject CreateTabContainer()
        {
            return CreateTabContainerWithObjects();
        }

        public static GameObject CreateBLANKTabContainer()
        {
            //create the tabContainer from a prefab, without any tabs.
            GameObject tabContainer = Instantiate<GameObject>(Resources.Load<GameObject>(TabConfig.prefabPath_Container));
            tabContainer.name = "TabContainer";
            
            return tabContainer;
        }
    }
}
