using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JH
{
    public class IngredientsObject : MonoBehaviour
    {
        [SerializeField] IngredientsData ingredientsData;
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
    }
}
