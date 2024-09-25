using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefabEnemy;
    [SerializeField] private Finish _finish;

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
            new(2, 0, 2),
            new(-2, 0, 2),
            new(2, 0, -2),
        };
    }

    private void Start()
    {
        _coroutine = StartCoroutine(SpawnCooldown());
    }

    private IEnumerator SpawnCooldown()
    {
        while (enabled)
        {
            Spawn();

            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
     
    private void Spawn()
    {
        Vector3 spawnPosition = GetPosition();
        Enemy enemy = _pool.Get();
        enemy.Init(spawnPosition, _finish.transform.position);
        Debug.Log(GetDirection(spawnPosition));
        enemy.OnTriggerEntered += Release;
    }

    private Vector3 GetPosition()
    {
        var minCoordinateRate = 0;
        var maxCoordinateRate = _spawnCoordinate.Count;

        return _spawnCoordinate[Random.Range(minCoordinateRate, maxCoordinateRate)];
    }

    private Vector3 GetDirection(Vector3 spawnPosition)
    {
        return (_finish.transform.position - spawnPosition).normalized;
    }

    private void Release(Enemy enemy)
    {
        enemy.OnTriggerEntered -= Release;
        _pool.Release(enemy);
    }
}
