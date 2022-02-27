using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine;
using UnityEngine.UI;


public class SpeechToText : MonoBehaviour
{ 
    public Text usersInput;

    static string YourSubscriptionKey = "9d934d02d3e24398bd3d8eb5e5fdf686";
    static string YourServiceRegion = "germanywestcentral";

    public object[] artWorks;

    HttpClient client = new HttpClient();
    SpeechRecognizer speechRecognizer;
    SpeechConfig speechConfig;

    private object threadLocker = new object();
    private bool speechStarted = false; //checking to see if you have started listening for speech
    public string message;

    private bool micPermissionGranted = false;

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
                usersInput.text = message = speechRecognitionResult.Text;
                //artWorks = client.Query(message);
                Debug.Log(artWorks);
                break;
            case ResultReason.NoMatch:
                Debug.Log($"NOMATCH: Speech could not be recognized.");
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
                break;
        }
    }

    async void Start()
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


