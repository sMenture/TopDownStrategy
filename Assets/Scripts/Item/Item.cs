using UnityEngine;

public class Item : MonoBehaviour
{
    public Bot Owner { get; private set; }
    
    public void SetOwner(Bot owner) => Owner = owner;

    public void ClearOwner() => Owner = null;
}
