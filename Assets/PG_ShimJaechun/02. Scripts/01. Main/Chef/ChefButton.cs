using Jc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jc
{
    [Serializable]
    public struct ChefInfo
    {
        public string chefName;
        public Sprite sprite;
        public int index;

        public ChefInfo(string chefName, Sprite sprite, int index)
        {
            this.chefName = chefName;
            this.sprite = sprite;
            this.index = index;
        }
    }

    public class ChefButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField]
        private PlayerEntry entry;

        [SerializeField]
        private ChefInfo info;

        [SerializeField]
        private GameObject hltObject;

        private int chefIndex;

        public void OnClickSelectButton()
        {
            entry.ChangeChef(info);
        }

        private void OnDisable()
        {
            hltObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            entry.ChangeChef(info);
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
