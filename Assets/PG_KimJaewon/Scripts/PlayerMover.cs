using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Photon.Pun;


namespace KIMJAEWON
{
    public class PlayerMover : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f; // 회전 속도를 조절할 변수 추가
        [SerializeField]
        private Rigidbody rb;
        private Vector2 moveInput;
        [SerializeField] float dashPower;
        [SerializeField] AnimationCurve curve;
        [SerializeField] PlayerInput input;
        [SerializeField] PhotonView view;

        private void Awake()
        {
            if (view.IsMine == false)
            {
                Destroy(input);
            }
        }
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            Move();
            Rotate();
        }

        void Move()
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            Vector3 moveVelocity = moveDirection * moveSpeed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }

        void Rotate()
        {
            if (moveInput != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            if(moveInput.x != 0 && moveInput.y != 0)
            {

            }
        }

        public void OnDash()
        {
            Dash();
        }


        public void Dash()
        {
            if (rb != null)
            {
                Vector3 dashDir = transform.forward; // 플레이어의 현재 방향으로 대쉬하도록 수정
                rb.AddForce(dashDir  * dashPower , ForceMode.Impulse);
            }
            else
            {
                Debug.Log("rb = null");
            }
        }


    }
}

