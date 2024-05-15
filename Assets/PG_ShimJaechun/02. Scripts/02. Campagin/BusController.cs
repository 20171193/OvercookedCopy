using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BusController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CharacterController controller;

    [Header("Specs")]
    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float moveSpeed;

    [Header("Ballancing")]
    [SerializeField]
    private Vector3 moveDir;

    private void Update()
    {
        Move();
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputDir = value.Get<Vector2>();
        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }

    private void Move()
    {
        if (moveDir == Vector3.zero) 
            return;

        transform.right = moveDir;

        controller.Move(moveDir * moveSpeed*Time.deltaTime);
    }
}
