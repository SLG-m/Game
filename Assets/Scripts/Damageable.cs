using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;

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

    //public bool IsHit
    //{
    //    get
    //    {
    //        return animator.GetBool(AnimationStrings.isHit);
    //    }
    //    private set
    //    {
    //        animator.SetBool(AnimationStrings.isHit, value);
    //    }
    //}


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

            if (value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
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
                // убираем неуязвимость
                isInvincible = false;
                timerSinceHit = 0;
            }
            timerSinceHit += Time.deltaTime;
        }
    }

    // возвращаем значение был ли персонаж поражен
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        // цель не поражена
        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);

            int actualHeal = Mathf.Min(maxHeal, healthRestore);

            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, actualHeal);

            return true;    
        }

        return false;
    }
}
