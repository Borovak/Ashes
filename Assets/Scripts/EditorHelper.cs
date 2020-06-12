using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class EditorHelper : MonoBehaviour
{
    [Serializable]
    public class ChamberSetting
    {
        public static ChamberSetting activeChamberSetting;
        public ChamberController chamber;
        public bool active;
        private bool _previousActive;
        private GameObject _contentFolder;

        public void Update()
        {            
            if (Application.isPlaying) return;
            if (chamber == null) return;
            Init();
            // if (_editorHelper.chamberSettings.All(x => x.active == false)){
            //     activeChamberSetting = null;
            // }
            var shouldBeActive = activeChamberSetting == this;
            if (_contentFolder.activeSelf != shouldBeActive)
            {
                _contentFolder.SetActive(shouldBeActive);
            }
            if (active == _previousActive) return;
            _previousActive = active;
            if (active)
            {
                activeChamberSetting = this;
                Debug.Log($"{chamber.gameObject.name} is now active");
                foreach (var chamberSetting in _editorHelper.chamberSettings)
                {
                    if (chamberSetting.chamber == chamber) continue;
                    chamberSetting.active = false;
                }
            }
        }

        private void Init()
        {
            if (_contentFolder == null)
            {
                _contentFolder = chamber.transform.Find("Content").gameObject;
            }
        }
    }

    public List<ChamberSetting> chamberSettings = new List<ChamberSetting>();
    public Transform chambersFolder;
    public bool wipeSaveFiles;

    private static EditorHelper _editorHelper;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) return;
        _editorHelper = this;
        foreach (var chamberSetting in chamberSettings)
        {
            chamberSetting.Update();
        }
        if (chambersFolder.childCount == chamberSettings.Count) return;
        for (int i = 0; i < chambersFolder.childCount; i++)
        {
            var t = chambersFolder.GetChild(i);
            var chamberController = t.GetComponent<ChamberController>();
            if (!chamberSettings.Any(x => x.chamber == chamberController))
            {
                chamberSettings.Add(new ChamberSetting { chamber = chamberController, active = false });
            }
        }
        for (int i = chamberSettings.Count - 1; i >= 0; i--)
        {
            var found = false;
            for (int k = 0; k < chambersFolder.childCount; k++)
            {
                if (chambersFolder.GetChild(i) == chamberSettings[i].chamber.transform)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                chamberSettings.RemoveAt(i);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Your gizmo drawing thing goes here if required...
        foreach (var chamberSetting in chamberSettings)
        {
            var w = chamberSetting.chamber.w * ChamberController.unitSize;
            var h = chamberSetting.chamber.h * ChamberController.unitSize;
            var x = chamberSetting.chamber.x * ChamberController.unitSize + w / 2f;
            var y = chamberSetting.chamber.y * ChamberController.unitSize + h / 2f;
            var colors = new Dictionary<int, Color> {
                {1, Color.green},
                {2, Color.blue}
            };
            Gizmos.color = colors.TryGetValue(chamberSetting.chamber.region, out var c) ? c : Color.magenta;
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
