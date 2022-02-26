using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GenericEditorButton))]
[CanEditMultipleObjects]
public class GenericEditorButtonEditor : Editor
{
    GenericEditorButton myScript;
    private void OnEnable()
    {
        myScript = (GenericEditorButton)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Invoke Events"))
        {
            myScript.Invoke();
        }
    }
}