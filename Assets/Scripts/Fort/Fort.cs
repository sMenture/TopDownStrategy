using System.Collections;
using UnityEngine;

[RequireComponent (typeof(FortUI))]
[RequireComponent (typeof(FortVisor))]
[RequireComponent (typeof(FortTaskDispatcher))]
[RequireComponent (typeof(FortWarehouse))]
public class Fort : MonoBehaviour
{
    [SerializeField] private Bot bot;
    [SerializeField] private Item item;

    private FortUI _ui;
    private FortVisor _fortVisor;
    private FortWarehouse _warehouse;
    private FortTaskDispatcher _taskDispatcher;

    private float _updateDelay = 1;

    private void Awake()
    {
        _taskDispatcher = GetComponent<FortTaskDispatcher>();
        _warehouse = GetComponent<FortWarehouse>();
        _fortVisor = GetComponent<FortVisor>();
        _ui = GetComponent<FortUI>();
    }

    private void OnEnable()
    {
        _warehouse.ChangeCount += _ui.ChangeText;
    }

    private void OnDisable()
    {
        _warehouse.ChangeCount -= _ui.ChangeText;
    }

    private void Start()
    {
        _taskDispatcher.Initialized(_warehouse);

        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        var wait = new WaitForSeconds(_updateDelay);

        while (enabled)
        {
            if (_taskDispatcher.HasAvailableBots() == false)
            {
                var items = _fortVisor.Search();
                _taskDispatcher.DistributeTask(items);
            }

            yield return wait;

        }
    }
}
