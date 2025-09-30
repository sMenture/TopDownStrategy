using UnityEngine;

public class BotHolder : MonoBehaviour
{
    [SerializeField] private Vector3 _handPosition;

    public Item SelectedItem { get; private set; }

    public void TakeItem(Item selectedItem)
    {
        SelectedItem = selectedItem;

        SelectedItem.transform.SetParent(transform);
        SelectedItem.transform.localPosition = _handPosition;
    }

    public Item GiveItem()
    {
        if (SelectedItem == null)
            return null;

        Item selected = SelectedItem;
        SelectedItem = null;

        return selected;
    }
}
