#region IMPORTS

using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;


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

        [Tooltip("How long should fade in from black last?")]
        public float fadeInDuration;

        [Tooltip( "How long should fade out to black last?" )]
        public float fadeOutDuration;

        /// <summary>
        /// The names of the config files to locate in our resources folder
        /// </summary>
        private List<string> appConfigFileNames = new List<string>() 
        { 
            "BrainWaves",
            "Tesseract"
        };

        /// <summary>
        /// The root GameObject of the button that will launch an application process
        /// </summary>
        public List<GameObject> processButtons = new List<GameObject>();

        #endregion

        #region PRIVATE - VARIABLES

        /// <summary>
        /// The processes handlers we pass values into from our config.json.
        /// One process component is generated per appConfigFileName
        /// </summary>
        private List<Process> processes = new List<Process>();

        /// <summary>
        /// Config Manager System for the launcher config file
        /// </summary>
        private ConfigManager.ConfigManagerSystem configSystem_launcher;

        /// <summary>
        /// The config systems we create to pull our json config files
        /// </summary>
        private List<ConfigManager.ConfigManagerSystem> configSystem_apps = new List<ConfigManager.ConfigManagerSystem>();

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

            CreateProcessComponents();

            LoadConfigs();

        } //END Start

        #endregion

        #region PRIVATE - CREATE PROCESS COMPONENTS ON BUTTONS

        /// <summary>
        /// For each app config file, generate a process component and place it on a button that will launch the process
        /// </summary>
        //------------------------------------------//
        private void CreateProcessComponents()
        //------------------------------------------//
        {

            if(processButtons == null || (processButtons != null && processButtons.Count == 0))
            {
                Debug.LogError( "Main.cs CreateProcessComponents() Process Button Parent GameObject links are not setup. Unable to continue" );
                return;
            }

            int buttonCounter = 0;

            foreach(GameObject go in processButtons)
            {
                Process process = go.AddComponent<Process>();
                process.id = appConfigFileNames[ buttonCounter ];
                process.fadeInDuration = fadeInDuration;
                process.fadeOutDuration = fadeOutDuration;

                processes.Add( process );
                buttonCounter++;
            }

            foreach(Process process in processes)
            {
                if(string.IsNullOrEmpty( process.id ))
                {
                    Debug.LogError( "Main.cs CreateProcessComponents() process missing id, make sure each process has an id used to identify and set its values using the configuration json file" );
                    return;
                }
            }

        } //END CreateProcessComponents

        #endregion

        #region PRIVATE - LOAD CONFIGS

        /// <summary>
        /// Loads the config files for the launcher as well as all of the apps
        /// </summary>
        //-------------------------------------------//
        private void LoadConfigs()
        //-------------------------------------------//
        {

            int count = 0;
            int total = 1 + (appConfigFileNames.Count);

            ConfigManager.UpdateLocalDataAndReturn
            (
                "config",
                true,

                //ON SUCCESS
                ( ConfigManager.ConfigManagerSystem _system ) =>
                {
                    configSystem_launcher = _system;

                    count++;
                    if(count == total)
                    {
                        GetVariablesFromConfig();
                    }
                },

                //ON FAILED
                ( string error ) =>
                {
                    Debug.LogError( error );
                }
            );

            foreach(string appConfigFileName in appConfigFileNames)
            {
                ConfigManager.UpdateLocalDataAndReturn
                (
                    appConfigFileName,
                    true,

                    //ON SUCCESS
                    ( ConfigManager.ConfigManagerSystem _system ) =>
                    {
                        _system.id = appConfigFileName;
                        configSystem_apps.Add( _system );

                        count++;
                        if(count == total)
                        {
                            GetVariablesFromConfig();
                        }
                    },

                    //ON FAILED
                    ( string error ) =>
                    {
                        Debug.LogError( error );
                    }
                );
            }

        } //END LoadConfigs Method

        #endregion

        #region PRIVATE - GET VARIABLES FROM CONFIG

        /// <summary>
        /// After loading the config, sets the scripts to the data pulled from the config file
        /// </summary>
        //----------------------------------------//
        private void GetVariablesFromConfig()
        //----------------------------------------//
        {
            int count = 0;
            int total = 2 + (configSystem_apps.Count * 6);

            //There are some pieces of data that need to be sent into each process, such as the udp address and port

            //Communication - Address
            ConfigManager.GetNestedString
            (
                configSystem_launcher,
                new string[ ]
                {
                    "communication",
                    "address"
                },
                ( string value ) =>
                {
                    foreach( Process process in processes)
                    {
                        process.AddArgumentKey( "address" );
                        process.AddArgumentValue( value );
                    }

                    count++;
                    if(count == total)
                    {
                        CreateProcessSystem();
                    }
                },
                ( string error ) =>
                {
                    Debug.LogError( error );
                }
            );

            //Communication - Port
            ConfigManager.GetNestedInteger
            (
                configSystem_launcher,
                new string[ ]
                {
                    "communication",
                    "port"
                },
                ( int value ) =>
                {
                    try
                    {
                        foreach(Process process in processes)
                        {
                            process.AddArgumentKey( "port" );
                            process.AddArgumentValue( value.ToString() );
                        }

                        count++;
                        if(count == total)
                        {
                            CreateProcessSystem();
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.LogError ( e );
                    }
                },
                ( string error ) =>
                {
                    Debug.LogError( error );
                }
            );

            //For each 'app' json configuration file, there are a number of unique variables we need to pull from the specific app config json file
            foreach(ConfigManager.ConfigManagerSystem system in configSystem_apps)
            {
                Process process = null;

                //Find the proper process that matches the id of the config, so we know the config data is for this process
                foreach(Process currentProcess in processes)
                {
                    //Debug.Log( "currentProcess.id = " + currentProcess.id + " : system.id = " + system.id );
                    
                    if(currentProcess.id == system.id)
                    {
                        process = currentProcess;
                    }
                }

                //If we didn't find the proper process that matches this config system, then something is very wrong!
                if(process == null)
                {
                    Debug.LogError( "Main.cs GetVariablesFromConfig() one of our config json files has an id that doesn't match any process! Unable to continue" );
                    return;
                }

                //app/name
                ConfigManager.GetNestedString
                (
                    system,
                    new string[ ]
                    {
                    "app",
                    "name"
                    },
                    ( string value ) =>
                    {
                        //Once we have the app name, proceed to grab all of the data we need for this app and set the process accordingly
                        process.SetText( value );

                        count++;
                        if(count == total)
                        {
                            CreateProcessSystem();
                        }
                    },
                    ( string error ) =>
                    {
                        Debug.LogError( error );
                    }
                );

                //app/path
                ConfigManager.GetNestedPath
                (
                    system,
                    new string[ ]
                    {
                    "app",
                    "path"
                    },
                    ( string path ) =>
                    {
                        process.SetPath( path );

                        count++;
                        if(count == total)
                        {
                            CreateProcessSystem();
                        }
                    },
                    ( string error ) =>
                    {
                        Debug.LogError( error );
                    }
                );

                //app/length
                ConfigManager.GetNestedFloat
                (
                    system,
                    new string[ ]
                    {
                    "app",
                    "length"
                    },
                    ( float value ) =>
                    {
                        process.AddArgumentKey( "length" );
                        process.AddArgumentValue( value.ToString() );

                        count++;
                        if(count == total)
                        {
                            CreateProcessSystem();
                        }
                    },
                    ( string error ) =>
                    {
                        Debug.LogError( error );
                    }
                );

                //app/debug
                ConfigManager.GetNestedBool
                (
                    system,
                    new string[ ]
                    {
                    "app",
                    "debug"
                    },
                    ( bool value ) =>
                    {
                        process.AddArgumentKey( "debug" );
                        process.AddArgumentValue( value.ToString() );

                        count++;
                        if(count == total)
                        {
                            CreateProcessSystem();
                        }
                    },
                    ( string error ) =>
                    {
                        Debug.LogError( error );
                    }
                );

                //app/logs
                ConfigManager.GetNestedBool
                (
                    system,
                    new string[ ]
                    {
                    "app",
                    "logs"
                    },
                    ( bool value ) =>
                    {
                        process.AddArgumentKey( "logs" );
                        process.AddArgumentValue( value.ToString() );

                        count++;
                        if(count == total)
                        {
                            CreateProcessSystem();
                        }
                    },
                    ( string error ) =>
                    {
                        Debug.LogError( error );
                    }
                );

                //app/threshold
                ConfigManager.GetNestedFloat
                (
                    system,
                    new string[ ]
                    {
                    "app",
                    "threshold"
                    },
                    ( float value ) =>
                    {
                        process.AddArgumentKey( "threshold" );
                        process.AddArgumentValue( value.ToString() );

                        count++;
                        if(count == total)
                        {
                            CreateProcessSystem();
                        }
                    },
                    ( string error ) =>
                    {
                        Debug.LogError( error );
                    }
                );

            } //end of process foreach


        } //END GetVariablesFromConfig Method

        #endregion

        #region PRIVATE - CREATE PROCESS SYSTEMS

        /// <summary>
        /// After all the variables are pulled from the config, we can create the process systems
        /// </summary>
        //--------------------------------------//
        private void CreateProcessSystem()
        //--------------------------------------//
        {

            if(processes == null || ( processes != null && processes.Count == 0 ) )
            {

                Debug.LogError( "Main.cs CreateProcessSystem() processes list is null or empty. Unable to continue." );
                return;
            }

            foreach( Process process in processes )
            {
                process.CreateSystem();
            }

        } //END CreateProcessSystem Method

        #endregion

    } //END Main Class

} //END gambit.launcher Namespace