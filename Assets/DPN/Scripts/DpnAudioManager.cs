
using UnityEngine;

namespace dpn
{
    class AndroidAudioManager
    {
        static AndroidAudioManager instance;

        static public AndroidAudioManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AndroidAudioManager();
                return instance;
            }
        }

        private static AndroidJavaObject audioManager;

        AndroidAudioManager()
        {
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            audioManager = currentActivity.Call<AndroidJavaObject>("getSystemService", new AndroidJavaObject("java.lang.String", "audio"));
        }

        public int GetStreamVolume(int streamType)
        {
            return audioManager.Call<int>("getStreamVolume", streamType);
        }

        public void SetStreamVolume(int streamType, int volume)
        {
            object[] @params =
            {
                streamType,
                volume,
                0
            };

            audioManager.Call("setStreamVolume", @params);
        }

        public void AdjustStreamVolume(int streamType, int adjustDirction)
        {
            object[] @params =
            {
                streamType,
                adjustDirction,
                0
            };

            audioManager.Call("adjustStreamVolume", @params);
        }
    }

    public enum StreamType
    {
        STREAM_VOICE_CALL = 0,
        STREAM_SYSTEM = 1,
        STREAM_RING = 2,
        STREAM_MUSIC = 3,
        STREAM_ALARM = 4,
        STREAM_NOTIFICATION = 5,
        STREAM_DTMF = 8,
    }

    public enum AdjustDirction
    {
        ADJUST_LOWER,
        ADJUST_RAISE,
        ADJUST_SAME,
    }

    public class DpnAudioManager : MonoBehaviour
    {
        /// <summary>
        /// music volume range [0, 15]
        /// </summary>
        static public int MusicVolume
        {
            set
            {
                AndroidAudioManager.Instance.SetStreamVolume((int)StreamType.STREAM_MUSIC, Mathf.Clamp(value, 0, 15));
            }
            get
            {
                return AndroidAudioManager.Instance.GetStreamVolume((int)StreamType.STREAM_MUSIC);
            }
        }

        static public void AdjustMusicVolume(AdjustDirction adjustDirction)
        {
            AndroidAudioManager.Instance.AdjustStreamVolume((int)StreamType.STREAM_MUSIC, (int)adjustDirction);
        }
    }

}
