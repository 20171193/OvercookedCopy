using Photon.Pun;
using System;
using UnityEngine;

namespace JH
{
    public class IngredientsObject : Item, IComparable<IngredientsObject>, IPickable
    {
        [Header("Status")]
        public IngredientsData ingredientsData;
        public IngredientState IngState;

        [Header("Option")]
        [SerializeField] bool initialize;   // 플레이 했을때 IngState를 Original로 자동으로 초기화 할지 여부

        [Header("Misc")]
        // public Rigidbody rigid;

        private GameObject CurrentObject;

        private void Awake()
        {
            rigid = gameObject.GetComponent<Rigidbody>();
            collid = gameObject.GetComponent<BoxCollider>();
            rigid.isKinematic = true;
            collid.enabled = false;
        }

        void Start()
        {
            switch (IngState)
            {
                case IngredientState.Original:
                    CurrentObject = (GameObject)Instantiate(ingredientsData.Original, gameObject.transform.position, Quaternion.identity);
                    break;
                case IngredientState.Paned:
                    CurrentObject = (GameObject)Instantiate(ingredientsData.Paned, gameObject.transform.position, Quaternion.identity);
                    PanHeated();
                    break;
                case IngredientState.Potted:
                    CurrentObject = (GameObject)Instantiate(ingredientsData.Potted, gameObject.transform.position, Quaternion.identity);
                    break;
                case IngredientState.Sliced:
                    CurrentObject = (GameObject)Instantiate(ingredientsData.Sliced, gameObject.transform.position, Quaternion.identity);
                    Slice();
                    break;
            }
            CurrentObject.transform.SetParent(gameObject.transform, true);
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
        }

        public void SetIngredient(IngredientsData ingredient)
        {
            this.ingredientsData = ingredient;
        }

        /// <summary>재료를 자르거나 다질 수 있는지 확인하는 함수.</summary>
        public bool CanSlice()
        {
            return ingredientsData.Sliced != null;
        }

        /// <summary>재료를 다지는 함수.</summary>
        [ContextMenu("Slice")]
        public void Slice()
        {
            if (ingredientsData.Sliced == null)
                return;
            Debug.Log("Slice");
            /*
            IngState = IngredientState.Sliced;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            CurrentObject = (GameObject)Instantiate(ingredientsData.Sliced, gameObject.transform);
            CurrentObject.transform.SetParent(gameObject.transform, true);
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
            */
            photonView.RPC("IngSlice", RpcTarget.All);
        }

        [ContextMenu("PanHeat")]
        public void PanHeated()
        {
            if (ingredientsData.Paned == null)
                return;
            Debug.Log("Panned");
            /*
            IngState = IngredientState.Paned;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            CurrentObject = (GameObject)Instantiate(ingredientsData.Paned, gameObject.transform);
            CurrentObject.transform.SetParent(gameObject.transform, true);
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
            */
            photonView.RPC("IngPanHeat", RpcTarget.All);
        }

        [PunRPC]
        public void IngSlice()
        {
            IngState = IngredientState.Sliced;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            CurrentObject = (GameObject)Instantiate(ingredientsData.Sliced, gameObject.transform);
            CurrentObject.transform.SetParent(gameObject.transform, true);
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
        }

        [PunRPC]
        public void IngPanHeat()
        {
            IngState = IngredientState.Paned;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            CurrentObject = (GameObject)Instantiate(ingredientsData.Paned, gameObject.transform);
            CurrentObject.transform.SetParent(gameObject.transform, true);
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
        }

        public int CompareTo(IngredientsObject other)
        {
            if (ingredientsData.id == other.ingredientsData.id)
                return IngState - other.IngState;
            return ingredientsData.id - other.ingredientsData.id;
        }

        #region Debug
#if UNITY_EDITOR

        [Header("Debug")]
        [SerializeField] GameObject DebugGameObject;

        [ContextMenu("[Debug]GoTo")]
        public void DebugGoTo()
        {
            GoTo(DebugGameObject);
        }

        [ContextMenu("[Debug]Drop")]
        public void DebugDrop()
        {
            Drop();
        }

        [ContextMenu("[Debug]Destroy")]
        public void Destorythis()
        {
            Destroy(gameObject);
        }
#endif
        #endregion
    }
}
