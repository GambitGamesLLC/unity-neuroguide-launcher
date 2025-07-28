#region IMPORTS

using UnityEngine;

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

        public ProcessManager.ProcessSystem system;

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
            ProcessManager.Create
            (

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