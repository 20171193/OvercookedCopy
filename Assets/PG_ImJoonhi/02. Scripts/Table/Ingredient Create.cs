using UnityEngine;

namespace JH
{
    public class IngredientCreate : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] IngredientsObject ingredientObject;    // IngredientsObject 속성을 가진 Prefab

        /// <summary>재료를 생성 및 부여하는 함수</summary>
        /// <param name="GeneratePoint">GeneratePoint는 생성한 재료 GameObject를 자식으로 넣을 위치에 대한 인수입니다.</param>
        void TakeIngredient(GameObject GeneratePoint)
        {
            IngredientsObject ingObj = Instantiate(ingredientObject, GeneratePoint.transform.position, GeneratePoint.transform.rotation);
            ingObj.transform.SetParent(GeneratePoint.transform);
        }

        #region Debug
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] GameObject DebugGenpoint;

        [ContextMenu("[Debug]TakeOut Ingredients")]
        public void DebugTakeOut()
        {
            if (DebugGenpoint != null)
                TakeIngredient(DebugGenpoint);
        }
#endif
        #endregion
    }
}
