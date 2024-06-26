using KIMJAEWON;
using Photon.Pun;
using UnityEngine;

namespace JH
{
    public class IngredientCreate : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ingredientObject;    // IngredientsObject 속성을 가진 Prefab

        [PunRPC]
        public void GenIngredient(int PlayerHoldPointPhotonID, int ingredient)
        {
            Transform HoldPoint = PhotonView.Find(PlayerHoldPointPhotonID).gameObject.transform;
            GameObject ingObj = PhotonView.Find(ingredient).gameObject;
            // GameObject ingObj = PhotonNetwork.Instantiate($"Ingredient_Create/{ingredientObject.name}", HoldPoint.position, HoldPoint.rotation);
            // Debug.Log($"IngObj : {HoldPoint.gameObject.name}");
            // Debug.Log($"IngObj : {ingObj.name}");
            ingObj.GetComponent<IngredientsObject>().GoTo(HoldPoint.gameObject);
            // HoldPoint.gameObject.transform.parent.parent.parent.gameObject.GetComponent<PlayerAction>().SetPickedItem(ingObj.GetComponent<IngredientsObject>());
        }

        /// <summary>재료를 생성 및 부여하는 함수</summary>
        /// <param name="GeneratePoint">GeneratePoint는 생성한 재료 GameObject를 자식으로 넣을 위치에 대한 인수입니다.</param>
        public void TakeIngredient(GameObject GeneratePoint)
        {
            PhotonView photonView = PhotonView.Get(this);
            GameObject ingObj = PhotonNetwork.Instantiate($"Ingredient_Create/{ingredientObject.name}", GeneratePoint.transform.position, GeneratePoint.transform.rotation);
            photonView.RPC("GenIngredient", RpcTarget.MasterClient, GeneratePoint.GetPhotonView().ViewID, ingObj.GetPhotonView().ViewID);
            GeneratePoint.gameObject.transform.parent.parent.parent.gameObject.GetComponent<PlayerAction>().SetPickedItem(ingObj.GetComponent<IngredientsObject>());
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
                // PhotonView photonView = PhotonView.Get(this);
                TakeIngredient(DebugGenpoint);
            }
        }
#endif
        #endregion
    }
}
