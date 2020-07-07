using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class EditorHelper : MonoBehaviour
{
    public Transform chambersFolder;
    public bool wipeSaveFiles;

    private static EditorHelper _editorHelper;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) return;
        _editorHelper = this;
    }

    void OnDrawGizmos()
    {        
        for (int i = 0; i < chambersFolder.childCount; i++)
        {
            if (!chambersFolder.GetChild(i).TryGetComponent<ChamberController>(out var chamberController)) continue;
            var w = chamberController.w * ChamberController.unitSize;
            var h = chamberController.h * ChamberController.unitSize;
            var x = chamberController.x * ChamberController.unitSize + w / 2f;
            var y = chamberController.y * ChamberController.unitSize + h / 2f;
            var colors = new Dictionary<int, Color> {
                {1, Color.green},
                {2, Color.red},
                {3, Color.blue}
            };
            Gizmos.color = colors.TryGetValue(chamberController.region, out var c) ? c : Color.magenta;
            Gizmos.DrawWireCube(new Vector3(x, y, 0f), new Vector3(w, h, 1f));
        }
#if UNITY_EDITOR
        // Ensure continuous Update calls.
        if (Application.isPlaying) return;
        UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
        UnityEditor.SceneView.RepaintAll();
        if (wipeSaveFiles){
            wipeSaveFiles = false;  
            SaveSystem.WipeFiles();          
        }
#endif
    }
}
