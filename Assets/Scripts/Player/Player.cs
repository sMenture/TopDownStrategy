using UnityEngine;

[RequireComponent (typeof(PlayerFlagMover))]
[RequireComponent (typeof(PlayerInputReader))]
[RequireComponent (typeof(PlayerStructureCreator))]
public class Player : MonoBehaviour
{
    private PlayerFlagMover _flagMover;
    private PlayerInputReader _inputReader;
    private PlayerStructureCreator _structureCreator;

    private void Awake()
    {
        _flagMover = GetComponent<PlayerFlagMover>();
        _inputReader = GetComponent<PlayerInputReader>();
        _structureCreator = GetComponent<PlayerStructureCreator>();

        _structureCreator.Initialized(_flagMover);
    }

    private void OnEnable()
    {
        _inputReader.Accept += _flagMover.Create;
        _inputReader.Cancel += _flagMover.Disable;

        _inputReader.MousePosition += _flagMover.UpdatePosition;
    }

    private void OnDisable()
    {
        _inputReader.Accept -= _flagMover.Create;
        _inputReader.Cancel -= _flagMover.Disable;

        _inputReader.MousePosition -= _flagMover.UpdatePosition;
    }
}
