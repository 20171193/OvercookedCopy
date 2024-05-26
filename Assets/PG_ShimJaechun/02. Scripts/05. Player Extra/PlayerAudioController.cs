using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PlayerAudioController : MonoBehaviour
    {
        public enum SFXType { PickUp, PutDown, Dash, Chop, Wash}

        [Header("집기 / 놓기 / 대쉬 / 썰기 / 씻기")]
        [SerializeField]
        private AudioSource[] audioSources; // 열거형과 매칭 

        public void PlaySFX(SFXType type)
        {
            if (audioSources[(int)type].isPlaying) return;

            audioSources[(int)type].Play();
        }

        public void StopSFX(SFXType type)
        {
            if (!audioSources[(int)type].isPlaying) return;

            audioSources[(int)type].Stop();
        }
    }
}
