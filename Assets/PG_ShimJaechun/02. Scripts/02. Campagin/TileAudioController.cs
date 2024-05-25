using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip[] clips;

    [SerializeField]
    private ChangeableTile baseTile;

    private void Awake()
    {
        baseTile.OnReversing += PlayReverse;
        baseTile.OnFloating += PlayFloat;
    }
   
    public void PlayReverse()
    {
        if (source.isPlaying)
            source.Stop();

        source.clip = clips[0];
        source.Play();
    }
    public void PlayFloat()
    {
        StartCoroutine(FloatSFXDelay());
    }

    IEnumerator FloatSFXDelay()
    {
        yield return new WaitForSeconds(1.5f);
        if (source.isPlaying)
            source.Stop();

        source.clip = clips[1];
        source.Play();
    }
}
