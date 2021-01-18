using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPlayerContact : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.TryGetComponent<LifeController>(out var lifeController)) return;
        GameObject.Destroy(gameObject);
    }
}
