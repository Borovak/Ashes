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
    public Transform currentWaypoint;
    public Transform[] waypoints;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private LifeController _lifeController;
    private Transform _playerTransform;
    private float _waitTimer;
    private bool _shouldDecideUpDown;
    private float _wakeUpTimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = waypoints[0];
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lifeController = GetComponent<LifeController>();
        //_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform == null)
        {
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            if (playerGameObject == null) return;
            _playerTransform = playerGameObject.transform;
        }
        delay = Mathf.Lerp(maxDelay, minDelay, _lifeController.HealthRatio);
        speed = Mathf.Lerp(maxSpeed, minSpeed, _lifeController.HealthRatio); 
        _animator.SetFloat("distanceFromPlayer", Vector3.Distance(_playerTransform.position, transform.position));
    }
}
