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
            int total = 10;

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

            foreach(ConfigManager.ConfigManagerSystem system in configSystem_apps)
            {

            }

            //App1 - Name
            ConfigManager.GetNestedString
            (
                configSystem_app1,
                new string[ ]
                {
                    "app",
                    "name"
                },
                ( string value ) =>
                {
                    app1.SetText( value );

                    count++;
                    if( count == total )
                    {
                        CreateProcessSystem();
                    }
                },
                ( string error ) =>
                {
                    Debug.LogError( error );
                }
            );

            //App1 - Path
            ConfigManager.GetNestedPath
            (
                configSystem_app1,
                new string[ ] 
                {
                    "app",
                    "path"
                },
                (string path)=>
                {
                    app1.SetPath( path );

                    count++;
                    if(count == total)
                    {
                        CreateProcessSystem();
                    }
                },
                (string error)=>
                {
                    Debug.LogError( error );
                }
            );

            //App1 - Length
            ConfigManager.GetNestedFloat
            (
                configSystem_app1,
                new string[ ]
                {
                    "app",
                    "path"
                },
                ( float value ) =>
                {
                    app1.AddArgumentKey( "length" );
                    app1.AddArgumentValue( value.ToString() );

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


            //App2 - Name
            ConfigManager.GetNestedString
            (
                configSystem_app2,
                new string[ ]
                {
                    "app",
                    "name"
                },
                ( string name ) =>
                {
                    app2.SetText( name );

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

            //App2 - Path
            ConfigManager.GetNestedPath
            (
                configSystem_app2,
                new string[ ]
                {
                    "app",
                    "path"
                },
                ( string path ) =>
                {
                    app2.SetPath( path );

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