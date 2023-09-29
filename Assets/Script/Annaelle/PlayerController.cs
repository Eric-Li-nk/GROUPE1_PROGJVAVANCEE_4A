using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject placeBomb;
    [SerializeField] private GameObject Bomb;

    [SerializeField] private float moveSpeed;
    
    private Rigidbody _rigidbody;
    private Vector2 moveInput = Vector2.zero;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnPlaceBomb(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            GameObject newBomb = Instantiate(Bomb);
            newBomb.transform.position = placeBomb.transform.position;
            newBomb.transform.position = new Vector3(Mathf.RoundToInt(newBomb.transform.position.x),
                newBomb.transform.position.y, Mathf.RoundToInt(newBomb.transform.position.z));
            Destroy(newBomb, 2.6f);
        }
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
    }
    
}
