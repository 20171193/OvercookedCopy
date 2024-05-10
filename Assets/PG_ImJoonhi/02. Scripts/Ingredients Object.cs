using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JH
{
    public class IngredientsObject : MonoBehaviour
    {
        [SerializeField] Ingredients ingredientsData;
        public bool sliced { get; private set; }

        private GameObject CurrentObject;

        void Start()
        {
            sliced = false;
            CurrentObject = (GameObject) Instantiate(ingredientsData.Original, gameObject.transform.position, Quaternion.identity);
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
            this.sliced = true;
            CurrentObject.SetActive(false);
            Destroy(CurrentObject);
            Instantiate(ingredientsData.Sliced, gameObject.transform);
        }
    }
}
