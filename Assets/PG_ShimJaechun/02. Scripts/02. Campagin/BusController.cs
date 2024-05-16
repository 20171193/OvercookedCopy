using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class BusController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Rigidbody rigid;

    [Header("Specs")]
    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float dashPower;
    [SerializeField]
    private float dashDelay;

    [Header("Ballancing")]
    [SerializeField]
    private Vector3 moveDir;
    [SerializeField]
    private bool enableDash = true;

    private void FixedUpdate()
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

        transform.forward = moveDir;

        rigid.velocity = transform.forward * moveSpeed;
    }

    private void OnDash(InputValue value)
    {
        if (enableDash)
        {
            rigid.AddForce(transform.forward * dashPower, ForceMode.Impulse);
            StartCoroutine(DashDelayRoutine());
        }
    }


    IEnumerator DashDelayRoutine()
    {
        enableDash = false;
        yield return new WaitForSeconds(dashDelay);
        enableDash = true;
    }
}
