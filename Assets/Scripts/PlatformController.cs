using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public static List<Transform> platformsTouchedWhileGoingUp; 
    
    void Awake()
    {
        if (platformsTouchedWhileGoingUp == null)
        {
            platformsTouchedWhileGoingUp = new List<Transform>();
        }
    }

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (!collider.gameObject.TryGetComponent<PlayerPlatformerController>(out var player) || player.targetVelocity.y <= 0 || platformsTouchedWhileGoingUp.Contains(transform)) return;
    //     platformsTouchedWhileGoingUp.Add(transform);
    //     Debug.Log("added");
    // }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.gameObject.TryGetComponent<LifeController>(out var lifeController) || !platformsTouchedWhileGoingUp.Contains(transform)) return;
        platformsTouchedWhileGoingUp.Remove(transform);
    }

}
