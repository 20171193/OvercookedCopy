using UnityEngine;
using UnityEngine.InputSystem;

namespace JH
{
    public class PlayerInteraction : MonoBehaviour
    {
        Vector3 position;
        Vector3 startposition = new Vector3(0,0.9f,0);
        RaycastHit hit;

        private Vector2 moveInput;

        private void Update()
        {
            position = gameObject.transform.position;

            Raycast();
        }

        private void Raycast()
        {
            if (Physics.Raycast(position - startposition, new Vector3(moveInput.x, 0f, moveInput.y).normalized, out hit, 1f))
            {
                GameObject gameObject = hit.collider.gameObject;
                Debug.Log($"RAY HIT : {gameObject.name}");
            }
            else
            {
                Debug.Log("Nothing");
            }
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            Gizmos.DrawLine(transform.position - startposition, transform.position - startposition + direction);
        }
    }
}
