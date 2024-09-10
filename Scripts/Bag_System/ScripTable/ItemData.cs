using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ScripTable/Item")]
public class ItemData : ScriptableObject
{ 
    public int itemID; // 每个物品的唯一ID
    public string itemName;
    public int  maxCount;
    public bool canCreate;
    public string text;
    public Method getMethod;
    public string recipe;
    public Type type;
    public string imagePath;
}
