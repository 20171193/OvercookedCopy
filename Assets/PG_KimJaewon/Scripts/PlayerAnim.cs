using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace KIMJAEWON
{
    public class PlayerAnim : MonoBehaviour
    {

        [SerializeField] int hp = 3;
        Animator anim;
        [SerializeField] State state;
        bool isMoving = false;

        void Start()
        {
            anim = GetComponent<Animator>(); // Animator 컴포넌트를 가져옴
            StartCoroutine(StateMachine()); // 상태 머신 코루틴 시작
        }

        enum State
        {
            IDLE,
            MOVE
        }

        IEnumerator StateMachine()
        {
            while (true) 
            { 
                if(state != State.IDLE)
                {
                    yield return StartCoroutine(state.ToString());
                }
                yield return null;
            }
        }

        IEnumerator IDLE()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) // 현재 애니메이션이 Idle이 아닐 때
            {
                Debug.Log("Anim Played");
                anim.Play("Idle"); // Idle 애니메이션 재생
            }
            yield return null; 
        }

        IEnumerator MOVE()
        {
            while (isMoving)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Move")) // 현재 애니메이션이 Move가 아닐 때
                {
                    anim.Play("Move", 0, 0); // Move 애니메이션 재생
                }
                yield return null;
            }
        }

        // 이동 상태를 설정하는 메서드
        public void SetMoving(bool moving)
        {
            isMoving = moving;
        }
    }
}

