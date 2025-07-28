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

        public TextMeshPro text;

        /// <summary>
        /// The Process Manager System returned after CreateSystem()
        /// </summary>
        public ProcessManager.ProcessSystem system;

        /// <summary>
        /// Path to the process to call
        /// </summary>
        [NonSerialized]
        public string path;

        /// <summary>
        /// Process argument keys
        /// </summary>
        public List<string> argumentKeys;

        /// <summary>
        /// Process argument values
        /// </summary>
        public List<string> argumentValues;

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

        #region PUBLIC - START

        /// <summary>
        /// Unity lifecycle method
        /// </summary>
        //------------------------------//
        public void Start()
        //------------------------------//
        {
            //CreateSystem();

        } //END Start Method

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
            text.SetText( _text );

        } //END SetText Method

        #endregion

    } //END Process Class

} //END gambit.launcher Namespace