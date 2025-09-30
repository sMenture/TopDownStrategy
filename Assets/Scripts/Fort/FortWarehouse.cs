using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FortWarehouse : MonoBehaviour
{
    private List<Item> _items = new List<Item>();

    public event Action<int> ChangeCount;

    public int ItemCount => _items.Count;

    private void Start()
    {
        ChangeCount?.Invoke(_items.Count);
    }

    public void Add(Item item)
    {
        _items.Add(item);

        item.transform.SetParent(transform);
        item.gameObject.SetActive(false);

        ChangeCount?.Invoke(_items.Count);
    }

    public void Remove(int count = 1)
    {
        if(_items.Count > count && count >= 1)
        {
            _items = _items.Skip(count).ToList();

            ChangeCount?.Invoke(_items.Count);
        }
    }
}
