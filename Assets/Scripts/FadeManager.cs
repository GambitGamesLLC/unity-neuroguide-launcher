#region IMPORTS

using UnityEngine;
using UnityEngine.UI;
using System;

#if GAMBIT_SINGLETON
using gambit.singleton;
#endif

#if EXT_DOTWEEN
using DG.Tweening;
#endif

#endregion

namespace gambit.launcher
{
    /// <summary>
    /// Controls the UI image used to fade in and out
    /// </summary>
    public class FadeManager: Singleton<FadeManager>
    {
        #region PUBLIC - VARIABLES
        /// <summary>
        /// The background image used for the fade in effect
        /// </summary>
        private static Image image;
        #endregion

        #region PUBLIC - AWAKE

        /// <summary>
        /// Unity lifecycle function
        /// </summary>
        //-----------------------------//
        protected override void Awake()
        //-----------------------------//
        {
            base.Awake();

#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                image = GetComponent<Image>();

                //Force the fade image to 100% transparancy
                image.color = Color.black;
            }
            catch(Exception e) 
            {
                Debug.LogError( "FadeManager.cs Awake() unable to find image component attached to this GameObject" );
            }
#pragma warning restore CS0168 // Variable is declared but never used
#pragma warning restore IDE0059 // Unnecessary assignment of a value

        } //END Awake

        #endregion

        #region PUBLIC - FADE

        /// <summary>
        /// Tweens the opacity of the fade image over time
        /// </summary>
        //--------------------------//
        public static void Fade
        (
            float opacity = 0f,
            float duration = 1f,
            Action OnComplete = null
        )
        //--------------------------//
        {
            if(image == null)
            {
                Debug.LogError( "FadeManager.cs Fade() the image component used for fading is null. Unable to continue." );
                return;
            }

#if EXT_DOTWEEN
            image.DOFade( opacity, duration ).OnComplete( ()=> { OnComplete?.Invoke(); } );
#endif

        } //END Fade Method

        #endregion

    } //END FadeManager Class

} //END gambit.launcher Namespace