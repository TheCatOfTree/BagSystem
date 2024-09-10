using Create_System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bag_System.Item
{
    public class Item : MonoBehaviour, IPointerClickHandler
    {
        // private int _id;
        // private string _name;
        // private int _maxCount;
        // private string _text;
        // private bool _canCreate;
        // private Method _getMethod;
        // private Image _image;
        // private int _grade;
        private ItemData _data;
        private int _count;

        private void Start()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerClick != null)
            {
                if (eventData.pointerClick.GetComponent<Item>())
                    CreateSomething.Instance.SetClickObject(eventData.pointerClick);
                else
                {
                    CreateSomething.Instance.compoundMianBan.SetActive(false);
                }
            }
        }

        protected void Init()
        {
        }

        public bool SetItemData(ItemData data)
        {
            _data = data;
            return true;
        }

        public bool setCount(int count)
        {
            _count = count;
            return true;
        }

        public ItemData GetData()
        {
            return _data;
        }
    }
}