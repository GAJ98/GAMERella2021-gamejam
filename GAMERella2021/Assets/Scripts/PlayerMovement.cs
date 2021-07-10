using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float walkSpeed = 0.9f;
    [SerializeField] public float crouchSpeed = 1.3f;
    [SerializeField] public float runSpeed = 1.3f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    private float speed;
    private Vector3 prevMove = Vector3.zero;
    private Rigidbody rb;
    private Collider capsuleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<Collider>();
        speed = walkSpeed;
    }

    private void Update()
    {
        Move();
        Crouch();
        Jump();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D))
        {
            float xDirection = Input.GetAxis("Horizontal");
            float zDirection = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);
        
            Debug.Log(moveDirection);

            transform.position = transform.position + (moveDirection * (speed * Time.deltaTime));
            
            transform.rotation = Quaternion.LookRotation(moveDirection);
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    private void Crouch()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Crouched", true);
            speed = crouchSpeed;
        }
        else
        {
            anim.SetBool("Crouched", false);
            speed = walkSpeed;
        }
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0f,
            Vector3.down, 1f, groundLayer);
    }
}
