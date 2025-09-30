using System;
using UnityEngine;

[RequireComponent(typeof(BotMoverToPosition))]
[RequireComponent(typeof(BotHolder))]
public class Bot : MonoBehaviour
{
    private BotMoverToPosition _mover;
    private BotHolder _holder;

    private Flag _selectedFlag;
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

    public void MoveToFlag(Flag flag)
    {
        _selectedFlag = flag;
        _mover.MoveTo(flag.transform.position);
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
        if(_selectedFlag != null)
        {
            _selectedFlag.Build(this);
            _selectedFlag = null;
        }
        else if(_selectedWarehouse == null)
        {
            _holder.TakeItem(_selectedItem);
            ItemCollected?.Invoke(this);
        }
        else
        {
            _selectedItem.ClearOwner();

            _selectedWarehouse.Add(_holder.GiveItem());
            _selectedWarehouse = null;
            _selectedItem = null;
        }
    }

    public bool HaveItem()
    {
        return _selectedItem != null;
    }

    public bool HaveFlag()
    {
        return _selectedFlag != null;
    }
}
