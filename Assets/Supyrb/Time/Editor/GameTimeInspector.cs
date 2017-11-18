using System;
using System.Collections.Generic;

namespace Supyrb.Common
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;

    [CustomEditor(typeof(GameTime))]
    public class GameTimeInspector : Editor
    {
        private TimeSpan timeSpan;
        private GUIStyle greenText;
        private GUIStyle redText;
        private const int modifierRefreshRate = 50;
        private string modifierString = "";
        private int modifierRefreshTimeCounter;

        void OnEnable()
        {
            greenText = new GUIStyle();
            greenText.normal.textColor = Color.green;

            redText = new GUIStyle();
            redText.normal.textColor = Color.red;

            modifierRefreshTimeCounter = 0;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Separator();
            GUIStyle style = (GameTime.Racing) ? greenText : redText;
            EditorGUILayout.LabelField("Timescale", Time.timeScale.ToString("0.00"));
            EditorGUILayout.LabelField("Elapsed time", GetTimeStringFromSeconds(GameTime.TimeSinceStartup));
            EditorGUILayout.LabelField("Elapsed race time", GetTimeStringFromSeconds(GameTime.RaceTime), style);

            if (GameTime.Instance.GetTimeManipulator() != null && Application.isPlaying)
            {
                GUILayout.Space(20f);
                modifierRefreshTimeCounter++;
                bool refreshed = false;
                if (modifierRefreshTimeCounter >= modifierRefreshRate)
                {
                    modifierRefreshTimeCounter = 0;
                    modifierString = StringOfAllTimeScaleModifiers();
                    refreshed = true;
                }
                EditorGUILayout.LabelField("Modifiers " + (refreshed?".":""), EditorStyles.boldLabel);
                EditorGUILayout.TextArea(modifierString);
            }
            this.Repaint();
        }

        private string GetTimeStringFromSeconds(float seconds)
        {
            timeSpan = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        private string StringOfAllTimeScaleModifiers()
        {
            string list = "";
            var timeManipulators = GameTime.Instance.GetTimeManipulator();
            for (int i = 0; i < timeManipulators.MaxModifiers; i++)
            {
                var value = (timeManipulators.UsedModifierSlots[i])
                    ? timeManipulators.Modifiers[i].ToString("0.00")
                    : "---";
                list += string.Format("{0}: {1}\n", i, value);
            }
            return list;
        }
    }
}
