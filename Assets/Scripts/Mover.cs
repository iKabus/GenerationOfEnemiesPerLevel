using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float _speed;

    private Vector3 _targetPosition;

    private void Update()
    {
        Vector3 direction = (_targetPosition - transform.position).normalized;
        
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    public void GetDirection(Vector3 direction)
    {
        _targetPosition = direction;
    }
}
