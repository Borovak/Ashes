#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SavePointController))]
    public class SavePointControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set as active"))
            {
                var savePointController = (SavePointController) target;
                SaveSystem.EditorSave(savePointController.guid, out _);
            }
        
        }
    }
}
#endif
