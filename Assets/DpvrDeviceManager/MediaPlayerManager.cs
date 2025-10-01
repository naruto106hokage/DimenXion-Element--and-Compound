using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dpvr
{
    public partial class DpvrDeviceManager
    {
        public class MediaPlayerManager
        {
            // playMode EAC link https://blog.google/products/google-ar-vr/bringing-pixels-front-and-center-vr-video/
            public enum VideoPlayMode
            {
                Mode_2D=0,
                Mode_3DLR=1,
                Mode_3DRL=2,
                Mode_3DTB=4,
                Mode_3DBT=8,
                Mode_Pano_2D=16,
                Mode_Pano_3DLR=17,
                Mode_Pano_3DRL=18,
                Mode_Pano_3DTB=20,
                Mode_Pano_3DBT=24,
                Mode_180_2D=32,
                Mode_180_3DLR=33,
                Mode_180_3DRL=34,
                Mode_180_3DTB=36,
                Mode_180_3DBT=40,
                Mode_Fisheye_2D=64,
                Mode_Fisheye_3DLR=65,
                Mode_Fisheye_3DRL=66,
                Mode_Fisheye_3DTB=68,
                Mode_Fisheye_3DBT=72,
                Mode_EAC=128
            }

            // target package after exit playing, go to launcher if not set.
            public static void PlayVideo(string videoPath, VideoPlayMode playMode, string pkgname="")
            {
                Debug.Log("media player: path = " + videoPath + "  mode = " + ((int)playMode) + "  pkgname = " + pkgname);
                GetDeviceManager().Call("playVideo", videoPath,(int)playMode, pkgname);
            }
        }
    }
}
