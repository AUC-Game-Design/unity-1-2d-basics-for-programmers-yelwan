using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
   [SerializeField] PlayerController controller;
    void OnTriggerStay2D(Collider2D other)
    {

        if (null == controller) return;
        controller.ChangeHealth(-1);
        
    }
}
