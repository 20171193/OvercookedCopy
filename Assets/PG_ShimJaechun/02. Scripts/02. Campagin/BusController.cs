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

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject boatObject;


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
        FixRotation();
    }
    // 플레이어 회전방향 고정
    private void FixRotation()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
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
            anim.SetBool("IsMoving", false);
            return;
        }
        else
        {
            anim.SetBool("IsMoving", true);
        }

        // 경사면에 따른 방향 도출
        SlopeMovement();

        rigid.velocity = transform.forward * moveSpeed;
    }

    // 경사면 방향 추출
    private void SlopeMovement()
    {
        // 초기 방향 세팅
        transform.forward = moveDir;

        Vector3 frontPos = transform.position + transform.forward * 0.5f + Vector3.up * 0.5f;
        Vector3 backPos = transform.position + -transform.forward * 0.3f;

        Ray ray = new Ray(frontPos, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1.5f, LayerMask.GetMask("SlopeTile")))
        {
            // 슬로프 평면에 Hit된 경우
            rigid.useGravity = false;
            transform.forward = (hitInfo.point - backPos).normalized;
        }
        else
        {
            rigid.useGravity = true;
            transform.forward = moveDir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Manager.Layer.waterTileLM.Contain(other.gameObject.layer))
        {
            Debug.Log("Enter Water");
            anim.SetBool("IsInWater", true);
            boatObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
       if(Manager.Layer.waterTileLM.Contain(other.gameObject.layer))
        {
            Debug.Log("Exit Water");
            anim.SetBool("IsInWater", false);
            boatObject.SetActive(false);
        }
    }

}
