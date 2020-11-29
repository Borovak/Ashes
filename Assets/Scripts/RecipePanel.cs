using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{
    public bool refreshNeeded = true;

    private List<RecipeSlotController> _recipeSlotControllers;

    // Start is called before the first frame update
    void Start()
    {
        var recipeSlotControllers = new List<RecipeSlotController>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            if (!t.TryGetComponent<RecipeSlotController>(out var recipeSlotController)) continue;
            recipeSlotControllers.Add(recipeSlotController);
        }
        _recipeSlotControllers = recipeSlotControllers.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ToList();
    }

    void OnEnable(){
        refreshNeeded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!refreshNeeded) return;
        refreshNeeded = false;
        var items = DropController.GetCraftables();
        //Debug.Log($"{items.Count} craftables found");
        for (int i = 0; i < _recipeSlotControllers.Count; i++)
        {
            _recipeSlotControllers[i].Item = i < items.Count ? items[i] : null;
        }
    }
}
