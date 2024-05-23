using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KIMJAEWON
{


    public class PlayerController : MonoBehaviour
    {
        [Header("에디터 세팅")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f; // 회전 속도를 조절할 변수 추가
        [SerializeField]
        private Rigidbody rb;


        [SerializeField] Animator anim;
        [SerializeField] float dashPower;
        [SerializeField] PhotonView view;
        [SerializeField] PlayerInput input;

        [Header("밸런싱")]
        [SerializeField]
        private Vector2 moveInput;

        private void Awake()
        {
            if (view.IsMine == false)
            {
                Destroy(input);
            }
        }
        private void FixedUpdate()
        {
            Move();
        }

        private void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        private void Move()
        {
            if (moveInput == Vector2.zero)
            {
                anim.SetBool("isMoving", false);
                return;
            }

            // 회전값 적용
            Rotation();

            anim.SetBool("isMoving", true);
            rb.velocity = transform.forward * moveSpeed;
        }
        private void Rotation()
        {
            Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
            transform.forward = dir;
        }

        private void OnDash(InputValue value)
        {
            Dash();
        }
        private void Dash()
        {
            Vector3 dashDir = transform.forward; // 플레이어의 현재 방향으로 대쉬하도록 수정
            rb.AddForce(dashDir * dashPower, ForceMode.Impulse);
        }

        private void Start()
        {
           
        }
    }
}

