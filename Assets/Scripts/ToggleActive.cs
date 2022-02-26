using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    [SerializeField]
    private bool activated = true;

    public GameObject onWhenActive;
    public GameObject offWhenActive;

    private void Awake()
    {
        gameObject.SetActive(activated);
    }

    public void Toggle(bool active)
    {
        activated = active;
        onWhenActive?.SetActive(active);
        offWhenActive?.SetActive(!active);
    }

    public void Toggle()
    {
        Toggle(!activated);
    }
}
