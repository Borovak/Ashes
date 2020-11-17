using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWolfController : MonoBehaviour
{
    public enum States
    {
        Asleep,
        WakingUp,
        Normal
    }

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float playerYThreshold;
    public float minDelay;
    public float maxDelay;
    public float delay;
    public float minSpeed;
    public float maxSpeed;
    public float speed;
    public float positionSpeed;
    public float positionRatio;
    public AnimationCurve speedCurve;
    public AnimationCurve jumpCurve;
    public Rect wakesUpWhenPlayerIsInRectangle;
    public bool goingLeft;
    public bool goingUp;
    public float wakeUpTimer;
    public States state;

    private SpriteRenderer _spriteRenderer;
    private LifeController _lifeController;
    private Transform _playerTransform;
    private float _waitTimer;
    private bool _shouldDecideUpDown;
    private float _wakeUpTimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lifeController = GetComponent<LifeController>();
        //_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        switch (state)
        {
            case States.Asleep:
                if (_playerTransform.position.x < wakesUpWhenPlayerIsInRectangle.x 
                    || _playerTransform.position.x > wakesUpWhenPlayerIsInRectangle.x + wakesUpWhenPlayerIsInRectangle.width
                    || _playerTransform.position.y < wakesUpWhenPlayerIsInRectangle.y 
                    || _playerTransform.position.y > wakesUpWhenPlayerIsInRectangle.y + wakesUpWhenPlayerIsInRectangle.height) return;
                state = States.WakingUp;
                _wakeUpTimeRemaining = wakeUpTimer;
                break;
            case States.WakingUp:
                _wakeUpTimeRemaining -= Time.deltaTime;
                if (_wakeUpTimeRemaining > 0) return;
                state = States.Normal;
                break;
        }
        delay = Mathf.Lerp(maxDelay, minDelay, _lifeController.HealthRatio);
        speed = Mathf.Lerp(maxSpeed, minSpeed, _lifeController.HealthRatio);
        //Check if flip needed
        if ((goingLeft && transform.position.x < minX) || (!goingLeft && transform.position.x > maxX))
        {
            goingLeft = !goingLeft;
            _waitTimer = delay;
            _shouldDecideUpDown = true;
        }
        if (_waitTimer > 0)
        {
            _waitTimer -= Time.deltaTime;
        }
        else if (_shouldDecideUpDown)
        {
            goingUp = _playerTransform.position.y > playerYThreshold;
            _shouldDecideUpDown = false;
        }
        else
        {
            //x
            var x = (transform.position.x - minX) / (maxX - minX);
            positionRatio = goingLeft ? 1f - x : x;
            positionSpeed = speed * speedCurve.Evaluate(x);
            var move = positionSpeed * Time.deltaTime * (goingLeft ? -1f : 1f);
            //y
            var y = goingUp ? (maxY - minY) * jumpCurve.Evaluate(positionRatio) + minY : minY;
            //move
            transform.position = new Vector3(transform.position.x + move, y, 0f);
        }
        _spriteRenderer.flipX = goingLeft;
    }
}
