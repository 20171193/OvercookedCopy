using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KIMJAEWON
{
    public class ObjectTrigger : MonoBehaviour
    {
        [SerializeField]
        private Material originTile;

        [SerializeField]
        private Material changeTile;
        private GameObject nearestOb;
        
        //리스트로 나와 부딪히는 테이블을 담아놓는다
        private List<GameObject> interactableList = new List<GameObject>();

        public Color color;

        private void Update()
        {
            // 매 프레임마다 테이블과 나의 거리를 계산해준다
            CalculateDistance();
        }

        // 나와 테이블의 거리 계산식
        private void CalculateDistance()
        {
            // 먼저 GameObject를 null 선언해준다 (비어있어야 하기 때문)
            GameObject nearObject = null;

            // 만약 나에게 부딪히는 테이블의 카운트가 0이라면
            if (interactableList.Count < 1)
            {
                // 그냥 리턴해준다
                return;
            }
            // 만약 나에게 부딪히는 테이블의 카운트가 1이라면
            else if (interactableList.Count == 1)
            {
                // 리스트의 첫 번째 오브젝트를 nearObject로 설정
                nearObject = interactableList[0];
            }
            // 만약 나에게 부딪히는 테이블의 카운트가 2 이상이라면
            else
            {
                // 최소 거리를 -1로 초기화
                float minDist = -1f;

                // 리스트를 순회하면서 가장 가까운 오브젝트 찾기
                for (int i = 0; i < interactableList.Count; i++)
                {
                    // 현재 오브젝트를 가져옴
                    GameObject interactable = interactableList[i];

                    // 현재 오브젝트와의 거리를 계산
                    float dist = Vector3.Distance(transform.position, interactable.transform.position);

                    // 최초 탐색
                    if (minDist == -1f)
                    {
                        // 최소 거리를 현재 거리로 설정
                        minDist = dist;

                        // 가장 가까운 오브젝트를 현재 오브젝트로 설정
                        nearObject = interactable;
                    }
                    // 가까운 오브젝트 갱신
                    else if (dist < minDist)
                    {
                        // 최소 거리를 현재 거리로 갱신
                        minDist = dist;

                        // 가장 가까운 오브젝트를 현재 오브젝트로 갱신
                        nearObject = interactable;
                    }
                }
            }
            // 가장 가까운 오브젝트를 갱신하는 함수 호출
            RenewObject(nearObject);
        }


        private void RenewObject(GameObject target)
        {
            // 구해진 가장 가까운 값이 없다면
            if (nearestOb == null)
            {
                IHighlightable ob = target.GetComponent<IHighlightable>();
                if( ob == null)
                {
                    return;
                }

                // 새로운 오브젝트 할당
                ob.EnterPlayer();
                nearestOb = target;
            }
            else
            {
                IHighlightable prevOb = nearestOb.GetComponent<IHighlightable>();
                prevOb.ExitPlayer();

                nearestOb = target;
                IHighlightable nextOb = nearestOb.GetComponent<IHighlightable>();
                nextOb.EnterPlayer();
            }
        }
        
        // 콜라이더 부딪히는거 식
        private void OnTriggerEnter(Collider other)
        {
            if (Manager.Layer.interactableLM.Contain(other.gameObject.layer))
            {
                Debug.Log("인식했숩니당");
                interactableList.Add(other.gameObject);
            }
        }
        
        // 콜라이더 나갈때 식
        private void OnTriggerExit(Collider other)
        {
            if (Manager.Layer.interactableLM.Contain(other.gameObject.layer))
            {
                interactableList.Remove(other.gameObject);
            }
        }
    }
}
