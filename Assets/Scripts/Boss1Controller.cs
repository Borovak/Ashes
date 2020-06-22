using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : Enemy
{
    public Vector2 min = new Vector2(4, 35);
    public Vector2 max = new Vector2(46, 43);
    public float waitBeforeAttack;
    public float waitAfterAttack;
    private Transform _castPointLeft;
    private Transform _castPointRight;
    public float spellSpeed;
    public GameObject fireballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _castPointLeft = transform.Find("CastPointLeft");
        _castPointRight = transform.Find("CastPointRight");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetTeleportLocation(){
        var x = UnityEngine.Random.Range(min.x, max.x);
        var y = UnityEngine.Random.Range(min.y, max.y);
        return new Vector2(x, y);
    }

    public void CastSpell(){
        var playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        var spellOrigin = playerPosition.x < transform.position.x ? _castPointLeft : _castPointRight;
        var fireball = GameObject.Instantiate(fireballPrefab);
        fireball.transform.position = spellOrigin.position;
        var controller = fireball.GetComponent<DirectionalFireball>();
        controller.destination = playerPosition;
        controller.speed = spellSpeed;
    }
}
