using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mafu.DebugConsole
{
    public struct DebugLogEntry
    {
        public string Timestamp;
        public string Text;
        public Color Color;
        public DebugLogEntry(string text, string timestamp, Color color)
        {
            Text = text;
            Timestamp = timestamp;
            Color = color;
        }
    }

    public class DebugLog : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField] Color defaultColor = Color.white;

        [Header("Sizing")]
        [SerializeField] float width = 400;
        [SerializeField] float lineSpacing = 10;

        [Header("Lines")]
        [SerializeField] int displayedLines = 10;
        [SerializeField] int maxLines = 100;

        private bool show;
        private List<DebugLogEntry> entries;
        private Vector2 scroll;

        float tempTimer = 0f;
        float tempDuration = 0.5f;
        int i;
        private List<KeyValuePair<string, Color>> tempEntries;

        void Awake()
        {
            entries = new();

            // tempEntries = new()
            // {
            //     new KeyValuePair<string, Color> ("hello world", Color.white),
            //     new KeyValuePair<string, Color> ("this is green", Color.green),
            //     new KeyValuePair<string, Color> ("this is red", Color.red)
            // };

            // for (int i = 0; i < 100; i++)
            // {
            //     Log("hello world");
            // }
        }

        void Update()
        {
            // tempTimer += Time.deltaTime;
            // if (tempTimer > tempDuration)
            // {
            //     i++;
            //     if (i > 2)
            //     {
            //         i=0;
            //     }
            //     tempTimer = 0f;
            //     Log(tempEntries[i].Key, tempEntries[i].Value);
            // }
        }

        public void OnToggleDebug(InputValue _)
        {
            show = !show;
        }

        private void OnGUI()
        {
            if (!show) return;

            float height = lineSpacing * displayedLines;
            float y = Screen.height - height;
            GUI.Box(new Rect(0, y, width, height), "");

            Rect logViewport = new Rect(0, y, width - 20, lineSpacing * entries.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y, width, height), scroll, logViewport);

            for (int i = 0; i < entries.Count; i++)
            {
                DebugLogEntry entry = entries[i];
                string hexRGB = ColorUtility.ToHtmlStringRGB(entry.Color);
                string output = $"<color=#{hexRGB}>{entry.Timestamp} : {entry.Text}</color>";
                Rect labelRect = new Rect(5, Screen.height - (lineSpacing * i), logViewport.width - 20, lineSpacing);
                GUIStyle style = new GUIStyle();
                style.richText = true;
                GUI.Label(labelRect, output, style);
            }

            GUI.EndScrollView();
            GUI.backgroundColor = new Color(0,0,0,0);
        }

        public void Log(string text) => Log(text, defaultColor);

        public void Log(string text, Color color)
        {
            TimeSpan time = TimeSpan.FromSeconds(Time.time);
            string formattedTime = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);

            entries.Insert(0, new DebugLogEntry(text, formattedTime, color));
            ClampEntries();

            scroll.y = lineSpacing * displayedLines;
        }

        private void ClampEntries()
        {
            if (entries.Count > maxLines)
            {
                int amount = entries.Count - 100;
                entries.RemoveRange(100, amount);
            }
        }
    }
}