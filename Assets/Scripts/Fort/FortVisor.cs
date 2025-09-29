using System.Collections.Generic;
using UnityEngine;

public class FortVisor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _radius;
    [SerializeField] private LayerMask _itemMask;

    public List<Item> Search()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _itemMask);
        List<Item> suitableItems = new List<Item>();

        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out Item item))
            {
                suitableItems.Add(item);
            }
        }

        return suitableItems;
    }
}
