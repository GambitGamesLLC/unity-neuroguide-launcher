#region IMPORTS

using UnityEngine;


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
            
        } //END Start

        #endregion

    } //END Main Class

} //END gambit.launcher Namespace