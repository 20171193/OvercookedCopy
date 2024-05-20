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

    [SerializeField]
    private bool isGround = false;

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
        {
            rigid.useGravity = true;
            return;
        }

        // 일반 이동
        if(GetSlopeDirection() == Vector3.zero)
        {
            rigid.useGravity = true;
        }
        // 경사면 이동
        else
        {
            Debug.Log("OnSlope");
            rigid.useGravity = false;
            Debug.Log("Projection : " + moveDir);
        }

        transform.forward = moveDir;
        rigid.velocity = transform.forward * moveSpeed;
    }

    // 경사면 방향 추출
    private Vector3 GetSlopeDirection()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1.5f, LayerMask.NameToLayer("Tile")))
        {
            Debug.DrawLine(hitInfo.point, hitInfo.normal * 1.5f, Color.red, 0.3f);
            // 경사면에 투영된 벡터방향 정규화
            return Vector3.ProjectOnPlane(moveDir, hitInfo.normal).normalized;
        }
        else
        {
            return Vector3.zero;
        }
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
