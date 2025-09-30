using System.Collections.Generic;
using UnityEngine;

public class TaskAssignmentManager : MonoBehaviour
{
    private Dictionary<Item, Bot> _assignments = new Dictionary<Item, Bot>();

    public bool TryAssign(Bot bot, Item item)
    {
        if (_assignments.ContainsKey(item))
            return false;

        _assignments[item] = bot;
        return true;
    }

    public Bot GetAssignedBot(Item item)
    {
        _assignments.TryGetValue(item, out Bot bot);
        return bot;
    }

    public bool IsAvailable(Item item)
    {
        return _assignments.ContainsKey(item) == false;
    }
}