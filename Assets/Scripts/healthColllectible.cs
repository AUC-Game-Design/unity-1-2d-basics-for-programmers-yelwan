using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthColllectible : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] AudioClip collectedClip;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (null == playerController) return;

        if (playerController.health < playerController.maxHealthGetter)
        {
            playerController.ChangeHealth(1);
            Destroy(gameObject);
            playerController.PlaySound(collectedClip);
        }
    }
}
