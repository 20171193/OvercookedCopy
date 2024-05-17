using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Table: MonoBehaviour
{
    [SerializeField]
    private Material originMT;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material changeMT;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        meshRenderer = child.GetComponent<MeshRenderer>();
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
