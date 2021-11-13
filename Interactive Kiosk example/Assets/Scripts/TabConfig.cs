using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TabSystem.TabManager
{
    //this class is used to store configuration data, all settings/configs should be moved here.
    class TabConfig
    {
        //TabContext settings
        public const bool context_ContainerStretchFill = true;

        //TabManager settings
        public const bool manager_ForeceRename = true;
        public const bool manager_SwitchToNewTab = false;

        //Prefab Locations
        //This plugin has a resources folder where the default prefabs are used, but it can grab them from any Resouces folder.
        //If you want to style your tabs simple make your own TabButton and TabPanel prefabs and change this config. Any new tabs will use your prefabs instead.
        public static string prefabPath_Button = "Prefabs/CF_TabButton";
        public static string prefabPath_Container = "Prefabs/CF_TabContainer";
        public static string prefabPath_Panel = "Prefabs/CF_TabPanel";
    }
}
