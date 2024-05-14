using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    [CreateAssetMenu(fileName = "Ingredients Data", menuName = "Data/Ingredients")]
    public class IngredientsData : ScriptableObject, IComparable<IngredientsData>
    {
        [Header("Status")]
        public int id;
        public string Name;


        [Header("Ingredients Mesh")]
        public UnityEngine.Object Original;
        public UnityEngine.Object Sliced;
        public UnityEngine.Object Potted;
        public UnityEngine.Object Paned;

        [Header("Ingredients Sprite")]
        public Sprite ingSprite;

        public int CompareTo(IngredientsData other)
        {
            return id - other.id;
        }
    }

    public enum IngredientState { Original = 0, Sliced, Potted, Paned, }
}