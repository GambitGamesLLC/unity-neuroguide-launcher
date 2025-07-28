#region IMPORTS

using UnityEngine;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;




#if GAMBIT_PROCESS
using gambit.process;
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

        /// <summary>
        /// The Process Manager System returned after CreateSystem()
        /// </summary>
        public ProcessManager.ProcessSystem system;

        /// <summary>
        /// Path to the process to call
        /// </summary>
        public string path;

        /// <summary>
        /// Process argument keys
        /// </summary>
        public List<string> argumentKeys;

        /// <summary>
        /// Process argument values
        /// </summary>
        public List<string> argumentValues;

        #endregion

        #region PUBLIC - START

        /// <summary>
        /// Unity lifecycle method
        /// </summary>
        //------------------------------//
        public void Start()
        //------------------------------//
        {
            CreateSystem();

        } //END Start Method

        #endregion

        #region PRIVATE - CREATE PROCESS MANAGER SYSTEM

        /// <summary>
        /// Creates a process manager system based on the variables set in the editor
        /// </summary>
        //-----------------------------//
        private void CreateSystem()
        //-----------------------------//
        {
            ProcessManager.Options options = new ProcessManager.Options();
            options.path = path;
            options.argumentKeys = argumentKeys;
            options.showDebugLogs = true;

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
                        FadeManager.Fade( 1f, 3f );
                    }
                    else if(state == ProcessManager.State.NotRunning)
                    {
                        FadeManager.Fade( 0f, 3f );
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
                    FadeManager.Fade( 1f, 3f );
                },

                //ON LAUNCH FAILED
                (string error)=>
                {
                    Debug.Log(error);
                }
            );

        } //END Launch

        #endregion

    } //END Process Class

} //END gambit.launcher Namespace