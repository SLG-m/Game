using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> HealthChanged;

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
            HealthChanged?.Invoke(_health, _maxHealth);
            if (_health <= 0) IsAlive = false;
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

            if (value == false)
            {
                Debug.Log("Player died!"); // �������� ���
                damageableDeath.Invoke();

                if (gameObject.CompareTag("Player"))
                {
                    Debug.Log("Trying to show lose panel...");
                    var uiManager = FindObjectOfType<UIManager>();
                    if (uiManager != null)
                    {
                        uiManager.ShowLosePanel();
                    }
                    else
                    {
                        Debug.LogError("UIManager not found when player died!");
                    }
                }
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
                // ������� ������������
                isInvincible = false;
                timerSinceHit = 0;
            }
            timerSinceHit += Time.deltaTime;
        }
    }

    // ���������� �������� ��� �� �������� �������
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
        // ���� �� ��������
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
