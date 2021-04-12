#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ChamberController))]
    public class ChamberControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set as active"))
            {
                var chamberController = (ChamberController) target;
                var chambers = GameObject.FindGameObjectsWithTag("Chamber");
                foreach (var chamber in chambers)
                {
                    foreach (var containerName in ChamberController.ContainerNames)
                    {
                        var obj = chamber.transform.Find(containerName);
                        if (obj == null) continue;
                        obj.gameObject.SetActive(chamber.name == chamberController.name);
                    }
                }
            }
        
        }
    }
}
#endif
