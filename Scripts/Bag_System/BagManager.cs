using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using Bag_System.Item;
using Common;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BagManager : MonoSingleton<BagManager>
{
    public SQLiteConnection connection;
    public ItemDatabase itemDatabase;
    private Dictionary<int, Bag> _playerBag;
    public UnityAction _onShowUpdate;
    [SerializeField] public ShowUpdate showUpdates;

    public int bagCount = 0;
    public int maxBagCount = 30;

    void Start()
    {
        connection = new SQLiteConnection(Application.streamingAssetsPath + "/DB/Bag.db",
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _playerBag = new Dictionary<int, Bag>();
        if (TableExists("Bag"))
        {
            List<Bag> bagItems = ReadBagData(); // 读取数据
            foreach (var item in bagItems)
            {
                Debug.Log($"ID: {item.Id}, Count: {item.Count}");
                UpdateBagData(item);
            }
        }
        else
        {
            Debug.Log("Bag table does not exist. Creating table...");
            connection.CreateTable<Bag>(); // 如果表不存在，则创建表
            foreach (var variable in itemDatabase.Ditems)
            {
                Bag newBagItem = new Bag(variable.Key, 0);
                connection.Insert(newBagItem);
            }
        }
    }

    private void UpdateBagData(Bag item)
    {
        _playerBag.TryAdd(item.Id, new Bag(item.Id, item.Count));
    }

    public void OnApplicationQuit()
    {
        foreach (var bag in _playerBag)
        {
            //在数据库中查找
            var existingBag = connection.Table<Bag>().FirstOrDefault(b => b.Id == bag.Key);

            if (existingBag != null)
            {
                //更新记录
                existingBag.Count = bag.Value.Count;
                connection.Update(existingBag);
            }
            else
            {
                //插入新记录
                connection.Insert(new Bag
                {
                    Id = bag.Key,
                    Count = bag.Value.Count
                });
            }
        }

        //关闭数据库连接
        connection.Close();
    }

    bool TableExists(string tableName)
    {
        var tableInfo = connection.GetTableInfo(tableName);
        return tableInfo.Count > 0;
    }

    List<Bag> ReadBagData()
    {
        return connection.Table<Bag>().ToList(); // 读取 Bag 表中的所有数据
    }

    public ItemDatabase GetDatabase()
    {
        return itemDatabase;
    }

    public void UpdateShowUp(GameObject obj) //订阅
    {
        _onShowUpdate = null;
        showUpdates = obj.GetComponentInChildren<ShowUpdate>();
        _onShowUpdate += showUpdates.UpdateShow;
        _onShowUpdate?.Invoke();
    }

    public Dictionary<int, Bag> GetBag()
    {
        return _playerBag;
    }

    public bool ChangeItem(int id, int count)
    {
        if (!itemDatabase.Ditems.TryGetValue(id, out ItemData a))
        {
            return false;
        }

        if (!_playerBag.TryGetValue(id, out Bag bag))
        {
            _playerBag.TryAdd(id, new Bag(id, 0));
            _playerBag.TryGetValue(id, out bag);
            bag.Count += count;
        }
        else
        {
            bag.Count += count;
        }

        if (showUpdates.type == BagType.Bag)
            _onShowUpdate?.Invoke();
        return true;
    }
}