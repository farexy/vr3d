using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    private Dictionary<string, int> _items;

    public void Startup()
    {
        Debug.Log("Inventory manager starting...");
        _items = new Dictionary<string, int>();

        status = ManagerStatus.Started;
    }

    public void AddItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name] += 1;
        }
        else
        {
            _items[name] = 1;
        }
    }

    public bool RemoveItem()
    {
        // TODO oculus
        return true;
        if (_items.Count == 0)
        {
            return false;
        }
        var item = _items.First();
        if (item.Value == 1)
        {
            _items.Remove(item.Key);
        }
        else
        {
            _items[item.Key] -= 1;
        }
        return true;
    }

    public List<string> GetItemList()
    {
        List<string> list = new List<string>(_items.Keys);
        return list;
    }

    public int GetItemCount(string name)
    {
        if (_items.ContainsKey(name))
        {
            return _items[name];
        }
        return 0;
    }

    public int Count()
    {
        int count = 0;
        foreach (int i in _items.Values)
        {
            count += i;
        }
        return count;
    }

    public bool Contains(string name)
    {
        return _items.ContainsKey(name);
    }
}
