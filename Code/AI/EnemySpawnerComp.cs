using Sandbox;

public sealed class EnemySpawnerComp : Component
{
	[Property] public GameObject EnemyPrefab { get; set; }

	[Property] public float SpawnInterval { get; set; } = 10f;

	private TimeSince _timeSinceLastSpawn;

	protected override void OnUpdate()
	{
		// Only the server is allowed to spawn
		if ( !Networking.IsHost )
			return;

		if ( EnemyPrefab is null )
			return;

		if ( _timeSinceLastSpawn >= SpawnInterval )
		{
			SpawnEnemy();
			_timeSinceLastSpawn = 0;
		}
	}

	private void SpawnEnemy()
	{
		var enemy = EnemyPrefab.Clone(
			Transform.Position + Vector3.Forward * 100f
		);

		Log.Info( $"Spawned enemy: {enemy.Name}" );
	}
}
