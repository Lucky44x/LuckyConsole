using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lucky44.LuckyConsole;
using System.Reflection;
using System;
using System.Linq;

namespace Lucky44.ConsoleEditor
{
    public class EditorExtension : Editor
    {
        [MenuItem("GameObject/UI/LuckyConsole", false, 0)]
        static void InstantiatePrefab()
        {
            UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath("Packages/de.lucky44.lucky-console/Samples/SamplesStandart/LuckyConsole.prefab", typeof(GameObject));
            GameObject g = PrefabUtility.InstantiatePrefab(playerPrefab as GameObject) as GameObject;

            GameObject selected = Selection.activeObject as GameObject;

            if (selected != null)
            {
                g.transform.SetParent(selected.transform);
                g.transform.localScale = new Vector3(1, 1, 1);
            }

            PrefabUtility.UnpackPrefabInstance(g, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

    }
}
