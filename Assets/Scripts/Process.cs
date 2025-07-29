#region IMPORTS

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;

#if GAMBIT_PROCESS
using gambit.process;
#endif

#if GAMBIT_CONFIG
using gambit.config;
#endif

#endregion

namespace gambit.launcher
{

    /// <summary>
    /// Launches a process based on the data fields set in the editor
    /// </summary>
    public class Process: MonoBehaviour
    {
        #region PUBLIC - VARIABLES

        public TextMeshProUGUI text;

        /// <summary>
        /// The Process Manager System returned after CreateSystem()
        /// </summary>
        public ProcessManager.ProcessSystem system;

        /// <summary>
        /// Path to the process to call
        /// </summary>
        private string path;

        /// <summary>
        /// Process argument keys
        /// </summary>
        private List<string> argumentKeys = new List<string>();

        /// <summary>
        /// Process argument values
        /// </summary>
        private List<string> argumentValues = new List<string>();

        /// <summary>
        /// The duration of the fade in tween used when the process starts
        /// </summary>
        public float fadeInDuration;

        /// <summary>
        /// The duration of the fade out tween used when the process ends
        /// </summary>
        public float fadeOutDuration;

        public List<Button> buttonsToSetInteractable;

        #endregion

        #region PUBLIC - CREATE PROCESS MANAGER SYSTEM

        /// <summary>
        /// Creates a process manager system based on the variables set in the editor
        /// </summary>
        //-----------------------------//
        public void CreateSystem()
        //-----------------------------//
        {
            path = ConfigManager.UnescapeAndExpandPath( path );

            ProcessManager.Options options = new ProcessManager.Options();
            options.path = path;
            options.argumentKeys = argumentKeys;
            options.argumentValues = argumentValues;
            options.showDebugLogs = false;

            ProcessManager.Create
            (
                options,

                //ON CREATE SUCCESS
                (ProcessManager.ProcessSystem _system) =>
                {
                    system = _system;
                },

                //ON CREATE FAILED
                (string error ) =>
                {
                    Debug.LogError( error );
                },

                //ON STATE CHANGED
                ( ProcessManager.ProcessSystem _system, ProcessManager.State state)=>
                {
                    if(state == ProcessManager.State.Running)
                    {
                        FadeManager.Fade( 1f, fadeInDuration );
                        SetButtons( false );
                    }
                    else if(state == ProcessManager.State.NotRunning)
                    {
                        FadeManager.Fade( 0f, fadeOutDuration );
                        SetButtons( true );
                    }
                }
            );
        
        } //END CreateSystem Method

        #endregion

        #region PUBLIC - LAUNCH

        /// <summary>
        /// Launches the process
        /// </summary>
        //---------------------------------//
        public void Launch()
        //---------------------------------//
        {
            if(system == null)
            {
                Debug.LogError( "Process.cs Launch() system is null. Unable to continue" );
                return;
            }

            Debug.Log( "----------------------------------------- \n -------------------------------------------------" );
            foreach(string key in system.options.argumentKeys)
            {
                Debug.Log( "key: " + key );
            }
            Debug.Log( "----------------------------------------- \n -------------------------------------------------" );
            foreach(string value in system.options.argumentValues)
            {
                Debug.Log( "value: " + value );
            }
            Debug.Log( "----------------------------------------- \n -------------------------------------------------" );

            ProcessManager.LaunchProcess
            ( 
                system,

                //ON LAUNCH SUCCESS
                ()=>
                {
                    Debug.Log( "Process.cs Launch() Process launched successfully" );
                },

                //ON LAUNCH FAILED
                (string error)=>
                {
                    Debug.Log(error);
                }
            );

        } //END Launch

        #endregion

        #region PRIVATE - SET BUTTONS

        /// <summary>
        /// Sets the buttons as interactable or disabled
        /// </summary>
        /// <param name="active"></param>
        //----------------------------------------------------//
        private void SetButtons( bool active )
        //----------------------------------------------------//
        {
            if(buttonsToSetInteractable != null && buttonsToSetInteractable.Count > 0)
            {
                foreach(Button button in buttonsToSetInteractable)
                {
                    button.interactable = active;
                }
            }
        
        } //END SetButtons

        #endregion

        #region PRIVATE - SET TEXT

        /// <summary>
        /// Sets the text property of the button
        /// </summary>
        /// <param name="_text"></param>
        //--------------------------------------//
        public void SetText( string _text )
        //--------------------------------------//
        {
            if(text == null)
            {
                Debug.LogError( "Process.cs SetText() TextMeshPro 'text' component is null, unable to continue" );
                return;
            }

            text.SetText( _text );

        } //END SetText Method

        #endregion

        #region PRIVATE - SET PATH

        /// <summary>
        /// Sets the path of the process
        /// </summary>
        /// <param name="value"></param>
        //--------------------------------------//
        public void SetPath( string value )
        //--------------------------------------//
        {
            path = value;

        } //END SetPath Method

        #endregion

        #region PRIVATE - ADD ARGUMENT KEY

        /// <summary>
        /// Adds a argument key passed in when launching the process
        /// </summary>
        //--------------------------------------//
        public void AddArgumentKey( List<string> keys )
        //--------------------------------------//
        {
            if( keys == null || ( keys != null && keys.Count == 0 ) )
            {
                return;
            }

            foreach(string key in keys)
            {
                AddArgumentKey( key );
            }

        } //END AddArgumentKey Method

        /// <summary>
        /// Adds a argument key passed in when launching the process
        /// </summary>
        //--------------------------------------//
        public void AddArgumentKey( string key )
        //--------------------------------------//
        {
            argumentKeys.Add( key );

        } //END AddArgumentKey Method

        #endregion

        #region PRIVATE - ADD ARGUMENT VALUE

        /// <summary>
        /// Adds a argument value passed in when launching the process
        /// </summary>
        //--------------------------------------//
        public void AddArgumentValue( List<string> values )
        //--------------------------------------//
        {
            if(values == null || (values != null && values.Count == 0))
            {
                return;
            }

            foreach(string value in values)
            {
                AddArgumentValue( value );
            }

        } //END AddArgumentValue Method

        /// <summary>
        /// Adds a argument value passed in when launching the process
        /// </summary>
        //--------------------------------------//
        public void AddArgumentValue( string value )
        //--------------------------------------//
        {
            argumentValues.Add( value );

        } //END AddArgumentValue Method

        #endregion

        #region PRIVATE - SET ARGUMENT KEYS

        /// <summary>
        /// Sets the argument keys passed in when launching the process
        /// </summary>
        /// <param name="_text"></param>
        //--------------------------------------//
        public void SetArgumentKeys( List<string> keys )
        //--------------------------------------//
        {
            argumentKeys = keys;

        } //END SetArgumentKeys Method

        #endregion

        #region PRIVATE - SET ARGUMENT VALUES

        /// <summary>
        /// Sets the argument values passed in when launching the process
        /// </summary>
        /// <param name="_text"></param>
        //--------------------------------------//
        public void SetArgumentValues( List<string> values )
        //--------------------------------------//
        {
            argumentValues = values;

        } //END SetArgumentValues Method

        #endregion

    } //END Process Class

} //END gambit.launcher Namespace