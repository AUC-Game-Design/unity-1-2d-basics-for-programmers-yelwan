using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float changeTime = 1.0f;
    float timer;
    int direction = 1;
    [SerializeField] bool vertical = true;
    bool broken = true;
    float speed = 1.5f;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] Animator animator;
    [SerializeField] PlayerController controller;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] GameObject Player;
    void Start()
    {
        animator.gameObject.SetActive(true);
        rigidBody2D.simulated = true;
        timer = changeTime;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        if (!broken) return;
        Vector2 position = rigidBody2D.position;
        if (vertical)
        {
            position.y += speed * direction * Time.deltaTime;
            animator.SetFloat("ForwardY", 0);
            animator.SetFloat("ForwardY", direction);
        }
        else
        {
            position.x += speed * direction *Time.deltaTime;
            animator.SetFloat("ForwardX", 0);
            animator.SetFloat("ForwardX", direction);
        }
        rigidBody2D.MovePosition(position);
    }
    public void Fix()
    {
        broken = false;
        rigidBody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (controller == null || other.gameObject != Player) return;

        controller.ChangeHealth(-1);

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }


}
