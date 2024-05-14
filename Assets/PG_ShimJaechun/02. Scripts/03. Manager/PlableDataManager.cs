using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

public class PlableDataManager : Singleton<PlableDataManager>
{
    public List<ChefInfo> chefInfos;

    private void Start()
    {
        chefInfos = new List<ChefInfo>();
    }
}
