using System;
using System.Collections.Generic;
using UnityEngine;

public class FortTaskDispatcher : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    private FortWarehouse _warehouse;

    private Flag _flag;

    public event Action FinishBuild;

    public int BotCount => _bots.Count;

    private void OnEnable()
    {
        foreach (var bot in _bots)
            bot.TaskCompleted += MissionCompletion;
    }

    private void OnDisable()
    {
        foreach (var bot in _bots)
            bot.TaskCompleted -= MissionCompletion;
    }

    public void Initialized(FortWarehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public void AddNewBot(Bot bot)
    {
        _bots.Add(bot);

        bot.TaskCompleted += MissionCompletion;
    }

    public void RemoveBot(Bot bot)
    {
        _bots.Remove(bot);

        bot.TaskCompleted -= MissionCompletion;
    }

    public void DistributeTask(List<Item> items)
    {
        if (items.Count == 0)
            return;

        foreach (var bot in _bots)
        {
            if (bot.State == BotState.None)
            {
                if (_flag && HaveCourier() == false && _warehouse.ItemCount >= _flag.Price)
                {
                    _warehouse.Remove(_flag.Price);
                    bot.MoveTo(_flag.transform.position, BotState.BuildFlag);
                }
                else
                {
                    Item suitableItem = FindSuitable(items);

                    if (suitableItem != null)
                    {
                        suitableItem.SetOwner(bot);
                        bot.UpdateTargetPosition(suitableItem);
                        bot.MoveTo(suitableItem.transform.position, BotState.MoveToItem);
                    }
                }
            }
        }
    }

    public void MissionCompletion(Bot bot)
    {
        switch (bot.State)
        {
            case BotState.MoveToItem:
                bot.MoveTo(_warehouse.transform.position, BotState.DeliverToWarehouse);
                break;

            case BotState.DeliverToWarehouse:
                _warehouse.Add(bot.ClearItem());
                bot.ResetState();
                break;

            case BotState.BuildFlag:
                _flag.Build(bot);
                _flag = null;

                bot.ResetState();

                RemoveBot(bot);
                FinishBuild?.Invoke();
                break;
        }
    }

    public bool HasAvailableBots()
    {
        foreach (var bot in _bots)
        {
            if (bot.State == BotState.None)
                return true;
        }

        return false;
    }

    public void UpdateFlagPosition(Flag flag)
    {
        _flag = flag;
    }

    private Item FindSuitable(List<Item> items)
    {
        Item selected = null;
        float maxDistance = float.MaxValue;

        foreach (var item in items)
        {
            if (item.Owner != null)
                continue;

            float distance = transform.position.SqrDistance(item.transform.position);

            if (maxDistance > distance)
            {
                selected = item;
                maxDistance = distance;
            }
        }

        return selected;
    }

    private bool HaveCourier()
    {
        foreach (var bot in _bots)
            if(bot.State == BotState.BuildFlag)
                return true;

        return false;
    }
}