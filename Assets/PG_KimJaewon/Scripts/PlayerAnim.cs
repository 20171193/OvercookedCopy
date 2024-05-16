using System.Xml.Serialization;
using UnityEditor.EditorTools;
using UnityEngine;

namespace KIMJAEWON
{
    public class PlayerController : MonoBehaviour
    {
        Animator anim;
        [SerializeField] GameObject Item1;
        [SerializeField] GameObject Item2;

        void Start()
        {
            anim = GetComponent<Animator>(); // Animator 컴포넌트 가져오기 .
        }

        void Update()
        {
            // 플레이어가 수평 및 수직 입력을 받으면 Move 애니메이션을 트리거
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

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ItemWashing();
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                ItemCutting();
            }
        }
        
        void ItemPickUp()
        {
            anim.SetTrigger("Pickup");
        }

        void ItemWashing()
        {
            anim.SetTrigger("Washing");
        }

        void ItemCutting()
        {
            anim.SetTrigger("Cutting");
        }


    }
}
