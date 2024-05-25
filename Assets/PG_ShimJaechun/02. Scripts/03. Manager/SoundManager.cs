using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BGM, UI SFX 전용 사운드 매니저
public class SoundManager : Singleton<SoundManager>
{
    public enum BGMType { Title, Room, Campagin, InGame}
    public enum SFXType { Click, PopUp, ChefChange}

    [SerializeField]
    private AudioSource bgmSource;  // 배경음 소스
    [Header("타이틀-방-캠페인-인게임")]
    [SerializeField]
    private AudioClip[] bgmClip;

    [SerializeField]
    private AudioSource sfxSource;  // 효과음 소스

    [Header("버튼클릭-팝업-요리사변경-로딩")]
    [SerializeField]
    private AudioClip[] sfxClip;

    public void PlayBGM(BGMType type)
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();

        bgmSource.clip = bgmClip[(int)type];
        bgmSource.Play();
    }
    public void StopBGM() 
    {
        if (!bgmSource.isPlaying)
            return;

        bgmSource.Stop();
    }

    public void PlaySFX(SFXType type)
    {
        if (sfxSource.isPlaying)
            sfxSource.Stop();

        sfxSource.clip = sfxClip[(int)type];
        sfxSource.Play();
    }
    public void StopSFX()
    {
        if (!sfxSource.isPlaying)
            return;

        sfxSource.Stop();
    }

}
