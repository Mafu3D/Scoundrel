using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mafu.DebugConsole;
using Mafu.UnityServiceLocator;
using UnityEngine;

namespace Project.DebugCommands
{
    [DebugCommand]
    public class Print : IDebugCommand<string>
    {
        public string ID => "print";

        public string Description => "Print text to log";

        public string Format => "print <text>";

        public bool HasDefaultValue => false;

        public string DefaultParameter => "";

        public void Invoke(string text)
        {
            UnityEngine.Debug.Log(text);
        }
    }

    [DebugCommand]
    public class ForceRun : IDebugCommand
    {
        public string ID => "run";

        public string Description => "Forces a run from this room";

        public string Format => "run";

        public void Invoke()
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.DEBUG_RUN();
        }
    }

    [DebugCommand]
    public class Kill : IDebugCommand<int>
    {
        public string ID => "kill";

        public string Description => "Kill card at the given slot";

        public string Format => "kill <int>";

        public bool HasDefaultValue => true;

        public string DefaultParameter => "0";

        public void Invoke(int index)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.CurrentRoom.DEBUG_REMOVECARD(gameManager.CurrentRoom.Cards[index]);
        }
    }
}