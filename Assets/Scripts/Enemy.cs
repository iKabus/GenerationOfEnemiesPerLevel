using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;

    private Action<Enemy> _contact;

    public void Init(Action<Enemy> contact)
    {
        _contact = contact;
        _mover = GetComponent<Mover>();
    }

    public void GetDirection(Transform direction)
    {
        _mover.GetTarget(direction);
    }
}
