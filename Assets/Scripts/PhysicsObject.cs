using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsObject : MonoBehaviour
{

    public static bool PhysicsEnabled = true;
    public float minGroundNormalY = .65f;
    public float gravityModifier = 3f;
    public float distance;
    public int hitBufferCount;
    public abstract bool canFly { get; }
    public bool isGravityEnabled = true;

    public Vector2 targetVelocity;
    protected bool grounded;
    protected Transform elevator;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected bool droppingThrough;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    protected GameController _gameController;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        if (!PhysicsObject.PhysicsEnabled) return;
        if (isGravityEnabled)
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }
        velocity.x = targetVelocity.x;

        grounded = false;
        elevator = null;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = canFly ? Vector2.right * deltaPosition.x : moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            hitBufferCount = hitBufferList.Count;
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                if (hitBufferList[i].transform.tag == "Platform" && (droppingThrough || PlatformController.platformsTouchedWhileGoingUp.Contains(hitBufferList[i].transform) || velocity.y > 0 || move.y > 0)) 
                {
                    if (!PlatformController.platformsTouchedWhileGoingUp.Contains(hitBufferList[i].transform))
                    {
                        PlatformController.platformsTouchedWhileGoingUp.Add(hitBufferList[i].transform);
                    }
                    continue;
                } else if (hitBufferList[i].transform.tag == "Interaction") continue;
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

}
