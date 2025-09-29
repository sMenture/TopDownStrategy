using System;
using System.Collections.Generic;
using UnityEngine;

public class FortWarehouse : MonoBehaviour
{
    private List<Item> _items = new List<Item>();

    public event Action<int> ChangeCount;

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
}
