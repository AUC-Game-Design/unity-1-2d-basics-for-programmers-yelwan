using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody2d;
    [SerializeField] EnemyController enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            PoolingSystem.instance.ReturnToPool(this.gameObject);
        }
    }
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (null == enemy) return;
        enemy.Fix();
        PoolingSystem.instance.ReturnToPool(this.gameObject);
    }
  
}
