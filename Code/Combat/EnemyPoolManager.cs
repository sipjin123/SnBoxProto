using Sandbox;

public sealed class EnemyPoolManager : Component
{
    public static EnemyPoolManager Instance { get; private set; }

    [Property]
    public GameObject EnemyPrefab { get; set; }

    [Property]
    public int InitialPoolSize { get; set; } = 16;
	
	[Property] public GameObject PreloadSpawnObject { get; set; }

    private readonly Queue<EnemyBase> _available = new();

    protected override void OnStart()
    {
        Instance = this;

        if ( !Networking.IsHost )
            return;

        for ( int i = 0; i < InitialPoolSize; i++ )
        {
            var enemy = CreateEnemy();

            if ( enemy != null )
            {
                _available.Enqueue( enemy );
            }
        }
    }

    private EnemyBase CreateEnemy()
    {
        var obj = EnemyPrefab.Clone(PreloadSpawnObject.WorldPosition);

        obj.NetworkSpawn();

        var enemy = obj.Components.Get<EnemyBase>();

        if ( enemy == null )
        {
            Log.Error( "Enemy prefab missing PooledEnemy component!" );
            return null;
        }

        enemy.Pool = this;

        enemy.SetActiveState( false );

        return enemy;
    }

    public EnemyBase SpawnEnemy(
        Vector3 position,
        Rotation rotation
    )
    {
        if ( _available.Count <= 0 )
        {
            var newEnemy = CreateEnemy();

            if ( newEnemy != null )
            {
                _available.Enqueue( newEnemy );
            }
        }

        if ( _available.Count <= 0 )
        {
            Log.Error( "Enemy pool empty!" );
            return null;
        }

        var enemy = _available.Dequeue();

        enemy.StartPosition = position;
        enemy.StartRotation = rotation;

        enemy.ResetEnemy();

        return enemy;
    }

    public void ReturnEnemy( EnemyBase enemy )
    {
        enemy.SetActiveState( false );

        _available.Enqueue( enemy );
    }
}