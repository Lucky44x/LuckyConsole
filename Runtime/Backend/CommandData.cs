using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Lucky44.LuckyConsole
{
    public class Command
    {
        public string name;
        public string description;

        public Dictionary<string, Type> types = new Dictionary<string, Type>();
        private List<object> defaultParams = new List<object>();

        private MethodInfo methodReference;

        public Command(MethodInfo method)
        {
            CommandAttribute a = (CommandAttribute)method.GetCustomAttribute(typeof(CommandAttribute));

            name = a.commandName;
            description = a.commandDescription;

            methodReference = method;
            ParameterInfo[] parameters = method.GetParameters();
            foreach (ParameterInfo pi in parameters)
            {
                this.types.Add(pi.Name,pi.ParameterType);
                this.defaultParams.Add(pi.DefaultValue);
            }
        }

        public string execute(string input)
        {
            string[] tmpArr = input.Split(' ');
            string[] args = new string[tmpArr.Length-1];

            for(int i = 0; i < args.Length; i++)
            {
                args[i] = tmpArr[i+1];
            }

            object[] parameters = new object[types.Count];
            for(int i = 0; i < parameters.Length; i++)
            {
                object param = null;
                if (i < defaultParams.Count)
                    param = defaultParams[i];

                if(i < args.Length)
                {
                    Type t = types.Values.ToArray()[i];
                    if (t == typeof(int))
                    {
                        int tmp = 0;
                        if (!int.TryParse(args[i], out tmp))
                        {
                            return parseError(i);
                        }
                        param = tmp;
                    }
                    else if (t == typeof(float))
                    {
                        float tmp = 0.0f;
                        if (!float.TryParse(args[i], out tmp))
                        {
                            return parseError(i);
                        }
                        param = tmp;
                    }
                    else if (t == typeof(bool))
                    {
                        bool tmp = false;
                        if (!bool.TryParse(args[i], out tmp))
                        {
                            return parseError(i);
                        }
                        param = tmp;
                    }
                    else if (t == typeof(string))
                    {
                        param = args[i];
                    }
                }

                parameters[i] = param;
            }

            object ret = methodReference.Invoke(null, parameters);
            return (string)ret;
        }

        string parseError(int i)
        {
            return $" {types.Keys.ToArray()[i]} is of type {types.Values.ToArray()[i]} please give a valid value".color("#ff4757");
        }

        public override string ToString()
        {
            string ret = name;
            ret += " ";

            foreach(KeyValuePair<string,Type> kvp in types)
            {
                ret += kvp.Key + $"[{kvp.Value}] ";
            }

            return ret;
        }
    }
}
