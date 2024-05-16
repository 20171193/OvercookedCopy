using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [SerializeField] Fireball fireballPrefab;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform targetPoint;
    [SerializeField] float throwDelay;

    private void OnEnable()
    {
        StartCoroutine(ThrowFireballRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator ThrowFireballRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(throwDelay);
            ThrowFireball(targetPoint.position);
        }
    }

    public void ThrowFireball(Vector3 targetPos)
    {
        Fireball fireball = Instantiate(fireballPrefab, startPoint);
        fireball.SetTargetPos(targetPoint.position);
    }
}
