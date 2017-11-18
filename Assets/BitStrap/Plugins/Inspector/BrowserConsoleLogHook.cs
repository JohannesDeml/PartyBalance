using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Debug helper class that will redirect Unity's Debug.Log messages to your browser's console
    /// via the javascript equivalent: "console.log()".
    /// </summary>
    public class BrowserConsoleLogHook : MonoBehaviour
    {
        public bool showConsoleLog = true;

        private void Awake()
        {
            Register();
        }

        private void OnEnable()
        {
            Register();
        }

        private void Register()
        {
            if( Application.isWebPlayer || Application.isEditor )
            {
                Application.logMessageReceived += Hook;
            }
        }

        private void Hook( string logMessage, string stackTrace, LogType type )
        {
            if( showConsoleLog )
            {
                string message;

                if( string.IsNullOrEmpty( stackTrace ) )
                {
                    message = string.Format( "[UNITY] {0}", logMessage );
                }
                else
                {
                    message = string.Format( "[UNITY] {0}\nStackTrace:\n{1}", logMessage, stackTrace );
                }

                switch( type )
                {
                case LogType.Log:
                    Application.ExternalCall( "console.log", message );
                    break;

                case LogType.Warning:
                    Application.ExternalCall( "console.warn", message );
                    break;

                default:
                    Application.ExternalCall( "console.error", message );
                    break;
                }
            }
        }
    }
}
