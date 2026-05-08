using Sandbox;

public sealed class SpawnerTrigger : Component, Component.ITriggerListener
{
	bool IsActivated;
	public void OnTriggerEnter(Collider other)
	{
		if ( Networking.IsHost && !IsActivated)
		{
			IsActivated = true;
			Log.Info($"TRIGGER ME: {GameObject.Name}");
			EnemySpawnerComp.Instance.SpawnEnemy(true);
			ToggleAfterDelay(3f);
		}
	}	
	public async void ToggleAfterDelay(float seconds = 5f)
	{
		await GameTask.DelaySeconds(seconds);
		IsActivated = false;
	}
}
