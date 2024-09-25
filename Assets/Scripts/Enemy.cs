using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;

    public event Action<Enemy> OnTriggerEntered;

    public void Init(Transform direction, Vector3 position)
    {
        transform.position = position;
        _mover = GetComponent<Mover>();
        _mover.GetDirection(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Finish>(out _))
        {
            OnTriggerEntered?.Invoke(this);
        }
    }
}

