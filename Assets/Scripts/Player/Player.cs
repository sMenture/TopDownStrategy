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
        _inputReader.Accept += _structureCreator.Create;
        _inputReader.Cancel += _structureCreator.CancelCreate;

        _inputReader.MousePosition += _flagMover.UpdatePosition;
    }

    private void OnDisable()
    {
        _inputReader.Accept -= _structureCreator.Create;
        _inputReader.Cancel -= _structureCreator.CancelCreate;

        _inputReader.MousePosition -= _flagMover.UpdatePosition;
    }
}
