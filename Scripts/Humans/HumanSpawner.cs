using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;


public class HumanSpawner : MonoBehaviour
{
    [SerializeField] private int _spawnOnStart;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _minDistanceFromPlayer;
    [SerializeField] private List<Human> _humanPrefabs;

    private Pool<Transform> _spawnPointsPool;

    [Inject] private Player _player;
    [Inject] private LevelSystem _levelSystem;
    [Inject] private DiContainer _diContainer;

    public List<Human> SpawnedHumans { get; private set; } = new List<Human>();

    private void Awake()
    {
        _spawnPointsPool = new Pool<Transform>(_spawnPoints);
    }

    private void Start()
    {
        int eachLevelHuman = _spawnOnStart / _humanPrefabs.Count;
        
        foreach (var humanPrefab in _humanPrefabs)
            for (int i = 0; i < eachLevelHuman; i++)
                SpawnHuman(humanPrefab);
    }

    private void SpawnHumanByLevel(int level)
    {
        var humanPrefab = _humanPrefabs.Find(x => x.Level == level);
        SpawnHuman(humanPrefab);
    }
    
    public void SpawnHuman(Human humanPrefab)
    {
        if(humanPrefab == null)
            return;
        
        var spawnedObject = _diContainer.InstantiatePrefab(humanPrefab);
        var spawnPosition= GetSpawnPosition();
        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }
        spawnedObject.transform.position = spawnPosition;

        Human human = spawnedObject.GetComponent<Human>();
        SpawnedHumans.Add(human);
        human.Scannable.OnScanEnd += OnScanEnd;
    }

    private void OnScanEnd(Scannable sender, ScanResult result)
    {
        sender.OnScanEnd -= OnScanEnd;
        SpawnedHumans.Remove(sender.Human);
        SpawnHumanByLevel(sender.Level);
    }

    private Vector3 GetSpawnPosition()
    {
        var spawnPoint = _spawnPointsPool.RandomFromPool(x =>
            Vector3.Distance(x.position, _player.transform.position) > _minDistanceFromPlayer);
        return spawnPoint.position;
    }
    
    public Transform GetNearestTarget()
    {
        var sortedList = SpawnedHumans.OrderBy(x => Vector3.Distance(_player.transform.position, x.transform.position))
            .ToList();
        var target = sortedList.Find(x => x.Level <= _levelSystem.CurrentLevel);
        return target.transform;
    }
}
