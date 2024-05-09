using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace KIMJAEWON
{
    public class PlayerMover : MonoBehaviour
    {
        public float moveSpeed = 5f;

        float xRotation;
        float yRotation;

        [SerializeField]
        private Rigidbody rb;

        [SerializeField]
        private Vector2 moveInput;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            Vector3 moveVelocity = moveDirection * moveSpeed;
            rb.velocity = moveVelocity;

        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }












    }
}