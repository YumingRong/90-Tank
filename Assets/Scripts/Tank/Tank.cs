using UnityEngine;

public partial class Tank : MonoBehaviour
{
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public float speed = 0.5f;

    protected Vector2 moveDirection;
    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    protected float smallestGrid = 0.25f;
    protected float invincibleTime;

    public int Health               // The amount of health each tank
    {
        set
        {
            m_health = value;
            animator.SetInteger("health", value);
        }
        get
        {
            return m_health;
        }
    }
    private int m_health;
    [HideInInspector] public bool m_Dead;          // Has the tank been reduced beyond zero health yet?
    protected GameManager gameManager;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = GameManager.GetInstance();
    }

    private void OnEnable()
    {
        rigidbody2d.isKinematic = false;

        // When the tank is enabled, reset the tank's health and whether or not it's dead.
        m_Dead = false;
    }

    private void OnDisable()
    {
        rigidbody2d.isKinematic = true;
    }
}
