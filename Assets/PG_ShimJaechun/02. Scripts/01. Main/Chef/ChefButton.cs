using Jc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jc
{
    public class ChefButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField]
        private PlayerEntry entry;

        [SerializeField]
        private GameObject hltObject;

        [SerializeField]
        private int chefIndex;

        private ChefInfo info;

        private void OnEnable()
        {
            info = Manager.PlableData.chefInfos[chefIndex];
        }

        public void OnClickSelectButton()
        {
            entry.ChangeChef(chefIndex);
        }

        private void OnDisable()
        {
            hltObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            entry.ChangeChef(chefIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            entry.ChefImage.sprite = info.sprite;
            hltObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hltObject.SetActive(false);
        }
    }
}
