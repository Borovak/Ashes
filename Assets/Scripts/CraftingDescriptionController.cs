using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingDescriptionController : MonoBehaviour
{
    // Start is called before the first frame update
    public static Item SelectedItem;

    public Image itemImage;
    public TextMeshProUGUI itemTitle;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemValue;
    public Image currencySymbol;
    public GameObject craftButton;

    private List<RecipeIngredient> _recipeIngredients;
    private Item _item;
    private List<KeyValuePair<Item, int>> _currentIngredients;

    void Awake()
    {
        var recipeIngredients = new List<RecipeIngredient>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            if (!t.TryGetComponent<RecipeIngredient>(out var recipeIngredient)) continue;
            recipeIngredients.Add(recipeIngredient);
        }
        _recipeIngredients = recipeIngredients.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ThenBy(x => x.transform.GetComponent<RectTransform>().anchoredPosition.x).ToList();
    }

    void Start()
    {
        InventoryItemController.SelectedItemChanged += OnSelectedItemChanged;
        RecipeSlotController.SelectedRecipeChanged += OnSelectedRecipeChanged;
        itemImage.sprite = null;
        Clear();
    }

    void OnEnable()
    {
        Clear();
        PlayerInventory.InventoryChanged += CheckIfRecipeCanBeMade;
    }

    void OnDisable()
    {
        PlayerInventory.InventoryChanged -= CheckIfRecipeCanBeMade;
        SelectedItem = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSelectedItemChanged(Item item)
    {
        OnSelectedItemChanged(item, false);
    }

    private void OnSelectedRecipeChanged(Item item)
    {
        OnSelectedItemChanged(item, true);
    }

    private void OnSelectedItemChanged(Item item, bool isRecipe)
    {
        _item = item;
        if (item == null)
        {
            Clear();
        }
        else
        {
            itemImage.sprite = item.GetArt();
            var c = itemImage.color;
            c.a = 1f;
            itemImage.color = c;
            itemTitle.text = item.name;
            itemDescription.text = item.description;
            itemValue.text = item.value.ToString();
            currencySymbol.enabled = true;
            if (isRecipe)
            {
                CheckForRecipe(item);
            }
            else
            {
                ClearIngredients();
            }
        }
    }

    private void Clear()
    {
        _item = null;
        var c = itemImage.color;
        c.a = 0f;
        itemImage.color = c;
        itemTitle.text = "";
        itemDescription.text = "";
        itemValue.text = "";
        currencySymbol.enabled = false;
        ClearIngredients();
    }

    private void ClearIngredients()
    {
        foreach (var recipeIngredient in _recipeIngredients)
        {
            recipeIngredient.Item = null;
        }
        _currentIngredients = null;
        craftButton.SetActive(false);
    }

    private void CheckForRecipe(Item item)
    {
        const string ingredientColumn = "ingredient";
        const string quantityColumn = "quantity";
        if (!item.isCraftable || !DataHandling.GetInfo($"SELECT {ingredientColumn}, {quantityColumn} FROM recipes WHERE items_id = '{item.id}'", out var dtRecipe))
        {
            ClearIngredients();
            return;
        }
        var ingredients = new Dictionary<Item, int>();
        foreach (DataRow dr in dtRecipe.Rows)
        {
            var id = int.TryParse(dr[ingredientColumn].ToString(), out var tempId) ? tempId : 0;
            var quantity = int.TryParse(dr[quantityColumn].ToString(), out var tempQuantity) ? tempQuantity : 0;
            ingredients.Add(DropController.GetDropInfo(id), quantity);
        }
        _currentIngredients = ingredients.OrderBy(x => x.Key.id).ToList();
        for (int i = 0; i < _recipeIngredients.Count; i++)
        {
            if (i < _currentIngredients.Count)
            {
                _recipeIngredients[i].Item = _currentIngredients[i].Key;
                _recipeIngredients[i].Quantity = _currentIngredients[i].Value;
            }
            else
            {
                _recipeIngredients[i].Item = null;
            }
        }
        CheckIfRecipeCanBeMade();
    }

    private void CheckIfRecipeCanBeMade()
    {
        var possible = true;
        var playerInventory = GlobalFunctions.GetPlayerInventory();
        foreach (var ingredient in _currentIngredients)
        {
            var playerQuantity = playerInventory.GetCount(ingredient.Key);
            var recipeIngredient = _recipeIngredients.First(x => x.Item == ingredient.Key);
            recipeIngredient.PlayerInventory = playerQuantity;
            if (ingredient.Value > playerQuantity)
            {
                possible = false;
            }
        }
        craftButton.SetActive(possible);
    }

    public void Craft()
    {
        if (_item == null) return;
        var playerInventory = GlobalFunctions.GetPlayerInventory();
        foreach (var ingredient in _currentIngredients)
        {
            playerInventory.Remove(ingredient.Key, ingredient.Value);
        }
        playerInventory.Add(_item);
    }

}