using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [SerializeField] Fireball fireballPrefab; // Fireball 프리팹
    [SerializeField] Transform startPoint; // Fireball 시작 지점
    [SerializeField] Transform[] targetPoints; // 목표 지점 배열
    [SerializeField] float createDelay; // Fireball 생성 간격
    [SerializeField] List<FireballTargetLv> fireballTargetLv; // Fireball 목표 레벨 리스트

    private void OnEnable()
    {
        fireballPrefab = Instantiate(fireballPrefab);
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
            foreach (Transform target in targetPoints)
            {
                CreateFireball(target);
            }
        }
    }

    public void CreateFireball(Transform target)
    {
       
        // Fireball을 시작 지점에서 생성
        Fireball fireball = Instantiate(fireballPrefab, startPoint.position, startPoint.rotation);
        // Fireball의 목표 위치 설정
        fireball.SetTargetPos(target.position);
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
