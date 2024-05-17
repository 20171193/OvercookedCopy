using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [SerializeField] Fireball fireballPrefab;
    [SerializeField] Transform startPoint;
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
            CreateFireball(fireballTargetLv[0].targetPos);
            }
        }

    public void CreateFireball(Vector3[] targetPos)
    {
        Fireball fireball = null;
        for (int i = 0; i < fireballTargetLv[0].targetPos.Length; i++)
    {
            fireball = Instantiate(fireballPrefab, startPoint);
    }
        // fireball.ThrowFireball();
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


