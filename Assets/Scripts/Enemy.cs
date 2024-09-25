using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;

    public event Action<Enemy> OnTriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Finish>(out _))
        {
            OnTriggerEntered?.Invoke(this);
        }
    }

    public void Init(Vector3 position, Vector3 target)
    {
        transform.position = position;
        _mover = GetComponent<Mover>();
        _mover.GetDirection(target);
    }
}

