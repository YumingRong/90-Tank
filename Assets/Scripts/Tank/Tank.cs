using System.Collections;
using UnityEngine;

public partial class Tank : MonoBehaviour
{
    public int m_PlayerNumber;
    [HideInInspector] public float speed = 0.5f;
    [HideInInspector] public int score;
    [HideInInspector] public bool isInvincible;

    protected Vector2 moveDirection;
    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    protected float smallestGrid = 0.25f;

    protected int Health               // The amount of health each tank
    {
        set
        {
            m_health = value;
            animator.SetInteger("health", value);
            print("Tank health " + value);
        }
        get
        {
            return m_health;
        }
    }
    private int m_health;
    private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?
    protected float invincibleTimer;
    protected float invincibleTime;
    private GameManager gameManager;

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

    public void TakeDamage()
    {
        print("Tank health before damage " + Health);


        if (isInvincible)
            return;
        // Reduce current health by the amount of damage done.
        Health--;
        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (Health <= 0 && !m_Dead)
        {
            StartCoroutine(OnDeath());
        }
    }

    private IEnumerator OnDeath()
    {
        // Set the flag so that this function is only called once.
        m_Dead = true;
        //// Move the instantiated explosion prefab to the tank's position and turn it on.
        //m_ExplosionParticles.transform.position = transform.position;
        //m_ExplosionParticles.gameObject.SetActive(true);

        //// Play the particle system of the tank exploding.
        //m_ExplosionParticles.Play();
        //Debug.Log("Tank explode:" + animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Explode"));
        yield return new WaitForSeconds(7f/10f);
        //// Play the tank explosion sound effect.
        //m_ExplosionAudio.Play();

        // Turn the tank off.
        if (m_PlayerNumber > 0)
            gameObject.SetActive(false);
        else
        {
            gameManager.liveEnemy--;
            Destroy(gameObject);
            gameManager.SpawnEnemyTank();
        }
    }

}
