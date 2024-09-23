using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefabEnemy;
    [SerializeField] private int _spawnAmount = 1;
    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private Coroutine _coroutine;
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
        _coroutine = StartCoroutine(SpawnCooldown());
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            _pool.Release(enemy);
        }
    }

    private IEnumerator SpawnCooldown()
    {
        while (enabled)
        {
            Spawn();

            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < _spawnAmount; i++)
        {
            var enemy = _pool.Get();
            enemy.Init(Release);
            enemy.transform.position = GetPosition();
            enemy.GetDirection(transform);
        }
    }

    private Vector3 GetPosition()
    {
        var minCoordinateRate = 0;
        var maxCoordinateRate = _spawnCoordinate.Count;

        return _spawnCoordinate[Random.Range(minCoordinateRate, maxCoordinateRate)];
    }

    private void Release(Enemy enemy)
    {
        _pool.Release(enemy);
    }
}
