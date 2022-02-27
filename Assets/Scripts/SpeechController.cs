using UnityEngine;
using UnityEngine.InputSystem;

public class SpeechController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference startMicButton;

    [SerializeField]
    private SpeechToText stt;

    private void OnEnable()
    {
        startMicButton.action.performed += OnStartMic;
    }

    private void OnDisable()
    {
        startMicButton.action.performed -= OnStartMic;
    }

    private void OnStartMic(InputAction.CallbackContext ctx)
    {
        stt.Record();
    }
}
