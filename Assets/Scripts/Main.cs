#region IMPORTS

using UnityEngine;

#if GAMBIT_CONFIG
using gambit.config;
#endif

#endregion

namespace gambit.launcher
{

    /// <summary>
    /// The main entrance point for the neuroguide launcher application
    /// </summary>
    public class Main: MonoBehaviour
    {
        #region PUBLIC - VARIABLES

        public float fadeInDuration;

        /// <summary>
        /// The process handler script used to manage the process for app1
        /// </summary>
        public Process app1;

        /// <summary>
        /// The process handler script used to manage the process for app2
        /// </summary>
        public Process app2;

        #endregion

        #region PRIVATE - VARIABLES

        /// <summary>
        /// Config Manager System created at start
        /// </summary>
        private ConfigManager.ConfigManagerSystem configSystem;

        #endregion

        #region PUBLIC - START

        /// <summary>
        /// Unity lifecycle method
        /// </summary>
        //----------------------------------//
        public void Start()
        //----------------------------------//
        {
            FadeManager.Fade( 0f, fadeInDuration );

            ConfigManager.UpdateLocalDataAndReturn
            (
                "config",
                true,

                //ON SUCCESS
                (ConfigManager.ConfigManagerSystem _system ) => 
                {
                    configSystem = _system;

                    GetVariablesFromConfig();
                },

                //ON FAILED
                (string error)=>
                {
                    Debug.LogError( error );
                }
            );

        } //END Start

        #endregion

        #region PRIVATE - GET VARIABLES FROM CONFIG

        /// <summary>
        /// After loading the config, sets the scripts to the data pulled from the config file
        /// </summary>
        //----------------------------------------//
        private void GetVariablesFromConfig()
        //----------------------------------------//
        {
            //App1 - Name
            ConfigManager.GetNestedString
            (
                configSystem,
                new string[ ]
                {
                    "app1",
                    "name"
                },
                (string name)=>
                {
                    app1.SetText( name );
                },
                (string error)=>
                {
                    Debug.LogError( error ); 
                }
            );

            //App1 - Path
            ConfigManager.GetNestedPath
            (
                configSystem,
                new string[ ] 
                {
                    "app1",
                    "path"
                },
                (string path)=>
                {
                    app1.path = path;
                    app1.CreateSystem();
                },
                (string error)=>
                {
                    Debug.LogError( error );
                }
            );

            //App2 - Name
            ConfigManager.GetNestedString
            (
                configSystem,
                new string[ ]
                {
                    "app2",
                    "name"
                },
                ( string name ) =>
                {
                    app2.SetText( name );
                },
                ( string error ) =>
                {
                    Debug.LogError( error );
                }
            );

            //App2 - Path
            ConfigManager.GetNestedPath
            (
                configSystem,
                new string[ ]
                {
                    "app2",
                    "path"
                },
                ( string path ) =>
                {
                    app2.path = path;
                    app2.CreateSystem();
                },
                ( string error ) =>
                {
                    Debug.LogError( error );
                }
            );

        } //END GetVariablesFromConfig Method

        #endregion

    } //END Main Class

} //END gambit.launcher Namespace