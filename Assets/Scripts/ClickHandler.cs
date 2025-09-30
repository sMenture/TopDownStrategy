using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    public event Action OnClick;

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}
