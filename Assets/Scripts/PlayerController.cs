using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] float timeInvincible = 2.0f;
    bool isInvincible = false;
    float damageCooldown;
    Vector2 moveDirection = new Vector2(1, 0);
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth;
    public Rigidbody2D rigidBody2D;
    [SerializeField] InputAction moveAction;
    [SerializeField] Animator animator;
    public GameObject projectilePrefab;
    [SerializeField] Projectile projectile;
    [SerializeField] InputAction talkAction;
    [SerializeField] NonPlayerCharacter character;
    [SerializeField] AudioSource audioSource;
    Vector2 move;

    void Start()
    {
        currentHealth = maxHealth;
       moveAction.Enable();
        talkAction.Enable();
    }

    void Update()
    {
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        move = moveAction.ReadValue<Vector2>();    
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0) isInvincible = false; 
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend();
        }

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Time.timeScale = 0.0f;
        }
    }
    void FixedUpdate()
    {
        Vector2 position = (Vector2)transform.position + move * 0.1f;
        rigidBody2D.MovePosition(position);
    }
   public void ChangeHealth (int amount)
    {
       
        if (amount < 0)
        {
            if (isInvincible) return;
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");

        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UiHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

    }
    void Launch()
    {
        GameObject projectileObject = PoolingSystem.instance.CreateProjectile();
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);

        animator.SetTrigger("Launch");
    }
    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidBody2D.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (null == hit) return;
        if (character != null)
        {
            UiHandler.instance.DisplayDialogue();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public int health { get { return currentHealth; } }
    public int maxHealthGetter { get { return maxHealth; } }
}
