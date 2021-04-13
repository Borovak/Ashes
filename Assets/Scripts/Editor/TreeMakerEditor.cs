#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TreeMaker))]
    public class TreeMakerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var treeMaker = (TreeMaker) target;
            if (GUILayout.Button("Reset orders in layer"))
            {
                treeMaker.Reorder();
            }
            if (GUILayout.Button("Regenerate"))
            {
                treeMaker.Regenerate();
            }
            if (GUILayout.Button("Duplicate"))
            {
                treeMaker.Duplicate();
            }
        }
    }
}
#endif