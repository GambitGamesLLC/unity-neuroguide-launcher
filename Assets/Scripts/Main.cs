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

        public float fadeInDuration;

        /// <summary>
        /// The processes handlers we pass values into from our config.json
        /// </summary>
        public List<Process> processes = new List<Process>();

        /// <summary>
        /// The names of the config files to locate in our resources folder
        /// </summary>
        private List<string> appConfigFileNames = new List<string>() 
        { 
            "app1",
            "app2"
        };

        #endregion

        #region PRIVATE - VARIABLES

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
            if(processes == null || ( processes != null && processes.Count == 0 ))
            {
                Debug.LogError( "Main.cs Start() processes handler links are not setup. Unable to continue" );
                return;
            }

            foreach( Process process in processes )
            {
                if(string.IsNullOrEmpty( process.id ))
                {
                    Debug.LogError( "Main.cs Start() process missing id, make sure each process has an id used to identify and set its values using the configuration json file" );
                    return;
                }
            }

            FadeManager.Fade( 0f, fadeInDuration );

            int count = 0;
            int total = 3;

            ConfigManager.UpdateLocalDataAndReturn
            (
                "config",
                true,

                //ON SUCCESS
                (ConfigManager.ConfigManagerSystem _system ) => 
                {
                    configSystem_launcher = _system;

                    count++;
                    if( count == total )
                    {
                        GetVariablesFromConfig();
                    }
                },

                //ON FAILED
                (string error)=>
                {
                    Debug.LogError( error );
                }
            );

            foreach( string appConfigFileName in appConfigFileNames )
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
            int count = 0;
            int total = 2 + (configSystem_apps.Count * 3);

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

            //For each 'app' json configuration file
            foreach(ConfigManager.ConfigManagerSystem system in configSystem_apps)
            {
                Process process = null;

                //Find the proper process that matches the id of the config, so we know the config data is for this process
                foreach(Process currentProcess in processes)
                {
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

            app1.CreateSystem();
            app2.CreateSystem();

        } //END CreateProcessSystem Method

        #endregion

    } //END Main Class

} //END gambit.launcher Namespace