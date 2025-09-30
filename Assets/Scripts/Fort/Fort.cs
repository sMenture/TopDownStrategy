using System;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(FortVisor))]
[RequireComponent (typeof(FortBotAutoBuilder))]
[RequireComponent (typeof(FortWarehouse))]
[RequireComponent (typeof(FortResourceUI))]
[RequireComponent (typeof(FortTaskDispatcher))]
public class Fort : MonoBehaviour
{
    [SerializeField] private ClickHandler _clickHandler;

    private FortVisor _visor;
    private FortBotAutoBuilder _creator;
    private FortWarehouse _warehouse;
    private FortResourceUI _resourceUI;
    private FortTaskDispatcher _taskDispatcher;

    private float _updateDelay = 1;

    public event Action<Fort> Selection;

    public int TotalUnits => _taskDispatcher.BotCount;

    private void Awake()
    {
        _visor = GetComponent<FortVisor>();
        _creator = GetComponent<FortBotAutoBuilder>();
        _warehouse = GetComponent<FortWarehouse>();
        _resourceUI = GetComponent<FortResourceUI>();
        _taskDispatcher = GetComponent<FortTaskDispatcher>();

        _taskDispatcher.Initialized(_warehouse);
        _creator.Initialized(_warehouse, _taskDispatcher);
    }

    private void OnEnable()
    {
        _taskDispatcher.FinishBuild += _creator.EnableAutoCreate;
        _clickHandler.OnClick += ClickReceived;
        _warehouse.ChangeCount += _resourceUI.ChangeText;
        _warehouse.ChangeCount += _creator.CreateNew;
    }

    private void OnDisable()
    {
        _taskDispatcher.FinishBuild -= _creator.EnableAutoCreate;
        _clickHandler.OnClick -= ClickReceived;
        _warehouse.ChangeCount -= _resourceUI.ChangeText;
        _warehouse.ChangeCount -= _creator.CreateNew;
    }

    private void Start()
    {
        _creator.EnableAutoCreate();

        StartCoroutine(MoveToTarget());
    }

    public void CreateNewFort(Flag flag)
    {
        _taskDispatcher.UpdateFlagPosition(flag);
        _creator.DisableAutoCreate();
    }

    public void AddBot(Bot bot) => _taskDispatcher.AddNewBot(bot);

    public void RemoveBot(Bot bot) => _taskDispatcher.RemoveBot(bot);

    private IEnumerator MoveToTarget()
    {
        var wait = new WaitForSeconds(_updateDelay);

        while (enabled)
        {
            var items = _visor.Search();
            _taskDispatcher.DistributeTask(items);

            yield return wait;

        }
    }

    private void ClickReceived()
    {
        Selection?.Invoke(this);
    }
}
