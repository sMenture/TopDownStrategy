using System.Collections.Generic;
using UnityEngine;

public class FortTaskDispatcher : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    private FortWarehouse _warehouse;

    public void Initialized(FortWarehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public void DistributeTask(List<Item> items)
    {
        if (items.Count == 0)
            return;

        foreach (var bot in _bots)
        {
            if (bot.TargetItem() != null)
                continue;

            Item suitableItem = FindSuitable(items);

            if (suitableItem != null)
            {
                bot.UpdateTargetPosition(suitableItem);
                bot.ItemCollected += HandleMissionCompletion;
            }
        }
    }

    public bool HasAvailableBots()
    {
        foreach (var bot in _bots)
        {
            if (bot.TargetItem() != null)
                return true;
        }

        return false;
    }

    private Item FindSuitable(List<Item> items)
    {
        Item selected = null;
        float maxDistance = float.MaxValue;

        foreach (var item in items)
        {
            if (CanReuse(item))
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

    private bool CanReuse(Item item)
    {
        if (item == null) return false;

        foreach (var bot in _bots)
        {
            if (bot == null) continue;

            Item heldItem = bot.TargetItem(); 

            if (heldItem == item)
                return true;
        }

        return false;
    }

    private void HandleMissionCompletion(Bot bot)
    {
        bot.ItemCollected -= HandleMissionCompletion;

        bot.UpdateWarehousePosition(_warehouse);
    }
}