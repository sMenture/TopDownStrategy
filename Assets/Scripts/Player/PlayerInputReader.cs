using System;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    private KeyCode _cancelButton = KeyCode.Mouse1;
    private KeyCode _acceptButton = KeyCode.Mouse2;

    public event Action Cancel;
    public event Action Accept;
    public event Action<Vector2> MousePosition;

    private void Update()
    {
        if(Input.GetKeyDown(_cancelButton))
            Cancel?.Invoke();

        if(Input.GetKeyDown(_acceptButton))
            Accept?.Invoke();

        MousePosition?.Invoke(Input.mousePosition);
    }
}
