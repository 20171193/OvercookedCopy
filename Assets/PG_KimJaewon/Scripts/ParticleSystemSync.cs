using Photon.Pun;
using UnityEngine;

public class ParticleSystemSync : MonoBehaviourPun, IPunObservable
{
    private ParticleSystem particleSystem;
    private bool isPlaying;


    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlaying)
            {
                StopParticles();
            }
            else
            {
                PlayParticles();
            }
        }
    }

    public void PlayParticles()
    {
        particleSystem.Play();
        isPlaying = true;
    }

    public void StopParticles()
    {
        particleSystem.Stop();
        isPlaying = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터 전송 (Send Data)
            stream.SendNext(isPlaying);
        }
        else
        {
            // 데이터 수신 (Receive Data)
            isPlaying = (bool)stream.ReceiveNext();
            if (isPlaying)
            {
                particleSystem.Play();
            }
            else
            {
                particleSystem.Stop();
            }
        }
    }
}
