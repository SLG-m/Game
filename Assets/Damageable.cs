using UnityEngine;

public class Damageable : MonoBehaviour
{
    Animator animator;


    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set 
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        { 
            _health = value;

            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;
    private float timerSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value); 
            Debug.Log("IsAlive set" + value);
        }
    }

    private void Awake()
    {
       // rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //touchingDirections = GetComponent<TouchingDirections>();
    }

    void Update()
    {
        if (isInvincible)
        {
            if (timerSinceHit > invincibilityTime)
            {
                // убираем неу€звимость
                isInvincible = false;
                timerSinceHit = 0;
            }
            timerSinceHit += Time.deltaTime;
        }
        Hit(10);
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
        }
    }
}
