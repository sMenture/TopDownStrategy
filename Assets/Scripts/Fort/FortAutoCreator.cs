using UnityEngine;

public class FortAutoCreator : MonoBehaviour
{
    private const int Price = 3;

    [SerializeField] private Bot _botPrefab;
    [SerializeField] private Transform _botContainer;

    private FortWarehouse _warehouse;
    private FortTaskDispatcher _taskDispatcher;

    public bool AutoCreate { get; private set; }

    public void DisableAutoCreate() => AutoCreate = false;
    public void EnableAutoCreate() => AutoCreate = true;


    public void Initialized(FortWarehouse warehouse, FortTaskDispatcher taskDispatcher)
    {
        _taskDispatcher = taskDispatcher;
        _warehouse = warehouse;
    }

    public void TryCreateNew(int currentCount)
    {
        if (AutoCreate == false)
            return;

        if (_warehouse.ItemCount >= Price)
        {
            _warehouse.Remove(Price);

            Bot bot = Instantiate(_botPrefab, _botContainer);
            _taskDispatcher.AddNewBot(bot);
        }
    }
}
