using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JH
{
    public class IngredientsObject : MonoBehaviour, IComparable<IngredientsObject>, IPickable
    {
        public IngredientsData ingredientsData;
        public IngredientState IngState;

        [SerializeField] bool initialize;

        private GameObject CurrentObject;

        [Header("Debug")]
        [SerializeField] GameObject DebugGameObject;

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
            IngState = IngredientState.Sliced;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            CurrentObject = (GameObject)Instantiate(ingredientsData.Sliced, gameObject.transform);
            CurrentObject.transform.SetParent(gameObject.transform, true);
        }

        [ContextMenu("PanHeat")]
        public void PanHeated()
        {
            if (ingredientsData.Paned == null)
                return;
            Debug.Log("Panned");
            IngState = IngredientState.Paned;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            CurrentObject = (GameObject)Instantiate(ingredientsData.Paned, gameObject.transform);
            CurrentObject.transform.SetParent(gameObject.transform, true);
        }

        public int CompareTo(IngredientsObject other)
        {
            if (ingredientsData.id == other.ingredientsData.id)
                return IngState - other.IngState;
            return ingredientsData.id - other.ingredientsData.id;
        }

        public void GoTo(GameObject GoPotint)
        {
            gameObject.transform.SetParent(GoPotint.transform, true);
        }
        public void Drop()
        {
            gameObject.transform.SetParent(null);
        }

        #region Debug
#if UNITY_EDITOR
        [ContextMenu("[Debug]Add Ingredients")]
        public void DebugGoTo()
        {
            GoTo(DebugGameObject);
        }
#endif
        #endregion
    }
}
