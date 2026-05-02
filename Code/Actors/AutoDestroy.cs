using Sandbox;

public sealed class AutoDestroy : Component
{
	protected override void OnStart()
	{
		DestroyAfterDelay(5f);
	}	
	public async void DestroyAfterDelay(float seconds = 5f)
	{
		await GameTask.DelaySeconds(seconds);

		if (GameObject.IsValid())
		{
			GameObject.Destroy();
		}
	}
}
