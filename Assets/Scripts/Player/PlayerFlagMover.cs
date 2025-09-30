using System;
using UnityEngine;

public class PlayerFlagMover : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;

    [Header("")]
    [SerializeField] private Flag _flag;
    [SerializeField] private Camera _camera;

    public Flag Flag => _flag;
    public bool IsUnderConstruction { get; private set; }
    public bool IsActive => _flag.gameObject.activeInHierarchy;
    public Vector3 Position => _flag.transform.position;

    private void Start()
    {
        Disable();
    }

    public void SetupFlag()
    {
        IsUnderConstruction = true;
    }

    public void FinishBuilding()
    {
        IsUnderConstruction = false;
        Disable();
    }

    public void Enable()
    {
        _flag.gameObject.SetActive(true);
    }

    public void Disable()
    {
        _flag.gameObject.SetActive(false);
    }

    public void UpdatePosition(Vector2 cursorPosition)
    {
        if (IsUnderConstruction)
            return;

        if (!IsActive) return;

        Ray ray = _camera.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
            _flag.transform.position = hit.point;
    }
}
