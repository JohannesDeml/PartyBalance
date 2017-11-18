﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Enables a console that shows Unity's Debug.Log messages when user holds "Shift+C" down.
    /// It's meant for debug purposes.
    /// </summary>
    public class RuntimeConsole : MonoBehaviour
    {
        private struct Log
        {
            public LogType logType;
            public string message;
            public string stackTrace;

            public bool AsGUI( bool showLog, bool showWarning, bool showError )
            {
                switch( logType )
                {
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    if( !showError )
                        return false;
                    GUI.color = Color.red;
                    break;

                case LogType.Warning:
                    if( !showWarning )
                        return false;
                    GUI.color = Color.yellow;
                    break;

                case LogType.Log:
                    if( !showLog )
                        return false;
                    GUI.color = Color.white;
                    break;
                }

                bool isSelected = GUILayout.Button( string.Concat( "> ", message ), GUI.skin.label );
                GUI.color = Color.white;

                return isSelected;
            }
        }

        /// <summary>
        /// Max number of logs saved.
        /// </summary>
        public int maxLogs = 100;

        private List<Log> logs = new List<Log>();
        private int selectedLog = -1;
        private Vector2 scroll = Vector2.zero;
        private Vector2 stackTraceScroll = Vector2.zero;

        private bool showLog = true;
        private bool showWarning = false;
        private bool showError = true;

        private GUIStyle windowStyle = null;

        private void Awake()
        {
            if( enabled )
            {
                Application.logMessageReceived += AddMessage;
            }
        }

        private void TryAdjustWindowBgColor()
        {
            if( windowStyle != null )
                return;

            windowStyle = new GUIStyle( GUI.skin.window );

            int width = 4;
            int height = 4;

            Texture2D windowBgTexture = new Texture2D( width, height );

            Color[] pixels = new Color[width * height];
            for( int i = 0; i < pixels.Length; i++ )
            {
                pixels[i] = new Color( 0.0f, 0.0f, 0.0f, 0.8f );
            }

            windowBgTexture.SetPixels( pixels );
            windowBgTexture.Apply();

            windowStyle.normal.background = windowBgTexture;
        }

        private void AddMessage( string message, string stackTrace, LogType logType )
        {
            if( enabled )
            {
                Log log = new Log();
                log.logType = logType;
                log.message = message;
                log.stackTrace = stackTrace;

                logs.Insert( logs.Count, log );
                if( logs.Count > maxLogs && maxLogs > 0 )
                {
                    logs.RemoveAt( 0 );
                }
            }
        }

        private void OnGUI()
        {
            TryAdjustWindowBgColor();

            if( Input.GetKey( KeyCode.LeftShift ) && Input.GetKey( KeyCode.C ) )
            {
                Rect screenRect = new Rect( 0.0f, 0.0f, Screen.width, Screen.height );
                GUILayout.BeginArea( screenRect, windowStyle );

                GUILayout.BeginHorizontal();
                GUILayout.Label( "LOGS" );
                GUILayout.FlexibleSpace();
                showLog = GUILayout.Toggle( showLog, "Log", GUI.skin.button );
                showWarning = GUILayout.Toggle( showWarning, "Warning", GUI.skin.button );
                showError = GUILayout.Toggle( showError, "Error", GUI.skin.button );
                if( GUILayout.Button( "Clear" ) )
                {
                    logs.Clear();
                    selectedLog = -1;
                }
                GUILayout.EndHorizontal();

                scroll = GUILayout.BeginScrollView( scroll );

                for( int i = 0; i < logs.Count; i++ )
                {
                    if( logs[i].AsGUI( showLog, showWarning, showError ) )
                    {
                        selectedLog = i;
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndScrollView();

                if( selectedLog >= 0 )
                {
                    GUILayout.Label( "-------------------------------------------------------------------------" );

                    stackTraceScroll = GUILayout.BeginScrollView( stackTraceScroll, GUILayout.Height( 128.0f ) );

                    Log log = logs[selectedLog];
                    GUILayout.Label( log.stackTrace );

                    GUILayout.EndScrollView();
                }

                GUILayout.EndArea();
            }
        }
    }
}
