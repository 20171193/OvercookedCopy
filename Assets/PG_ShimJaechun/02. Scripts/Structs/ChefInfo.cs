using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChefInfo
{
    public string chefName;
    public Sprite sprite;
    public GameObject prefab;

    public ChefInfo(string chefName, Sprite sprite, GameObject prefab)
    {
        this.chefName = chefName;
        this.sprite = sprite;
        this.prefab = prefab;
    }
}
