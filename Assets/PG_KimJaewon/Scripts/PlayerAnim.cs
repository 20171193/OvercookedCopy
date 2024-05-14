using System.Xml.Serialization;
using UnityEngine;

namespace KIMJAEWON
{
    public class PlayerController : MonoBehaviour
    {
        Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        }

        void Update()
        {
            // 예시: 플레이어가 수평 및 수직 입력을 받으면 Move 애니메이션을 트리거
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (horizontalInput != 0 || verticalInput != 0)
            {
                anim.SetBool("isMoving", true); // isMoving 파라미터를 true로 설정하여 Move 애니메이션을 재생
            }
            else
            {
                anim.SetBool("isMoving", false); // isMoving 파라미터를 false로 설정하여 Move 애니메이션을 정지
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ItemPickUp();
            }
        }
        
        void ItemPickUp()
        {
            anim.SetTrigger("PickUP");
        }
    }
}
