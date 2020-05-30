using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDownWrecker : MonoBehaviour
{

    private enum States
    {
        Idle,
        GoingDown,
        GoingUp
    }

    public float vision;
    public float speedUp;
    public float speedDown;
    public LayerMask whatIsLayout;
    public LayerMask whatIsPlayer;
    private float _width;
    private float _height;
    private float _yWhenUp;
    private float _yWhenDown = float.MinValue;
    private States state;
    private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        var spriteRenderer = GetComponent<SpriteRenderer>();
        _width = spriteRenderer.bounds.size.x;
        _height = spriteRenderer.bounds.size.y;
        _yWhenUp = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        var currentPosition = new Vector2(transform.position.x, transform.position.y);
        var biasCheck = Vector2.right * (_width / 2f);
        if (_yWhenDown == float.MinValue)
        {
            var hitLeftLayout = Physics2D.Raycast(currentPosition - biasCheck, Vector2.down, vision, whatIsLayout);
            var hitRightLayout = Physics2D.Raycast(currentPosition + biasCheck, Vector2.down, vision, whatIsLayout);
            _yWhenDown = (hitLeftLayout.point.y + hitRightLayout.point.y) / 2f + (_height / 2f);
        }
        float newY;
        switch (state)
        {
            case States.Idle:
                var hitLeft = Physics2D.Raycast(currentPosition - biasCheck, Vector2.down, vision, whatIsPlayer);
                var hitRight = Physics2D.Raycast(currentPosition + biasCheck, Vector2.down, vision, whatIsPlayer);
                if (hitLeft.collider == null && hitRight.collider == null) break;
                state = States.GoingDown;
                _animator.SetBool("attack", true);
                break;
            case States.GoingDown:
                newY = transform.position.y - (speedDown * Time.deltaTime);
                newY = Mathf.Max(newY, _yWhenDown);
                transform.position = new Vector2(transform.position.x, newY);
                if (Mathf.Abs(_yWhenDown - newY) < 0.0001f){
                    state = States.GoingUp;
                    _animator.SetBool("attack", false);
                }
                break;
            case States.GoingUp:
                newY = transform.position.y + (speedUp * Time.deltaTime);
                newY = Mathf.Min(newY, _yWhenUp);
                transform.position = new Vector2(transform.position.x, newY);
                if (Mathf.Abs(_yWhenUp - newY) < 0.0001f){
                    state = States.Idle;
                }
                break;
        }
    }
}
