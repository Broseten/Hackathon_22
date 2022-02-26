using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEditorButton : MonoBehaviour
{
    public UnityEvent onEditorButtonPress = new UnityEvent();

    public void Invoke()
    {
        onEditorButtonPress?.Invoke();
    }
}
