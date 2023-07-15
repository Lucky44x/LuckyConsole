using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Lucky44.LuckyConsole
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : System.Attribute
    {
        public string commandName = "";
        public string commandDescription = "";

        public CommandAttribute(string command, string description)
        {
            commandName = command;
            commandDescription = description;
        }
    }
}