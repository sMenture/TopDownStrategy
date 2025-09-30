using System;
using UnityEngine;

[RequireComponent(typeof(BotMoverToPosition))]
[RequireComponent(typeof(BotHolder))]
public class Bot : MonoBehaviour
{
    private BotMoverToPosition _mover;
    private BotHolder _holder;

    public Item SelectedItem { get; private set; }
    public BotState State { get; private set; } = BotState.None;

    public event Action<Bot> TaskCompleted;

    private void Awake()
    {
        _mover = GetComponent<BotMoverToPosition>();
        _holder = GetComponent<BotHolder>();
    }

    private void OnEnable()
    {
        _mover.BotArrived += Arrived;
    }

    private void OnDisable()
    {
        _mover.BotArrived -= Arrived;
    }

    public void UpdateTargetPosition(Item item)
    {
        State = BotState.MoveToItem;

        SelectedItem = item;
        _mover.MoveTo(SelectedItem.transform.position);
    }

    public void MoveTo(Vector3 position, BotState botState, Item item = null)
    {
        State = botState;
        _mover.MoveTo(position);

        if(item != null)
            SelectedItem = item;
    }

    private void Arrived()
    {
        if (State == BotState.MoveToItem)
            _holder.TakeItem(SelectedItem);

        TaskCompleted?.Invoke(this);
    }

    public Item ClearItem()
    {
        SelectedItem = null;
        return _holder.GiveItem();
    }

    public void ResetState()
    {
        State = BotState.None;
    }
}
