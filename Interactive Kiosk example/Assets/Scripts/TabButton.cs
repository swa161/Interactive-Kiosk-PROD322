using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TabSystem.TabManager
{
    public class TabButton : MonoBehaviour
    {
        // Use this for initialization
        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            this.GetComponent<Tab>().TabManager().SwitchToTab(this.GetComponent<Tab>().GetTab());
        }
    }
}
