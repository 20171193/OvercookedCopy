using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JH
{
    public class IngredientsObject : MonoBehaviour, IComparable<IngredientsObject>
    {
        public IngredientsData ingredientsData { get; private set; }
        public IngredientState IngState { get; private set; }

        private GameObject CurrentObject;

        void Start()
        {
            IngState = IngredientState.Original;
            CurrentObject = (GameObject)Instantiate(ingredientsData.Original, gameObject.transform.position, Quaternion.identity);
            CurrentObject.transform.SetParent(gameObject.transform, true);
        }

        void Update()
        {

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
            IngState = IngredientState.Sliced;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            CurrentObject = (GameObject)Instantiate(ingredientsData.Sliced, gameObject.transform);
            CurrentObject.transform.SetParent(gameObject.transform, true);
        }

        public int CompareTo(IngredientsObject other)
        {
            if (ingredientsData.id == other.ingredientsData.id)
                return IngState - other.IngState;
            return ingredientsData.id - other.ingredientsData.id;
        }
    }
}
