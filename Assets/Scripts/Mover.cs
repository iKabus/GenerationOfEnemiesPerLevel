using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _direction;
    
    private void Update()
    {
        transform.LookAt((_direction.position - transform.position).normalized);
        transform.position = Vector3.MoveTowards(transform.position, _direction.position, _speed * Time.deltaTime);
    }

    public void GetTarget(Transform direction)
    {
        _direction = direction;
    }
}
