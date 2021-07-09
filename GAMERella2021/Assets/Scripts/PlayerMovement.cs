using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 0.1f;
    private Vector3 prevMove = Vector3.zero;

    private void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);
        
        Debug.Log(moveDirection);

        transform.position = transform.position + (moveDirection * (speed * Time.deltaTime));

        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
}