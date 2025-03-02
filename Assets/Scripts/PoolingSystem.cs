using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PoolingSystem : MonoBehaviour
{
    public static PoolingSystem instance;
    List<GameObject> activeProjectiles;
    Queue<GameObject> projectilePool;
    [SerializeField] Transform projectileParent;
    [SerializeField] PlayerController playerController;

    private void Awake() => instance = this;

    void Start()
    {
        activeProjectiles = new List<GameObject>();
        projectilePool = new Queue<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            CreatePooledProjectile();
        }
    }

   
    void CreatePooledProjectile()
    {
       GameObject projectile = Instantiate(playerController.projectilePrefab, playerController.rigidBody2D.position + Vector2.up * 0.5f, playerController.transform.rotation);
       
       projectilePool.Enqueue(projectile);
        projectile.SetActive(false);
        projectile.transform.SetParent(projectileParent);
    }

    public GameObject CreateProjectile()
    {
        GameObject projectile;
        if (0 == projectilePool.Count)
        {
          projectile = Instantiate(playerController.projectilePrefab,playerController.rigidBody2D.position + Vector2.up * 0.5f, playerController.transform.rotation);
            activeProjectiles.Add(projectile);
            projectile.transform.SetParent(projectileParent);
        }
        else
        {
             projectile =  projectilePool.Dequeue();
            
        }
        activeProjectiles.Add(projectile);
        projectile.transform.position = playerController.rigidBody2D.position + Vector2.up * 0.5f;
        projectile.SetActive(true);
        if (activeProjectiles.Count > 5)
        {
            ClearPool();
        }
        return projectile;

    }
    public void ReturnToPool(GameObject _projectile)
    {
        if (!activeProjectiles.Remove(_projectile)) return;

        _projectile.SetActive(false);
        projectilePool.Enqueue(_projectile);
    }
    void ClearPool()
    {
        List<GameObject> projectilesToClear = new List<GameObject>(activeProjectiles);
        foreach (GameObject projectile in projectilesToClear)
        {
            ReturnToPool(projectile);
        }
    }
}
