using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayonCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "MurDestructible" || collision.transform.tag == "Player")
        {
            Destroy(collision.gameObject);
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
    
    
}
