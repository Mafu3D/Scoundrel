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

    [DebugCommand]
    public class AddBuffTo0 : IDebugCommand<string>
    {
        public string ID => "addBuff0";

        public string Description => "Adds the ability to the card at the given slot index";

        public string Format => "addBuff0 <string>";

        public bool HasDefaultValue => false;

        public string DefaultParameter => "";

        public void Invoke(string buffName)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            Buff buff = gameManager.BuffRegistry.GetBuffFromName(buffName);
            if (buff == null)
            {
                UnityEngine.Debug.Log($"{buffName} not found!");
            }

            gameManager.CurrentRoom.Cards[0].RegisterBuff(buff);
        }
    }

    [DebugCommand]
    public class AddBuffTo1 : IDebugCommand<string>
    {
        public string ID => "addBuff1";

        public string Description => "Adds the ability to the card at the given slot index";

        public string Format => "addBuff1 <string>";

        public bool HasDefaultValue => false;

        public string DefaultParameter => "";

        public void Invoke(string buffName)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            Buff buff = gameManager.BuffRegistry.GetBuffFromName(buffName);
            if (buff == null)
            {
                UnityEngine.Debug.Log($"{buffName} not found!");
            }

            gameManager.CurrentRoom.Cards[1].RegisterBuff(buff);
        }
    }

    [DebugCommand]
    public class AddBuffTo2 : IDebugCommand<string>
    {
        public string ID => "addBuff2";

        public string Description => "Adds the ability to the card at the given slot index";

        public string Format => "addBuff2 <string>";

        public bool HasDefaultValue => false;

        public string DefaultParameter => "";

        public void Invoke(string buffName)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            Buff ability = gameManager.BuffRegistry.GetBuffFromName(buffName);
            if (ability == null)
            {
                UnityEngine.Debug.Log($"{buffName} not found!");
            }

            gameManager.CurrentRoom.Cards[2].RegisterBuff(ability);
        }
    }

    [DebugCommand]
    public class AddBuffTo3 : IDebugCommand<string>
    {
        public string ID => "addBuff3";

        public string Description => "Adds the ability to the card at the given slot index";

        public string Format => "addBuff3 <string>";

        public bool HasDefaultValue => false;

        public string DefaultParameter => "";

        public void Invoke(string buffName)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            Buff buff = gameManager.BuffRegistry.GetBuffFromName(buffName);
            if (buff == null)
            {
                UnityEngine.Debug.Log($"{buffName} not found!");
            }

            gameManager.CurrentRoom.Cards[3].RegisterBuff(buff);
        }
    }
}