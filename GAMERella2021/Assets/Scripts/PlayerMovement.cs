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
    [SerializeField] public float jumpTime = 3f;
    [SerializeField] public float crouchColliderHeight = 0.75f;
    [SerializeField] public Vector3 crouchColliderCentre = Vector3.zero;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    private float colliderHeight;
    private Vector3 colliderCenter;
    private float speed;
    private Vector3 prevMove = Vector3.zero;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private int jumpHash = Animator.StringToHash("Jump");
    private bool hasJumped;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        speed = walkSpeed;
        colliderHeight = capsuleCollider.height;
        colliderCenter = capsuleCollider.center;
        hasJumped = false;
        timer = 0f;
    }

    private void Update()
    {
        Move();
        Crouch();
        Jump();

        if (hasJumped)
        {
            capsuleCollider.center = new Vector3(0f, anim.GetFloat("JumpCenter"), 0f);
            capsuleCollider.height = anim.GetFloat("JumpHeight");
            rb.useGravity = false;
            timer = timer + Time.deltaTime;
        }

        if (timer > jumpTime)
        {
            hasJumped = false;
            timer = 0f;
            rb.useGravity = true;
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D))
        {
            float xDirection = Input.GetAxis("Horizontal");
            float zDirection = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);
        
            Debug.Log(moveDirection);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position = transform.position + (moveDirection * (runSpeed * Time.deltaTime));
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
                transform.position = transform.position + (moveDirection * (speed * Time.deltaTime));
                anim.SetBool("Move", true);
            }

            transform.rotation = Quaternion.LookRotation(moveDirection);
            
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
            capsuleCollider.height = crouchColliderHeight;
            capsuleCollider.center = crouchColliderCentre;
        }
        else
        {
            anim.SetBool("Crouched", false);
            speed = walkSpeed;
            capsuleCollider.height = colliderHeight;
            capsuleCollider.center = colliderCenter;
        }
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.up * jumpForce; 
            anim.SetTrigger(jumpHash);
            hasJumped = true;
            timer = 0f;
            capsuleCollider.center = new Vector3(0f, anim.GetFloat("JumpCenter"), 0f);
            capsuleCollider.height = anim.GetFloat("JumpHeight");
        }
        else
        {
            anim.SetBool("Jump", false);
        }
    }

    private bool IsGrounded()
    {
        return Physics.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0f,
            Vector3.down, 1f, groundLayer);
    }
}
