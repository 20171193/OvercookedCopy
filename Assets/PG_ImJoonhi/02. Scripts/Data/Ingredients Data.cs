using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    [CreateAssetMenu(fileName = "Ingredients Data", menuName = "Data/Ingredients")]
    public class IngredientsData : ScriptableObject
    {
        [Header("Status")]
        public int id;
        public string Name;


        [Header("Ingredients Mesh")]
        public Object Original;
        public Object Sliced;
        public Object Potted;
        public Object Paned;
    }

    public enum IngredientState { Original, Sliced, Potted, Paned, }
}