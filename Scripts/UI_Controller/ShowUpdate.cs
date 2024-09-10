using System;
using System.Collections;
using System.Collections.Generic;
using Bag_System.Item;
using Common;
using Create_System;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Type = Common.Type;

public class ShowUpdate : MonoBehaviour
{
    private BagManager _idb;
    public GameObject prefab;
    public BagType type;

    void Start()
    {
    }

    private void Awake()
    {
        _idb = BagManager.Instance;
    }

    public void UpdateShow()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Dictionary<int, Bag> bag = _idb.GetBag();
       
        if (type == BagType.Bag)
        {
            _idb.bagCount = 0;
            foreach (var variable in bag)
            {
                _idb.GetDatabase().Ditems.TryGetValue(variable.Key, out ItemData data);


                int count = variable.Value.Count / data.maxCount;
                while (count > 0)
                {
                    GameObject item = Instantiate(prefab, transform);
                    Item citem = item.AddComponent<Item>();
                    citem.SetItemData(data);
                    citem.setCount(data.maxCount);
                    count--;
                    citem.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = data.maxCount.ToString();
                    citem.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = data.itemName;
                    // item.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{data.imagePath}");
                    _idb.bagCount++;
                }
                if (count == 0 && variable.Value.Count % data.maxCount > 0)
                {
                    GameObject item = Instantiate(prefab, transform);
                    Item citem = item.AddComponent<Item>();
                    citem.SetItemData(data);
                    citem.setCount(variable.Value.Count % data.maxCount);
                    citem.transform.Find("Count").GetComponent<TextMeshProUGUI>().text =
                        (variable.Value.Count % data.maxCount).ToString();
                    citem.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = data.itemName;
                    // item.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{data.imagePath}");
                    _idb.bagCount++;
                }
            }
        }
        

        if (type == BagType.Create)
        {
            
            foreach (var variable in _idb.GetDatabase().Ditems)
            {
                if (variable.Value.canCreate == true) //制造台
                {
                    GameObject item = Instantiate(prefab, transform);
                    Item citem = item.AddComponent<Item>();
                    citem.SetItemData(variable.Value);
                    citem.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = variable.Value.itemName;
                    // item.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{data.imagePath}");
                }
                
            }

            CreateSomething.Instance.compoundMianBan.SetActive(false);
        }
    }
}