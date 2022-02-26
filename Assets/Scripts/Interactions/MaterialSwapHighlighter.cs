using System;
using UnityEngine;

public class MaterialSwapHighlighter : MonoBehaviour
{
    [SerializeField]
    private Renderer rend;

    public Material HighlightMaterial;
    [Tooltip("If null, use the default that is on this gameobject.")]
    public Material IdleMaterial;

    private void Awake()
    {
        if (!rend) rend = GetComponent<Renderer>();
        if (!IdleMaterial)
        {
            IdleMaterial = rend.material;
        }
    }

    public void Higlight()
    {
        rend.material = HighlightMaterial;
    }

    public void Unhighlight()
    {
        rend.material = IdleMaterial;
    }
}
