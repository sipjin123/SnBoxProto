using System;
using Sandbox;

public sealed class EnemySpawnerComp : Component
{
    public static EnemySpawnerComp Instance { get; private set; }
	[Property] public GameObject EnemyPrefab { get; set; }

	[Property] public float SpawnInterval { get; set; } = 10f;

	[Property] public GameObject GameSpawnObject { get; set; }
	private TimeSince _timeSinceLastSpawn;

	[Property] public float SpawnRadius { get; set; } = 500f;
	protected override void OnStart()
	{
		Instance = this;
		SpawnAfterDelay(3f);
	}	
	public async void SpawnAfterDelay(float seconds = 5f)
	{
		await GameTask.DelaySeconds(seconds);
		//SpawnEnemy(true);
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
			//SpawnEnemy(true);
			_timeSinceLastSpawn = 0;
		}
	}

	public void SpawnEnemy(bool isPreload)
	{
		float randomX =  Game.Random.Float( -SpawnRadius, SpawnRadius );
		float randomY =  Game.Random.Float( -SpawnRadius, SpawnRadius );
		EnemyPoolManager.Instance?.SpawnEnemy(
			GameSpawnObject.WorldPosition + new Vector3(randomX, randomY, 0f),
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
