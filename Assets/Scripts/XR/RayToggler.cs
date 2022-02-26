using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Corinth.Hackathon.Gallery
{
    public class RayToggler : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference activateReference;

        [SerializeField]
        private bool enableOnActivate = true;

        [SerializeField]
        private XRRayInteractor rayInteractorToAffect;

        private bool isRayEnabled;

        private void Awake()
        {
            if (!rayInteractorToAffect) rayInteractorToAffect = GetComponent<XRRayInteractor>();
        }

        private void OnEnable()
        {
            if (activateReference)
            {
                activateReference.action.started += ReadInputPressed;
                activateReference.action.canceled += ReadInputPressed;
            }
        }

        private void OnDisable()
        {
            if (activateReference)
            {
                activateReference.action.started -= ReadInputPressed;
                activateReference.action.canceled -= ReadInputPressed;
            }
        }

        private void ReadInputPressed(InputAction.CallbackContext context)
        {
            isRayEnabled = context.control.IsPressed();
        }

        private void LateUpdate()
        {
            // Done in late update so the Select functionality (e.g. teleport) can be invoked before it gets disabled
            ApplyStatus();
        }

        private void ApplyStatus()
        {
            // can be probably simplified
            if (enableOnActivate)
            {
                if (rayInteractorToAffect.enabled != isRayEnabled)
                {
                    rayInteractorToAffect.enabled = isRayEnabled;
                }
            }
            else
            {
                if (rayInteractorToAffect.enabled == isRayEnabled)
                {
                    rayInteractorToAffect.enabled = !isRayEnabled;
                }
            }
        }

        public void ToggleRay(bool activate)
        {
            isRayEnabled = activate;
        }
    }
}
