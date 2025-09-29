using UnityEngine;
using System.Collections;
using System;

public class BotMoverToPosition : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    
    private Vector3 _targetPosition;
    private float _distanceToTarget = 0.1f;

    private Coroutine _coroutine;

    public event Action BotArrived;

    public void MoveTo(Vector3 pos)
    {
        _targetPosition = pos;

        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (transform.position.IsEnoughClose(_targetPosition, _distanceToTarget) == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
            yield return null;
        }

        BotArrived?.Invoke();
    }
}
