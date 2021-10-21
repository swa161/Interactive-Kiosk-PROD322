using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimeforest.TabManager
{
    public static class ComponentExtention
    {
        //Replaces the component with another one
        public static T ReplaceComponent<T>(this GameObject go, T toReplace) where T : Component
        {
            if(go.GetComponent(toReplace.GetType()) != null)
            {
                if (Application.isEditor)
                {
#if UNITY_EDITOR
                    Undo.DestroyObjectImmediate(go.GetComponent(toReplace.GetType()));
#endif
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(go.GetComponent(toReplace.GetType()));
                }
            }

#if UNITY_EDITOR
            if (Application.isEditor)
            {
                return Undo.AddComponent(go, toReplace.GetType()).GetCopyOf(toReplace);
            }
#endif
            return go.AddComponent<T>().GetCopyOf(toReplace) as T;
        }

        //Adds a copy of a component to an object
        //Code for this function is from http://answers.unity3d.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }

        //Code for this function is from http://answers.unity3d.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }
    }
}
