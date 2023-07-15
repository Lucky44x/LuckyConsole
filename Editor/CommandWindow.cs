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
    public class CommandWindow : EditorWindow
    {
        CommandManager cM = new CommandManager();
        List<Command> commandMethods = new List<Command>();

        string[] arguments = new string[0];

        [MenuItem("Window/LuckyConsole-TestWindow")]
        public static void showWindow()
        {
            EditorWindow.GetWindow(typeof(CommandWindow));
        }

        public void OnEnable()
        {
            loadMethods();
        }

        private void loadMethods()
        {
            commandMethods.Clear();
            commandMethods.AddRange(cM.subscribeMethods());
            arguments = new string[commandMethods.Count];
        }

        public void OnGUI()
        {
            if(GUILayout.Button("Load commands"))
            {
                loadMethods();
            }

            foreach(Command command in commandMethods)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(command.name))
                {
                    Debug.Log(command.execute(arguments[commandMethods.IndexOf(command)]));
                }
                arguments[commandMethods.IndexOf(command)] = GUILayout.TextField(arguments[commandMethods.IndexOf(command)]);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Label(command.description);
            }
        }
    }
}
