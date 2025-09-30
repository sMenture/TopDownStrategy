using System.Collections.Generic;
using UnityEngine;

public class PlayerStructureCreator : MonoBehaviour
{
    [SerializeField] private List<Fort> _forts = new List<Fort>();
    [SerializeField] private Fort _fortPrefab;

    private PlayerFlagMover _flagMover;
    private Fort _selectedFort;

    public void Initialized(PlayerFlagMover flag)
    {
        _flagMover = flag;

        _flagMover.Flag.BuildFinish += FinishBuild;
    }

    private void OnEnable()
    {
        foreach (var fort in _forts)
            fort.Selection += ShowCreateMenu;
    }

    private void OnDisable()
    {
        _flagMover.Flag.BuildFinish -= FinishBuild;

        foreach (var fort in _forts)
            fort.Selection -= ShowCreateMenu;
    }

    public void CancelCreate()
    {
        if (_flagMover.IsUnderConstruction)
            return;

        _flagMover.Disable();
    }

    public void Create()
    {
        if (_flagMover.IsActive == false)
            return;

        _flagMover.SetupFlag();

        _selectedFort.CreateNewFort(_flagMover.Flag);
    }

    private void ShowCreateMenu(Fort fort)
    {
        if (_flagMover.IsUnderConstruction)
            return;

        if (fort.TotalUnits >= 1)
        {
            _selectedFort = fort;
            _flagMover.Enable();
        }
    }

    private void FinishBuild(Bot bot)
    {
        Fort fort = Instantiate(_fortPrefab, transform);
        fort.transform.position = _flagMover.Position;

        if(_selectedFort != null)
            _selectedFort.RemoveBot(bot);

        fort.AddBot(bot);
        _forts.Add(fort);

        _selectedFort = null;
        _flagMover.FinishBuilding();
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Fort fort))
            {
                _forts.Add(fort);
            }
        }
    }
#endif
}
