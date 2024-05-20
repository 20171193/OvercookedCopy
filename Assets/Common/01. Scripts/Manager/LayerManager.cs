using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{

    [Header("테이블 + 아이템")]
    [SerializeField]
    private LayerMask interactableLM;

    [Header("테이블")]
    [SerializeField]
    private LayerMask tableLM;
}
