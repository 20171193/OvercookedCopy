using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredients Data", menuName = "Data/Ingredients")]
public class Ingredients : ScriptableObject
{
    [Header("Status")]
    public int id;
    public string Name;

    [Header("Ingredients Mesh")]
    public Object Original;
    public Object Sliced;
}

enum IngredientsState { Original, Sliced }