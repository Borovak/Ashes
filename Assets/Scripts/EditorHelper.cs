using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class EditorHelper : MonoBehaviour
{
    public Transform chambersFolder;

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
    }
}
