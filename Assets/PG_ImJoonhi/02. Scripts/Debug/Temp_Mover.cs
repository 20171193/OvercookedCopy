using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JH
{
    public class Temp_Mover : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;

        [SerializeField] PhotonView view;

        private Rigidbody rb;
        private Vector2 moveInput;

        private Vector3Int playerPosition;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (view.IsMine == false)
                return;

            Move();
            Rotate();
            playerPosition = new Vector3Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)gameObject.transform.position.z);
        }

        void Move()
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            Vector3 moveVelocity = moveDirection * moveSpeed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }

        private void Rotate()
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
        }
    }
}