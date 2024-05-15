using UnityEngine;
using UnityEngine.InputSystem;

namespace JH
{
    public class Temp_Mover : MonoBehaviour
    {
        private Vector3 moveDir;
        private Vector3 rotation;
        public Rigidbody rigidbody;

        public int movePower;
        public int maxSpeed;

        void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            Vector3 forceDir = transform.forward * moveDir.z;
            rigidbody.AddForce(forceDir * movePower, ForceMode.Force);
            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;  // 최대속도에서 속도제한
            }
        }

        private void Rotate()
        {
            transform.Rotate(0, rotation.y * 90f * Time.deltaTime, 0, Space.Self);
        }

        private void OnMove(InputValue value)
        {
            Vector2 inputDir = value.Get<Vector2>();
            moveDir.z = inputDir.y;
            rotation.y = inputDir.x;
        }
    }
}