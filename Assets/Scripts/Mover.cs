using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _target;
    
    void Update()
    {
        transform.LookAt((_target.position - transform.position).normalized);
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void GetTarget(Transform target)
    {
        _target = target;
    }
}
