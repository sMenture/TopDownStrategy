using System;
using UnityEngine;

[RequireComponent(typeof(BotMoverToPosition))]
[RequireComponent(typeof(BotHolder))]
public class Bot : MonoBehaviour
{
    private BotMoverToPosition _mover;
    private BotHolder _holder;

    private Item _selectedItem;
    private FortWarehouse _selectedWarehouse;

    public event Action<Bot> ItemCollected;

    private void Awake()
    {
        _mover = GetComponent<BotMoverToPosition>();
        _holder = GetComponent<BotHolder>();
    }

    private void OnEnable()
    {
        _mover.BotArrived += BotArrived;
    }

    private void OnDisable()
    {
        _mover.BotArrived -= BotArrived;
    }

    public void UpdateTargetPosition(Item item)
    {
        _selectedItem = item;
        _mover.MoveTo(_selectedItem.transform.position);
    }

    public void UpdateWarehousePosition(FortWarehouse fortWarehouse)
    {
        _selectedWarehouse = fortWarehouse;
        _mover.MoveTo(fortWarehouse.transform.position);
    }

    private void BotArrived()
    {
        if(_selectedWarehouse == null)
        {
            _holder.TakeItem(_selectedItem);

            ItemCollected?.Invoke(this);
        }
        else
        {
            _selectedWarehouse.Add(_holder.GiveItem());
            _selectedWarehouse = null;
            _selectedItem = null;
        }
    }

    public Item TargetItem()
    {
        return _selectedItem;
    }
}
