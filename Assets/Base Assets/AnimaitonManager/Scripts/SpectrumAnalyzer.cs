using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using Lomont;


public class SpectrumAnalyzer : MonoBehaviour
{

    public static SpectrumAnalyzer instance;
    //	LomontFFT forFFT;
    public Text subtitleText;
    List<string> lines;
    internal AudioSource audioSource;
    string subtitle;
    bool lineUpdated = true;
    int scilenceDetectedAt = 0;
    bool scilenceTimeReset = false;
    int lineIndex = 0;
    public string subtitleKey;
    internal bool animationPlaying = true;


    float[] clipSampleData = new float[1024];

    void Awake()
    {
        instance = this;
//		forFFT = GetComponent<LomontFFT> ();
        audioSource = GetComponent<AudioSource>();
    }


    void Start()
    {


        LanguageHandler.instance.OnLanguageChangeListener(subtitleText, subtitleKey);
        subtitle = subtitleText.text;

        AudioClip _Clip = Resources.Load<AudioClip>("VoiceOvers/" + LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex].LanguageID + "/" + subtitleKey);
        audioSource.clip = _Clip;


        GlobalAudioSrc.Instance.audioSrc.enabled = false;



//		Debug.Log ("audio clip name is " + audioSource.clip.name);

        audioSource.Play();

        Parse();
        subtitleText.text = lines[lineIndex++];

        for (int i = 0; i < lines.Count; i++)
            Debug.Log(lines[i]);	

    }



    void Update()
    {

        if (!animationPlaying)
            return;

        double currentAverageVol = CurrentAverageVolume();
//		double currentAverageVol = ManualDataAnalysis(audioSource.timeSamples);
//		SpecMat.color = new Color(1,1,1,1) * (currentAverageVol * 1000);
//		Debug.Log (currentAverageVol * 100000000);
//		Debug.Log(currentAverageVol* 10000000);
        if (currentAverageVol * 10000000 > 200)
        {
//			Debug.Log ("greater");
            if (scilenceTimeReset)
            {
                int scilenceGap = audioSource.timeSamples - scilenceDetectedAt;
//				Debug.Log ("silence ends at " + audioSource.timeSamples);

                if (scilenceGap > 20000 && lineIndex < lines.Count)
                {
//					Debug.Log ("Enough scilence gap " + scilenceGap);
                    subtitleText.text = lines[lineIndex++];
                }
                else if (scilenceGap > 25000)
                    subtitleText.text = "";
                scilenceTimeReset = false;
            }
				
        }
        else
        {
//			Debug.Log ("less");
            if (!scilenceTimeReset)
            {
                scilenceDetectedAt = audioSource.timeSamples;
//				Debug.Log ("silence detected at " + scilenceDetectedAt);
                scilenceTimeReset = true;
            }
        }
			
        //volume below level, but user was speaking before. So user stopped speaking
    }

	

	
    float CurrentAverageVolume()
    {
        audioSource.GetSpectrumData(clipSampleData, 0, FFTWindow.Rectangular);

        float sum = 0;
        for (int i = 0; i < 1024; i++)
            sum += clipSampleData[i];

        return sum / 1024;
    }


    void Parse()
    {
        lines = new List<string>();
        string currentLine = "";

        for (int i = 0; i < subtitle.Length; i++)
        {
            char ch = subtitle[i];
            if (LanguageHandler.instance.IsLeftToRight && ch == '<' || LanguageHandler.instance.IsRightToLeft && ch == '>')
            {
                lines.Add(currentLine);
                currentLine = "";
            }
            else if (i == subtitle.Length - 1)
            {
                currentLine += ch;
                lines.Add(currentLine);
                return;
            }
            else
                currentLine += ch;
        }

    }


    double ManualDataAnalysis(int offsetSamples)
    {
        if (offsetSamples > audioSource.clip.samples - 2048)
            return 0;
        audioSource.clip.GetData(clipSampleData, offsetSamples);

//		double[] clipSampleDataDouble = new double[clipSampleData.Length];
//		for (int i = 0; i < clipSampleData.Length; i++)
//			clipSampleDataDouble [i] = clipSampleData [i];
//
//		forFFT.FFT (clipSampleDataDouble, true);
        double sum = 0;
        for (int i = 0; i < clipSampleData.Length; i++)
            sum += clipSampleData[i] < 0 ? -clipSampleData[i] : clipSampleData[i];

        return sum / 1024;
    }


    public void LinesPassed()
    {
        audioSource.Pause();
//		audioSource.time = 12.0f;
        scilenceDetectedAt = 0;
        scilenceTimeReset = false;
        lineIndex = 0;

        audioSource.volume = audioSource.volume / 10;
        int timeSampleSet = audioSource.timeSamples;

        for (int i = 0; i < timeSampleSet / 1024; i++)
        {
            audioSource.timeSamples = i * 1024;
            double currentAverageVol = ManualDataAnalysis(i * 1024);

            //			Debug.Log ("current average volueme in lines passed " + currentAverageVol*10000000);

            if (currentAverageVol * 10000000 > 20)
            {
                Debug.Log("greater");
                if (scilenceTimeReset)
                {

                    int scilenceGap = audioSource.timeSamples - scilenceDetectedAt;

                    Debug.Log(" scilence gap " + scilenceGap);

                    if (scilenceGap > 20000 && lineIndex < lines.Count)
                    {
                        lineIndex++;
                        //						Debug.Log ("Enough scilence gap " + scilenceGap);
//						subtitleText.text = lines [lineIndex++];
                    }
                    scilenceTimeReset = false;
                }

            }
            else
            {
                Debug.Log("less");
                if (!scilenceTimeReset)
                {
                    scilenceDetectedAt = audioSource.timeSamples;
                    scilenceTimeReset = true;
                }
            }
        }
        audioSource.volume *= 10;

        subtitleText.text = lines[lineIndex++];
        Debug.Log("line index set by linepassed is " + lineIndex);
        audioSource.UnPause();

    }
		
}
