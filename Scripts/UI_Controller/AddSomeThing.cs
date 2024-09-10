using System;
using System.Collections;
using System.Collections.Generic;
using Bag_System.Item;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AddSomeThing : MonoBehaviour
{
    public ItemDatabase itemDatabase; // 引用ItemDatabase
    public GameObject togglePrefab; // 预制体用于生成Toggle
    private ItemData _addItem;

    void Start()
    {
        GenerateToggleList();
    }

    private void Update()
    {
    }

    void GenerateToggleList()
    {
        // 清空已有的Toggle对象
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 遍历ItemDatabase中的所有ItemData
        foreach (var itemdata in itemDatabase.items)
        {
            // 创建一个新的Toggle对象
            GameObject toggleObject = Instantiate(togglePrefab, transform);

            // 获取Toggle组件
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            TextMeshProUGUI toggleText = toggleObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Item item = toggleObject.GetComponent<Item>();
            // 设置Toggle的显示名称
            if (toggleText != null)
            {
                toggleText.text = itemdata.itemName; // 使用item的名称
            }

            toggle.group = transform.GetComponent<ToggleGroup>();
            // 绑定ChangeItem方法到Toggle的onValueChanged事件
            int itemId = itemdata.itemID; // 获取当前Item的ID
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    ChangeItem(itemId);
                }
            });
        }
    }

    public void ChangeItem(int id)
    {
        _addItem = itemDatabase.GetItemById(id);
    }

    public void AddItem()
    {
        BagManager.Instance.GetBag().TryGetValue(_addItem.itemID, out Bag bag);
        if ((_addItem && BagManager.Instance.bagCount < BagManager.Instance.maxBagCount)
            || bag.Count % _addItem.maxCount != 0)
            BagManager.Instance.ChangeItem(_addItem.itemID, 1);
    }
}