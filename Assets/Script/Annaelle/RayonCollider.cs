using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayonCollider : MonoBehaviour
{
    private CapsuleCollider cc;

    private void Start()
    {
        cc = this.GetComponent<CapsuleCollider>();
        StartCoroutine(ControlCollider());
    }

    IEnumerator ControlCollider()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(1.5f);
        cc.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "MurDestructible" || collision.transform.tag == "Player")
        {
            this.gameObject.SetActive(false);
            if(collision.transform.tag == "Player")
                BombermanState.isGameOver = true;
            
            Destroy(collision.gameObject);
        }
    }
    
    
}
