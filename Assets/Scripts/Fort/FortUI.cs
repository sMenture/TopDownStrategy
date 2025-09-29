using TMPro;
using UnityEngine;

public class FortUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCount;

    public void ChangeText(int currentCount)
    {
        _textCount.text = currentCount.ToString();
    }
}
