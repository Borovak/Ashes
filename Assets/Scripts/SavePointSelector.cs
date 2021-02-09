using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SavePointSelector : MonoBehaviour
{
    public bool switchToThisSavePoint;

    // Update is called once per frame
    void Update()
    {
        if (!switchToThisSavePoint) return;
        switchToThisSavePoint = false;
        var guid = GetComponent<SavePointController>().guid;
        if (!SaveSystem.EditorSave(guid, out var saveErrorMessage))
        {
            Debug.Log(saveErrorMessage);
        }
    }
}
