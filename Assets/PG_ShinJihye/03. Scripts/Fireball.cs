using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


namespace KJW
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] Vector3 targetPos;
        [SerializeField] float throwTime;
        private Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            collider.enabled = false;
        }

        public void SetTargetPos(Vector3 position)
        {
            targetPos = position;
            StartCoroutine(ThrowFireballRoutine());
        }

        IEnumerator ThrowFireballRoutine()
        {
            Vector3 startPoint = transform.position;
            Vector3 endPoint = targetPos;

            float xSpeed = (endPoint.x - startPoint.x) / throwTime;
            float zSpeed = (endPoint.z - startPoint.z) / throwTime;
            float ySpeed = -1 * (0.5f * Physics.gravity.y * throwTime * throwTime + startPoint.y) / throwTime;

            float curTime = 0f;
            while (curTime < throwTime)
            {
                curTime += Time.deltaTime;

                transform.position += new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime;
                ySpeed += Physics.gravity.y * Time.deltaTime;

                yield return null;
            }

            transform.position = endPoint;
            collider.enabled = true;
            yield return null;

            // 떨어지고 나서 할 일
        }
    }
}