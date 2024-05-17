using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] Vector3 targetPos;
    [SerializeField] float throwTime;

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
        yield return null;


        // 떨어지고 나서 할 일
    }
}
