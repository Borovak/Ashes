using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ShadowCastersController : MonoBehaviour
{

    public GameObject shadowCasterPrefab;
    public bool removePreviouslyGenerated = true;

    private GameOption _gameOptionShadowEnable;

    private class HorizontalGroup
    {
        internal int x;
        internal int y;
        internal int h;
    }

    public void Generate()
    {
        Debug.ClearDeveloperConsole();
        Debug.Log("Generating shadow casters");

        var chamberController = transform.parent.GetComponent<ChamberController>();

        //Get zones to cover
        var horizontalGroups = new List<HorizontalGroup>();
        for (int x = 0; x < chamberController.cellCount.x; x++)
        {
            var previousCell = 0;
            var groupStart = 0;
            for (int y = 0; y < chamberController.cellCount.y; y++)
            {
                var cell = chamberController.GetMapCell(x, y);
                if (previousCell == 0 && cell == 0) continue;
                if ((previousCell == 1 && cell == 0) || (previousCell == 1 && cell == 1 && y == chamberController.cellCount.y - 1))
                {
                    horizontalGroups.Add(new HorizontalGroup { x = x, y = groupStart, h = y - groupStart });
                }
                if (previousCell == 0 && cell == 1)
                {
                    groupStart = y;
                }
                previousCell = cell;
            }
        }
        //Remove previous shadow casters
        var itemsBefore = transform.childCount;
        if (removePreviouslyGenerated)
        {
            var security = 10000;
            while (transform.childCount > 0 && security > 0)
            {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
                security--;
            }
        }
        var itemRemovedCount = itemsBefore - transform.childCount;
        // create new ones
        var newItemCount = 0;
        foreach (var group in horizontalGroups)
        {
            var shadowCaster = GameObject.Instantiate(shadowCasterPrefab, transform);
            var x = Convert.ToSingle(group.x) * chamberController.scale + 0.25f;
            var y = Convert.ToSingle(group.y + group.h) * chamberController.scale - 0.5f;
            shadowCaster.transform.localPosition = new Vector3(x, y, 0f);
            shadowCaster.transform.localScale = new Vector3(1f, group.h, 1f);
            newItemCount++;
        }
        Debug.Log($"{itemRemovedCount} items removed, {newItemCount} items added");
    }

    void Start()
    {
        GameOptionsManager.TryGetOption(GameOptionsManager.OPTION_SHADOWS_ENABLE, out _gameOptionShadowEnable);
        _gameOptionShadowEnable.ValueChanged += OnShadowsEnableOptionChanged;
        OnShadowsEnableOptionChanged(_gameOptionShadowEnable != null ? _gameOptionShadowEnable.value : false.ToString());
    }

    private void OnShadowsEnableOptionChanged(string value)
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(value == true.ToString());
        }
    }

}
