using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mafu.DebugConsole;
using Mafu.UnityServiceLocator;
using Project.Core;
using Project.Decks;
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
            gameManager.HandlePlayerRun(force: true);
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
            gameManager.DungeonController.CurrentRoom.TryRemoveCard(gameManager.DungeonController.CurrentRoom.GetAllCards()[index]);
        }
    }

    [DebugCommand]
    public class Heal : IDebugCommand<int>
    {
        public string ID => "heal";

        public string Description => "Restore X hit points";

        public string Format => "heal <int>";

        public bool HasDefaultValue => true;

        public string DefaultParameter => "0";

        public void Invoke(int amount)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.Player.Heal(amount);
        }
    }

    [DebugCommand]
    public class AddArmor : IDebugCommand<int>
    {
        public string ID => "addArmor";

        public string Description => "Add X armor";

        public string Format => "addArmor <int>";

        public bool HasDefaultValue => true;

        public string DefaultParameter => "0";

        public void Invoke(int amount)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.Player.AddArmor(amount);
        }
    }

    [DebugCommand]
    public class AddGold : IDebugCommand<int>
    {
        public string ID => "addGold";

        public string Description => "Gain X gold";

        public string Format => "addGold <int>";

        public bool HasDefaultValue => true;

        public string DefaultParameter => "10";

        public void Invoke(int amount)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.Player.AddGold(amount);
        }
    }

    [DebugCommand]
    public class AddPoints : IDebugCommand<int>
    {
        public string ID => "addPoints";

        public string Description => "Gain X points";

        public string Format => "addPoints <int>";

        public bool HasDefaultValue => true;

        public string DefaultParameter => "10000";

        public void Invoke(int amount)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.ScoreKeeper.AddToScore(amount);
        }
    }

    [DebugCommand]
    public class AddRunTokens : IDebugCommand<int>
    {
        public string ID => "addRunTokens";

        public string Description => "Gain X run tokens";

        public string Format => "addRunTokens <int>";

        public bool HasDefaultValue => true;

        public string DefaultParameter => "1";

        public void Invoke(int amount)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.Player.AddRunTokens(amount);
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
                return;
            }

            Buff newInstance = gameManager.DungeonController.CurrentRoom.GetSlotOfindex(0).ActiveCard.AddNewBuff(buff);
            newInstance.OnDraw();
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
                return;
            }

            Buff newInstance = gameManager.DungeonController.CurrentRoom.GetSlotOfindex(1).ActiveCard.AddNewBuff(buff);
            newInstance.OnDraw();
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
            Buff buff = gameManager.BuffRegistry.GetBuffFromName(buffName);
            if (buff == null)
            {
                UnityEngine.Debug.Log($"{buffName} not found!");
                return;
            }

            Buff newInstance = gameManager.DungeonController.CurrentRoom.GetSlotOfindex(2).ActiveCard.AddNewBuff(buff);
            newInstance.OnDraw();
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
                return;
            }

            Buff newInstance = gameManager.DungeonController.CurrentRoom.GetSlotOfindex(3).ActiveCard.AddNewBuff(buff);
            newInstance.OnDraw();
        }
    }

    [DebugCommand]
    public class AddBuffPlayer : IDebugCommand<string>
    {
        public string ID => "addBuffPlayer";

        public string Description => "Adds the ability to the player";

        public string Format => "addBuffPlayer <string>";

        public bool HasDefaultValue => false;

        public string DefaultParameter => "";

        public void Invoke(string buffName)
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            PlayerBuff buff = gameManager.BuffRegistry.GetPlayerBuffFromName(buffName);
            if (buff == null)
            {
                UnityEngine.Debug.Log($"{buffName} not found!");
                return;
            }

            gameManager.Player.AddNewBuff(buff);
        }
    }

    [DebugCommand]
    public class GoToNextFloor : IDebugCommand
    {
        public string ID => "nextFloor";

        public string Description => "Immediately go to the next floor";

        public string Format => "nextFloor";

        public void Invoke()
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.GoToExitFloorState();
        }
    }

    [DebugCommand]
    public class DummyGameplayEffect : IDebugCommand
    {
        public string ID => "dummyEffect";

        public string Description => "Fire off a dummy gameplay effect to wait 4 seconds";

        public string Format => "dummyEffect";

        public void Invoke()
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            gameManager.GameplayEffectQueue.Add(new TestGameplayEffectWaitForSeconds(4f));
        }
    }

    [DebugCommand]
    public class LogDeck : IDebugCommand
    {
        public string ID => "logDeck";

        public string Description => "Log the current contents of the deck as a text file";

        public string Format => "logDeck";

        public void Invoke()
        {
            ServiceLocator.Global.Get(out GameManager gameManager);
            Deck<RuntimeCardModel> deck = gameManager.DeckController.Deck;

            List<string> contents = new();
            foreach(RuntimeCardModel card in deck.AllItems)
            {
                string status = "";
                if (gameManager.DungeonController.CurrentRoom.GetAllCards().Contains(card))
                {
                    status = " (In Room)";
                }
                else if (!deck.RemainingItems.Contains(card))
                {
                    status = " (Discarded)";
                }
                contents.Add($"{card.ToString()}{status}");
                foreach (Buff buff in card.BuffManager.GetBuffs())
                {
                    contents.Add($"    {buff.Name}");
                }
            }

            string rootDir;
            if (Application.isEditor)
            {
                rootDir = System.IO.Directory.GetCurrentDirectory();
            }
            else
            {
                rootDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                rootDir += "/Scoundrel";
            }
            string fileName = $"DeckContents_{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}";
            string filePath = $"{rootDir}/DebugLogs/{fileName}.txt";
            Directory.CreateDirectory($"{rootDir}/DebugLogs");
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                foreach(string line in contents)
                {
                    Debug.Log(line);
                }
            }
            else
            {
                File.WriteAllLines(filePath, contents);
                Debug.Log($"Logged deck contents to {filePath}");
            }
        }
    }
}