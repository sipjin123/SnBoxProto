using Sandbox;

public sealed class EnemySpawnerComp : Component
{
	[Property] public GameObject EnemyPrefab { get; set; }

	[Property] public float SpawnInterval { get; set; } = 10f;

	[Property] public GameObject GameSpawnObject { get; set; }
	private TimeSince _timeSinceLastSpawn;

	protected override void OnStart()
	{
		SpawnAfterDelay(3f);
	}	
	public async void SpawnAfterDelay(float seconds = 5f)
	{
		await GameTask.DelaySeconds(seconds);
		SpawnEnemy(true);
	}
	protected override void OnUpdate()
	{
		// Only the server is allowed to spawn
		if ( !Networking.IsHost )
			return;

		if ( EnemyPrefab is null )
			return;

		if ( _timeSinceLastSpawn >= SpawnInterval )
		{
			SpawnEnemy(true);
			_timeSinceLastSpawn = 0;
		}
	}

	private void SpawnEnemy(bool isPreload)
	{
		EnemyPoolManager.Instance?.SpawnEnemy(
			GameSpawnObject.WorldPosition,
			GameSpawnObject.WorldRotation
		);
		/*
		Vector3 spawnPt = GameSpawnObject.WorldPosition;
		var enemy = EnemyPrefab.Clone(
			spawnPt + Vector3.Forward * 100f
		);

		Log.Info( $"Spawned enemy: {enemy.Name}" );*/
	}
}
