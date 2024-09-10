using System;
using System.Collections.Generic;
using Bag_System.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Create_System
{
    public class CreateSomething : MonoSingleton<CreateSomething>
    {
        private BagManager _bag;

        public int compoundCount;

        public TextMeshProUGUI text;

        private Dictionary<int, int> _bags; //合成材料[id,count]

        public GameObject prefab;
        public GameObject layout;
        public GameObject clickObject;
        public GameObject compoundMianBan;
        bool _can = true;

        protected void Awake()
        {
            base.Awake();
            _bags = new Dictionary<int, int>();
            clickObject = new GameObject();
            compoundCount = 1;
        }

        private void Update()
        {
        }

        public void CheckDisplay(Item a) //选中合成物品后调用
        {
            foreach (Transform child in layout.transform)
            {
                Destroy(child.gameObject);
            }

            _can = true;
            _bags.Clear();
            ItemData data = a.GetData();
            text.text = data.text;
            //image.sprite = Resources.Load<Sprite>(data.imagePath);
            _bags = ParseRecipe(data.recipe);

            foreach (var variable in _bags)
            {
                BagManager.Instance.GetBag().TryGetValue(variable.Key, out Bag b);
                //预制体创建
                GameObject obj = Instantiate(prefab, layout.transform);
                obj.transform.Find("Items").GetComponent<TextMeshProUGUI>().text =
                    BagManager.Instance.itemDatabase.GetItemById(variable.Key).itemName;
                obj.transform.Find("Count").GetComponent<TextMeshProUGUI>().color = Color.black;
                //拿bag物品和bags物品比较大小
                if (b != null && !(b.Count >= variable.Value * compoundCount))
                {
                    _can = false;
                    obj.transform.Find("Count").GetComponent<TextMeshProUGUI>().color = Color.red;
                }

                if (b != null)
                    obj.transform.Find("Count").GetComponent<TextMeshProUGUI>().text =
                        $"( {b.Count} / {variable.Value * compoundCount} )";
            }
        }


        public void Compound() //合成count个物品
        {
            if (!clickObject)
            {
                return;
            }

            if (!_can)
            {
                Debug.LogWarning("不够材料");
                return;
            }

            ItemData _addItem =
                BagManager.Instance.itemDatabase.GetItemById(clickObject.GetComponent<Item>().GetData().itemID);
            BagManager.Instance.GetBag().TryGetValue(_addItem.itemID, out Bag bag);

            if (bag != null && ((_addItem && BagManager.Instance.bagCount < BagManager.Instance.maxBagCount)
                                || bag.Count % _addItem.maxCount != 0))
            {
                BagManager.Instance.ChangeItem(clickObject.GetComponent<Item>().GetData().itemID, compoundCount);
                foreach (var variable in _bags)
                {
                    BagManager.Instance.ChangeItem(variable.Key, -variable.Value * compoundCount);
                }
            }


            SetClickObject(clickObject);
        }

        public void SetClickObject(GameObject obj)
        {
            clickObject = obj;
            if (clickObject.TryGetComponent(out Item a))
            {
                compoundMianBan.SetActive(true);
                CheckDisplay(a);
            }
            else
            {
                compoundMianBan.SetActive(false);
            }
        }

        public void AddCount()
        {
            compoundCount++;
            if (clickObject)
                CheckDisplay(clickObject.GetComponent<Item>());
        }

        public void MinusCount()
        {
            if (compoundCount > 1)
                compoundCount--;
            if (clickObject)
                CheckDisplay(clickObject.GetComponent<Item>());
        }

        public Dictionary<int, int> ParseRecipe(string recipeString)
        {
            Dictionary<int, int> recipe = new Dictionary<int, int>();
            //根据','分割不同的材料
            string[] materials = recipeString.Split(',');

            foreach (var material in materials)
            {
                //根据':'分割ID和数量
                string[] parts = material.Split(':');
                if (parts.Length == 2)
                {
                    int id = int.Parse(parts[0]); //物品ID
                    int quantity = int.Parse(parts[1]); //物品数量

                    //将ID和数量添加到字典中
                    recipe.Add(id, quantity);
                }
                else
                {
                    Console.WriteLine("Invalid format: " + material);
                }
            }

            return recipe;
        }
    }
}