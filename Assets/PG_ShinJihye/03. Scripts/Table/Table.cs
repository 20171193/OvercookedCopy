using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Table: MonoBehaviour
{

    private Material originMT;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material changeMT;

    private void Awake()
    {
        originMT = meshRenderer.sharedMaterial;
    }

    public void EnterPlayer()
    {
        meshRenderer.material = changeMT;
    }

    public void ExitPlayer()
    {
        meshRenderer.material = originMT;
    }

}
