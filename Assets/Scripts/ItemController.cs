using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        Debug.Log($"{gameObject.name} touched by {collider2D.gameObject.name}");
        if (!collider2D.gameObject.TryGetComponent<PlayerLifeController>(out _)) return;
        Inventory.Add(id);
        GameObject.Destroy(gameObject);
    }
}
