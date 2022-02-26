using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine;
using UnityEngine.UI;



public class TextToSpeech : MonoBehaviour
{
    public GameObject krychle;
    public Text outputText;

    static string YourSubscriptionKey = "9d934d02d3e24398bd3d8eb5e5fdf686";
    static string YourServiceRegion = "germanywestcentral";
    
    SpeechRecognizer speechRecognizer;
    SpeechConfig speechConfig;

    private object threadLocker = new object();
    private bool speechStarted = false; //checking to see if you have started listening for speech
    private string message;

    private bool micPermissionGranted = false;

    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        lock (threadLocker)
        {
            message = e.Result.Text;
        }
    }

    public async void ButtonClick()
    {
        if (speechStarted)
        {
            await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false); // this stops the listening when you click the button, if it's already on
            lock (threadLocker)
            {
                speechStarted = false;
            }
        }
        else
        {
            await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false); // this will start the listening when you click the button, if it's already off
            lock (threadLocker)
            {
                speechStarted = true;
            }
        }

    }

    public async void Record()
    {
        Console.WriteLine("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        OutputSpeechRecognitionResult(speechRecognitionResult);
    }


    static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                break;
            case ResultReason.NoMatch:
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                break;
            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Double check the speech resource key and region.");
                }
                break;
        }
    }

    async void Start()
    {
        speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
        speechConfig.SpeechRecognitionLanguage = "en-US";

        //using var audioConfig = AudioConfig.FromWavFileInput("YourAudioFile.wav");
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
    }

    void Update()
    {

        lock (threadLocker)
        {
            if (outputText != null)
            {
                outputText.text = message;
            }
        }
    }
}


