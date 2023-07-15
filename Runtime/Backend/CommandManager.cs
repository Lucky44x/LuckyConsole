using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace Lucky44.LuckyConsole
{
    public class CommandManager
    {
        Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public List<Command> subscribeMethods()
        {
            commands = new Dictionary<string, Command>();
            List<Command> commandMethods = new List<Command>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                bool cont = false;
                foreach (AssemblyName asName in assembly.GetReferencedAssemblies())
                    if (asName.FullName == typeof(CommandManager).Assembly.FullName)
                        cont = true;

                if (!cont)
                    continue;

                foreach (Type type in assembly.GetTypes())
                {
                    foreach (MethodInfo method in type.GetMethods())
                    {
                        if (method.GetCustomAttribute(typeof(CommandAttribute)) == null)
                            continue;

                        if (method.ReturnParameter == null) { throw new Exception($"Method {method.Name} in class {type.Name} is command but doesn't return a string"); }
                        if (!method.IsStatic) { throw new Exception($"Method {method.Name} in class {type.Name} is command, but isn't static static"); }
                        if (!method.IsPublic) { throw new Exception($"Method {method.Name} in class {type.Name} is command, but isn't public"); }

                        Command c = new Command(method);

                        commandMethods.Add(c);
                        commands.Add(c.name, c);
                    }
                }
            }

            return commandMethods;
        }

        public void forceRegisterMethod(Type type)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                if (method.GetCustomAttribute(typeof(CommandAttribute)) == null)
                    continue;

                if (method.ReturnParameter == null) { throw new Exception($"Method {method.Name} in class {type.Name} is command but doesn't return a string"); }
                if (!method.IsStatic) { throw new Exception($"Method {method.Name} in class {type.Name} is command, but isn't static static"); }
                if (!method.IsPublic) { throw new Exception($"Method {method.Name} in class {type.Name} is command, but isn't public"); }

                Command c = new Command(method);

                commands.Add(c.name, c);
            }
        }

        public List<Command> getCommands()
        {
            List<Command> commands = new List<Command>();
            foreach (Command c in this.commands.Values)
            {
                commands.Add(c);
            }
            return commands;
        }

        public List<string> getAutocompletion(string input)
        {
            List<string> ret = new List<string>();

            foreach(string n in commands.Keys)
            {
                if(n.Contains(input))
                    ret.Add(n);
            }

            return ret;
        }

        public string executeCommand(string input)
        {
            Command c = null;
            if(commands.TryGetValue(input.Split(' ')[0], out c))
            {
                return c.execute(input);
            }

            return $"Command {input.Split(' ')[0]} wasn't found".color("#ff4757");
        }
    }
}
