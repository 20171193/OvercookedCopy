using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KIMJAEWON
{
    public class ObjectTrigger : MonoBehaviour
    {
        private Material originTile;
        [SerializeField]
        private Material changeTile;
        private GameObject nearestOb;

        private List<GameObject> interactableList = new List<GameObject>();

        public Color color;

        private void Update()
        {
            CalculateDistance();
        }

        private void CalculateDistance()
        {
            GameObject nearObject = null;
            if (interactableList.Count < 1)
            {
                return;
            }
            else if (interactableList.Count == 1)
            {
                nearObject = interactableList[0];
            }
            else
            {
                float minDist = -1f;
                for (int i = 0; i < interactableList.Count; i++)
                {
                    GameObject interactable = interactableList[i];
                    float dist = Vector3.Distance(transform.position, interactable.transform.position);

                    // 최초 탐색
                    if (minDist == -1f)
                    {
                        minDist = dist;

                        nearObject = interactable;
                    }
                    // 가까운 오브젝트 갱신
                    else if (dist < minDist)
                    {
                        minDist = dist;

                        nearObject = interactable;
                    }
                }
            }
            RenewObject(nearObject);
        }

        private void RenewObject(GameObject target)
        {
            // 구해진 가장 가까운 값이 없다면
            if (nearestOb == null)
            {
                IHighlightable ob = target.GetComponent<IHighlightable>();
                // 만약에 IHighlitable 
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
        private void OnTriggerEnter(Collider other)
        {
            if (Manager.Layer.interactableLM.Contain(other.gameObject.layer))
            {
                Debug.Log("인식했숩니당");
                interactableList.Add(other.gameObject);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (Manager.Layer.interactableLM.Contain(other.gameObject.layer))
            {
                interactableList.Remove(other.gameObject);
            }
        }
    }
}
