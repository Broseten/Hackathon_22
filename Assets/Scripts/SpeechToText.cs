using System;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityEventString : UnityEvent<string> { }

public class SpeechToText : MonoBehaviour
{
    static string YourSubscriptionKey = "9d934d02d3e24398bd3d8eb5e5fdf686";
    static string YourServiceRegion = "germanywestcentral";

    SpeechRecognizer speechRecognizer;
    SpeechConfig speechConfig;
    
    public UnityEventString OnSpeechRecognized = new UnityEventString();
    public UnityEvent OnSpeechNoMatch = new UnityEvent();
    public UnityEvent OnSpeechCanceled = new UnityEvent();

    public async void Record()
    {
        Debug.Log("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        Debug.Log(speechRecognitionResult);
        OutputSpeechRecognitionResult(speechRecognitionResult);
    }


    void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Debug.Log($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                OnSpeechRecognized.Invoke(speechRecognitionResult.Text);
                break;
            case ResultReason.NoMatch:
                Debug.Log($"NOMATCH: Speech could not be recognized.");
                OnSpeechNoMatch.Invoke();
                break;
            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Debug.Log($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Debug.Log($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Debug.Log($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Debug.Log($"CANCELED: Double check the speech resource key and region.");
                }
                OnSpeechCanceled.Invoke();
                break;
            default:
                break;
        }
    }

    void Start()
    {
        
        try
        {
        speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
        speechConfig.SpeechRecognitionLanguage = "en-US";
        
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}


