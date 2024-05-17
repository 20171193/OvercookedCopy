using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [SerializeField] Fireball fireballPrefab;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform targetPoint;
    [SerializeField] float createDelay;

    [SerializeField] List<FireballTargetLv> fireballTargetLv;

    private void OnEnable()
    {
        int fireballTargetLvCnt = fireballTargetLv.Count;  // 11
        int targetPosLv1Cnt = fireballTargetLv[0].targetPos.Length;  // 2

        Debug.Log(targetPosLv1Cnt);

        StartCoroutine(CreateFireballRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator CreateFireballRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(createDelay);

            for (int i = 0; i < fireballTargetLv[0].targetPos.Length; i++)
            {
                CreateFireball(fireballTargetLv[0].targetPos[i]);
            }
        }
    }

    public void CreateFireball(Vector3 targetPos)
    {
        Fireball fireball = Instantiate(fireballPrefab, startPoint);
        fireball.SetTargetPos(targetPos);
    }
}

[Serializable]
public class FireballTargetLv
{
    public Vector3[] targetPos;

    public FireballTargetLv(Vector3[] targetPos)
    {
        this.targetPos = targetPos;
    }
}


