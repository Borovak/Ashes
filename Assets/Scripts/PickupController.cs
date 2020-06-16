using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public enum Pickups
    {
        DoubleJump
    }

    public Pickups pickup;

    private bool _pickupState
    {
        get => new Dictionary<Pickups, bool>
        {
            {Pickups.DoubleJump, PlayerPlatformerController.Instance.hasDoubleJump}
        }[pickup];
        set
        {
        new Dictionary<Pickups, System.Action<bool>>
        {
            {Pickups.DoubleJump, value => PlayerPlatformerController.Instance.hasDoubleJump = value}
        }[pickup].Invoke(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_pickupState)
        {
            GameObject.Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Pickup touched");
        _pickupState = true;
        GameObject.Destroy(gameObject);
    }
}
