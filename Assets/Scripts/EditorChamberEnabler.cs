using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorChamberEnabler : MonoBehaviour
{
    public bool makeActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!makeActive) return;
        makeActive = false;
        var chambers = GameObject.FindGameObjectsWithTag("Chamber");
        foreach (var chamber in chambers)
        {
            if (!chamber.TryGetComponent<EditorChamberEnabler>(out _))
            {
                chamber.AddComponent<EditorChamberEnabler>();
            }
            foreach (var containerName in ChamberController.ContainerNames)
            {
                var obj = chamber.transform.Find(containerName);
                if (obj == null) continue;
                obj.gameObject.SetActive(chamber.name == gameObject.name);
            }
        }
    }
}
