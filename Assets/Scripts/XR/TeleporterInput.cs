using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Corinth.Hackathon.Gallery
{
    [RequireComponent(typeof(XRRayInteractor))]
    public class TeleporterInput : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference selectReference;
        [SerializeField]
        private InputActionReference activateReference;
        [SerializeField]
        private InputActionReference cancelReference;

        private XRRayInteractor rayInteractor;

        private bool selected = false;

        private void Awake()
        {
            rayInteractor = GetComponent<XRRayInteractor>();

            if ((rayInteractor.xrController as ActionBasedController).selectAction.reference != activateReference)
            {
                Debug.LogError($"The {nameof(TeleporterInput)} uses a different action to interact with the teleportation layer.");
            }
        }

        private void OnEnable()
        {
            selectReference.action.performed += OnSelect;
            activateReference.action.performed += OnActivate;
            cancelReference.action.performed += OnCancel;
        }
        private void OnDisable()
        {
            selectReference.action.performed -= OnSelect;
            activateReference.action.performed -= OnActivate;
            cancelReference.action.performed -= OnCancel;
        }


        private void OnSelect(InputAction.CallbackContext context)
        {
            Debug.Log("Select");
            selected = true;
        }

        private void OnActivate(InputAction.CallbackContext context)
        {
            Debug.Log("Activate");
            rayInteractor.allowSelect = false;
            Stop();
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            Debug.Log("Cancel");
            Stop();
        }

        private void Stop()
        {
            selected = false;
        }

        private void LateUpdate()
        {
            // late update after all interactors are processed
            ApplyState();

            rayInteractor.allowSelect = true;
        }

        private void ApplyState()
        {
            if (selected != rayInteractor.enabled)
            {
                // disable or enable ray based on the input
                rayInteractor.enabled = selected;
            }
        }
    }
}
