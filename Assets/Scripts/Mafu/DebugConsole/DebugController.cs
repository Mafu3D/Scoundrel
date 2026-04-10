using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEditor.Search;

namespace Mafu.DebugConsole
{
    public class DebugCommandLine : MonoBehaviour
    {
        // TODO: Add parameters for tweaking sizes, font size, color, etc.
        // Need to use GUIStyle for font sizes

        private Dictionary<string, object> commands;
        private bool show;
        private bool showHelp;
        private string currentInput;
        private Vector2 helpScroll;
        private Vector2 searchScroll;

        private List<object> GetSortedCommands()
        {
            List<string> sortedKeys = GetSortedIDs();
            List<object> sortedCommands = new();
            for (int i = 0; i < sortedKeys.Count; i++)
            {
                sortedCommands.Add(commands[sortedKeys[i]]);
            }
            return sortedCommands;
        }

        private List<string> GetSortedIDs()
        {
            List<string> sortedKeys = commands.Keys.ToList();
            sortedKeys.Sort();
            return sortedKeys;
        }

        private void OnGUI()
        {
            if (!show) return;

            float y = 0f;

            // Help scroll field - Shows a list of available commands
            if (showHelp)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");
                Rect helpViewport = new Rect(0, 0, Screen.width - 20, 20 * commands.Count);
                helpScroll = GUI.BeginScrollView( new Rect(0, y + 5f, Screen.width, 90), helpScroll, helpViewport);

                List<object> commandsToDisplay;
                if(string.IsNullOrEmpty(currentInput))
                {
                    commandsToDisplay = GetSortedCommands();
                }
                else
                {
                    commandsToDisplay = GetCommandsFromFuzzySearch(currentInput, 100);
                }
                for (int i = 0; i < commandsToDisplay.Count; i++)
                {
                    DebugCommandBase command = commandsToDisplay[i] as DebugCommandBase;
                    string label = $"{command.CommandFormat} : {command.CommandDescription}";
                    Rect labelRect = new Rect(5, 20 * i, helpViewport.width - 100, 20);
                    GUI.Label(labelRect, label);
                }
                GUI.EndScrollView();

                y += 100;
            }

            // Input text field
            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.SetNextControlName("DebugCommandField");
            currentInput = GUI.TextField(new Rect(10f, y + 5f, Screen.width-20f, 20f), currentInput);
            y += 30;

            GUI.backgroundColor = new Color(0,0,0,0);

            // If enter is pressed, set focus on the text field for quick debug command execution
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                GUI.FocusControl("DebugCommandField");
                // Optionally, consume the event to prevent other controls from reacting
                Event.current.Use();
            }
        }

        private List<object> GetCommandsFromFuzzySearch(string input, int targetScore)
        {
            List<object> matchedCommands = new();

            List<string> sortedKeys = GetSortedIDs();
            for (int i = 0; i < sortedKeys.Count; i++)
            {
                string key = sortedKeys[i];
                long score = 0;
                List<int> matches = new();
                if(FuzzySearch.FuzzyMatch(input, key, ref score, matches))
                {
                    if (score < 100) { continue; }
                    matchedCommands.Add(commands[key]);
                }
            }

            return matchedCommands;
        }

        public void OnToggleDebug(InputValue inputValue)
        {
            show = !show;
            currentInput = "";
        }

        public void OnReturn(InputValue inputValue)
        {
            if (show && !string.IsNullOrEmpty(currentInput))
            {
                ExecuteDebugCommand(currentInput);
                currentInput = "";
            }
        }

        void Awake()
        {
            // Create the commands list with a default "help" command
            commands = new()
            {
                {
                    "help",
                    new DebugCommand("help", "Shows a list of available commands", "help", () =>
                    {
                        showHelp = !showHelp;
                    })
                }
            };

            AddCommands();
            AddCommands<int>();
            AddCommands<float>();
            AddCommands<bool>();
            AddCommands<string>();
        }

        private void AddCommands()
        {
            // Using Reflection here could be not great. But this only runs at start up. <100-200 comamnds shouldn't impact perf
            var commandTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t.GetCustomAttribute<DebugCommandAttribute>() != null &&
                    typeof(IDebugCommand).IsAssignableFrom(t) &&
                    !t.IsAbstract);

            foreach (var type in commandTypes)
            {
                IDebugCommand instance = (IDebugCommand)Activator.CreateInstance(type);
                DebugCommand command = new DebugCommand(instance.ID, instance.Description, instance.Format, () => instance.Invoke());
                commands.Add(instance.ID, command);
            }
        }

        private void AddCommands<T>()
        {
            // Using Reflection here could be not great. But this only runs at start up. <100-200 comamnds shouldn't impact perf
            var commandTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t.GetCustomAttribute<DebugCommandAttribute>() != null &&
                    typeof(IDebugCommand<T>).IsAssignableFrom(t) &&
                    !t.IsAbstract);

            foreach (var type in commandTypes)
            {
                IDebugCommand<T> instance = (IDebugCommand<T>)Activator.CreateInstance(type);
                DebugCommand<T> command = new DebugCommand<T>(instance.ID, instance.Description, instance.Format, instance.DefaultParameter, (x) => instance.Invoke(x));
                commands.Add(instance.ID, command);
            }
        }

        public void ExecuteDebugCommand(string input)
        {
            foreach(string commandID in commands.Keys)
            {
                if (!input.Contains(commandID)) { continue; }

                DebugCommandBase commandBase = commands[commandID] as DebugCommandBase;

                if (TryInvokeCommand(commandBase)) { return; }
                if (TryInvokeCommand<int>(commandBase, int.Parse)) { return; }
                if (TryInvokeCommand<float>(commandBase, float.Parse)) { return; }
                if (TryInvokeCommand<bool>(commandBase, bool.Parse)) { return; }
                if (TryInvokeCommand<string>(commandBase, (x) => x)) { return; }

                Debug.LogWarning($"Something went wrong executing command {commandBase.CommandID}");
                return;
            }
        }

        private bool TryInvokeCommand(DebugCommandBase commandBase)
        {
            if (commandBase as DebugCommand == null) { return false; }
            (commandBase as DebugCommand).Invoke();
            return true;
        }

        private bool TryInvokeCommand<T>(DebugCommandBase commandBase, Func<string, T> parserDelegate)
        {
            if (commandBase as DebugCommand<T> == null) { return false; }

            DebugCommand<T> debugCommand = commandBase as DebugCommand<T>;
            if (TryGetParameterValue(debugCommand, currentInput, out string parameterValue))
            {
                debugCommand.Invoke(parserDelegate(parameterValue));
                return true;
            }

            return false;
        }

        private bool TryGetParameterValue<T>(DebugCommand<T> command, string inputString, out string parameterValue)
        {
            string[] properties = inputString.Split(" ");
            if(properties.Length == 1)
            {
                parameterValue = command.DefaultParameter.ToString();
                if (string.IsNullOrEmpty(parameterValue))
                {
                    Debug.LogWarning($"Debug command <{command.CommandID}> has no default parameter, please provide one!");
                    return false;
                }
            }
            else
            {
                parameterValue = properties[1];
            }
            return true;
        }

    }
}