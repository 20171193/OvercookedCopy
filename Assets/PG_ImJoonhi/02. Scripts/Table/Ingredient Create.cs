using Photon.Pun;
using UnityEngine;

namespace JH
{
    public class IngredientCreate : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ingredientObject;    // IngredientsObject 속성을 가진 Prefab

        [PunRPC]
        /// <summary>재료를 생성 및 부여하는 함수</summary>
        /// <param name="GeneratePoint">GeneratePoint는 생성한 재료 GameObject를 자식으로 넣을 위치에 대한 인수입니다.</param>
        void TakeIngredient(Transform GeneratePoint)
        {
            GameObject ingObj = PhotonNetwork.Instantiate($"Ingredient Prefab/{ingredientObject.name}", GeneratePoint.position, GeneratePoint.rotation);
            ingObj.transform.SetParent(GeneratePoint);
        }

        [PunRPC]
        void SetGenPoint()
        {

        }


            
        #region Debug
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] GameObject DebugGenpoint;

        [ContextMenu("[Debug]TakeOut Ingredients")]
        public void DebugTakeOut()
        {
            if (DebugGenpoint != null)
            {
                PhotonView photonView = PhotonView.Get(this);
                // TakeIngredient(DebugGenpoint.transform);
                photonView.RPC("TakeIngredient", RpcTarget.All, DebugGenpoint.transform);
            }
        }
#endif
        #endregion
    }
}
