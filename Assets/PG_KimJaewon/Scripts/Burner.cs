using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace KJW
{
    public class Burner : MonoBehaviour
    {
        [SerializeField] Fireball fireballPrefab; // Fireball 프리팹
        [SerializeField] Transform startPoint; // Fireball 시작 지점
        [SerializeField] Transform[] targetPoints; // 목표 지점 배열
        [SerializeField] float createDelay; // Fireball 생성 간격
        [SerializeField] List<FireballTargetLv> fireballTargetLv; // Fireball 목표 레벨 리스트

        private void OnEnable()
        {
            //fireballPrefab = Instantiate(fireballPrefab);
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
                List<Transform> selectedTargets = SelectRandomTargets(3);

                foreach (Transform target in selectedTargets)
                {
                    CreateFireball(target);
                }
            }
        }

        private List<Transform> SelectRandomTargets(int targetCount)
        {
            List<Transform> randomTargets = new List<Transform>();
            List<Transform> availableTargets = new List<Transform>(targetPoints);

            for (int i = 0; i < targetCount; i++)
            {
                if (availableTargets.Count == 0)
                    break;

                int randomIndex = UnityEngine.Random.Range(0, availableTargets.Count);
                randomTargets.Add(availableTargets[randomIndex]);
                availableTargets.RemoveAt(randomIndex);
            }

            return randomTargets;
        }

        public void CreateFireball(Transform target)
        {

            // Fireball을 시작 지점에서 생성
            Fireball fireball = PhotonNetwork.Instantiate("Fireball", startPoint.position, startPoint.rotation).GetComponent<Fireball>();
            // Fireball의 목표 위치 설정
            fireball.SetTargetPos(target.position);
            Destroy(fireball.gameObject, 8f);
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
}