using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlighterController : MonoBehaviour
{
    [SerializeField]
    private XRSimpleInteractable simpleInteractable;

    [SerializeField]
    private MaterialSwapHighlighter highlighter;

    private void Awake()
    {
        if (!simpleInteractable) simpleInteractable = GetComponent<XRSimpleInteractable>();
        if (!highlighter) highlighter = GetComponent<MaterialSwapHighlighter>();
    }

    private void OnEnable()
    {
        simpleInteractable.hoverEntered.AddListener(OnHover);
        simpleInteractable.hoverExited.AddListener(OnHoverExit);
    }
    private void OnDisable()
    {
        simpleInteractable.hoverEntered.RemoveListener(OnHover);
        simpleInteractable.hoverExited.RemoveListener(OnHoverExit);
    }


    private void OnHover(HoverEnterEventArgs args)
    {
        highlighter.Higlight();
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        highlighter.Unhighlight();
    }
}
