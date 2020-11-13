using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int id;
    public Vector2 spawnForce;


    private Rigidbody2D _rigidBody2D;
    private bool _forceApplied;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        var forceX = UnityEngine.Random.Range(0.5f, 1f) * 200f * (UnityEngine.Random.Range(0f, 1f) >= 0.5f ? 1f : -1f); 
        spawnForce = new Vector2(forceX, 50f); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if (_forceApplied) return;
       _forceApplied = true;
        _rigidBody2D.AddForce(spawnForce);
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        Debug.Log($"{gameObject.name} touched by {collider2D.gameObject.name}");
        if (!collider2D.gameObject.TryGetComponent<PlayerInventory>(out var inventory)) return;
        inventory.Add(id);
        GameObject.Destroy(gameObject);
    }
}
