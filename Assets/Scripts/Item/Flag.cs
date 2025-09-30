using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public event Action<Bot> BuildFinish;

    public int Price { get; private set; } = 5;

    public void Build(Bot courier)
    {
        BuildFinish?.Invoke(courier);
    }
}
