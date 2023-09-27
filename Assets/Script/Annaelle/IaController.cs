using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IaController : MonoBehaviour
{
    private int randAct;
    [SerializeField] private GameObject placeBomb;
    [SerializeField] private GameObject Bomb;

    [SerializeField] private float moveSpeed;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        randAct = Random.Range(0, 5);
        Vector3 moveInput = Vector3.zero;
        switch (randAct)
        {
            case 0:
                moveInput = new Vector3(-1, 0, 0);
                break;
            
            case 1:
                moveInput = new Vector3(1, 0, 0);
                break;
            case 2:
                moveInput = new Vector3(0, 0, 1);
                break;
            case 3:
                moveInput = new Vector3(0, 0, -1);
                break;
            case 4:
                GameObject newBomb = Instantiate(Bomb);
                newBomb.transform.position = placeBomb.transform.position;
                
                Destroy(newBomb,2.6f);
                break;
                
        }
        _rigidbody.velocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
    }
}
