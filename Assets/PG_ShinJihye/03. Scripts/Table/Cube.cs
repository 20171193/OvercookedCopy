using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // Item 클래스 임시

    public Animator trashBinAnime;

    private void Awake()
    {
        trashBinAnime = gameObject.GetComponent<Animator>();
    }


}
