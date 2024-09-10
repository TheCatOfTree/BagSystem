using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScripTable/ItemDatebase")]
public class ItemDatabase :ScriptableObject
{
   public List<ItemData> items = new List<ItemData>();
   public Dictionary<int, ItemData> Ditems = new Dictionary<int, ItemData>();
   
   void OnValidate()
   {
      // 清空字典，确保在添加新数据之前没有旧数据
      Ditems.Clear();

      foreach (ItemData item in items)
      {
         if (item != null)
         {
            // 使用item.id作为键，item作为值
            Ditems[item.itemID] = item;
         }
      }
   }

   public ItemData GetItemById(int id)
   {
      Ditems.TryGetValue(id, out ItemData itemData);
      return itemData;
   }
}
