using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefabEnemy;
    [SerializeField] private int _spawnAmount = 20;
    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Enemy> _pool;
    private List<Vector3> _spawnCoordinate;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_prefabEnemy),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => Destroy(enemy.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        _spawnCoordinate = new List<Vector3>()
        {
            new(5, 0, 5),
            new(-5, 0, 5),
            new(5, 0, -5),
        };
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0.0f, _repeatRate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            _pool.Release(enemy);
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < _spawnAmount; i++)
        {
            var enemy = _pool.Get();
            enemy.Init(RemoveToPool);
            enemy.transform.position = GetPosition();
            enemy.GetTarget(transform);
        }
    }

    private Vector3 GetPosition()
    {
        var minCoordinateRate = 0;
        var maxCoordinateRate = _spawnCoordinate.Count;

        return _spawnCoordinate[Random.Range(minCoordinateRate, maxCoordinateRate)];
    }

    private void RemoveToPool(Enemy enemy)
    {
        _pool.Release(enemy);
    }
}
