using UnityEngine;
using System.Collections.Generic;

//[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointRanchedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;


    Animator animator;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        { 
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
            }
        }
        //else
        //{
        //    rb.gravityScale = 2f;
        //    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //    deathCollider.enabled = true;
        //}
    }

    private void Flight()
    { 
        // лететь к следующей точке
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // проверка растояния до точки
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.linearVelocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        //готовы ли сменить точку перемещения
        if (distance <= waypointRanchedDistance)
        {
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                // возвращение к исходной точке
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }

    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if (transform.localScale.x > 0)
        {
            // смотрит на право
            if (rb.linearVelocity.x < 0)
            {
                // переворот
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            // смотрит на лево\
            if (rb.linearVelocity.x > 0)
            {
                // переворот
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        deathCollider.enabled = true;
    }
}
