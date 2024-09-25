using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float _speed;

    private Transform _direction;
    
    private void Update()
    {
        transform.LookAt((_direction.position - transform.position).normalized);

        var direction = (_direction.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, direction, _speed * Time.deltaTime);
    }

    public void GetDirection(Transform direction)
    {
        _direction = direction;
    }
}
