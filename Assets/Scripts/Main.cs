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
        /// Config Manager System for the launcher config file
        /// </summary>
        private ConfigManager.ConfigManagerSystem configSystem_launcher;

        /// <summary>
        /// Config Manager System for the app1 config file
        /// </summary>
        private ConfigManager.ConfigManagerSystem configSystem_app1;

        /// <summary>
        /// Config Manager System for the app2 config file
        /// </summary>
        private ConfigManager.ConfigManagerSystem configSystem_app2;

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

            ConfigManager.UpdateLocalDataAndReturn
            (
                "app1",
                true,

                //ON SUCCESS
                ( ConfigManager.ConfigManagerSystem _system ) =>
                {
                    configSystem_app1 = _system;

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

            ConfigManager.UpdateLocalDataAndReturn
            (
                "app2",
                true,

                //ON SUCCESS
                ( ConfigManager.ConfigManagerSystem _system ) =>
                {
                    configSystem_app2 = _system;

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
                    //Debug.Log( value );
                    app1.AddArgumentKey( "address" );
                    app1.AddArgumentValue( value );

                    app2.AddArgumentKey( "address" );
                    app2.AddArgumentValue( value );

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
                        //Debug.Log( value );
                        app1.AddArgumentKey( "port" );
                        app1.AddArgumentValue( value.ToString() );

                        app2.AddArgumentKey( "port" );
                        app2.AddArgumentValue( value.ToString() );

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
            
            //App1 - Argument Keys
            ConfigManager.GetNestedString
            (
                configSystem_app1,
                new string[ ]
                {
                    "app",
                    "argumentKeys"
                },
                ( string value ) =>
                {
                    if( !string.IsNullOrEmpty( value ) )
                    {
                        //Split the comma deliminated string into a List<string>
                        List<string> keys = value.Split( ',' ).ToList();
                        app1.AddArgumentKey( keys );
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

            //App1 - Argument Values
            ConfigManager.GetNestedString
            (
                configSystem_app1,
                new string[ ]
                {
                    "app",
                    "argumentValues"
                },
                ( string value ) =>
                {
                    if(!string.IsNullOrEmpty( value ))
                    {
                        //Split the comma deliminated string into a List<string>
                        List<string> values = value.Split( ',' ).ToList();
                        app1.AddArgumentValue( values );
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
            
            //App2 - Argument Keys
            ConfigManager.GetNestedString
            (
                configSystem_app2,
                new string[ ]
                {
                    "app",
                    "argumentKeys"
                },
                ( string value ) =>
                {
                    if(!string.IsNullOrEmpty( value ))
                    {
                        //Split the comma deliminated string into a List<string>
                        List<string> keys = value.Split( ',' ).ToList();
                        app2.AddArgumentKey( keys );
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

            //App2 - Argument Values
            ConfigManager.GetNestedString
            (
                configSystem_app2,
                new string[ ]
                {
                    "app",
                    "argumentValues"
                },
                ( string value ) =>
                {
                    if(!string.IsNullOrEmpty( value ))
                    {
                        //Split the comma deliminated string into a List<string>
                        List<string> values = value.Split( ',' ).ToList();
                        app2.AddArgumentValue( values );
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