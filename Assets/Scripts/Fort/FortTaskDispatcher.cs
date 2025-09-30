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

    public void Initialized(FortWarehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public void AddNewBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void RemoveBot(Bot bot)
    {
        _bots.Remove(bot);
    }

    public void DistributeTask(List<Item> items)
    {
        if (items.Count == 0)
            return;

        foreach (var bot in _bots)
        {
            if (bot.HaveItem() || bot.HaveFlag())
                continue;

            if (_flag != null && HaveCourier() == false)
            {
                if (_warehouse.ItemCount >= _flag.Price)
                {
                    _warehouse.Remove(_flag.Price);
                    bot.MoveToFlag(_flag);

                    FinishBuild?.Invoke();

                    continue;
                }
            }

            Item suitableItem = FindSuitable(items);

            if (suitableItem != null)
            {
                suitableItem.SetOwner(bot);
                bot.UpdateTargetPosition(suitableItem);
                bot.ItemCollected += HandleMissionCompletion;
            }
        }
    }

    public bool HasAvailableBots()
    {
        foreach (var bot in _bots)
        {
            if (bot.HaveFlag() || bot.HaveFlag())
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

    private void HandleMissionCompletion(Bot bot)
    {
        bot.ItemCollected -= HandleMissionCompletion;

        bot.UpdateWarehousePosition(_warehouse);
    }

    private bool HaveCourier()
    {
        foreach (var bot in _bots)
        {
            if (bot.HaveFlag())
                return true;
        }

        return false;
    }
}