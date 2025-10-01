/************************************************************************************

Copyright   :   Copyright 2015 DeePoon LLC. All Rights reserved.

DPVR Developer Website: http://developer.dpvr.cn

************************************************************************************/

using UnityEngine;
using System.Collections;
#if UNITY_5_3_OR_NEWER || UNITY_5_3
using UnityEngine.SceneManagement;
#endif

namespace dpn
{
    public class QuitGame : MonoBehaviour {

        public KeyCode quit = KeyCode.Escape;

        public KeyCode loadNextScene = KeyCode.N;

        // Update is called once per frame
        void Update () {
            if( Input.GetKeyDown( quit ) )
            {
                Application.Quit();
            }
        }
    }
}
