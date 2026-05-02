using Sandbox;

public sealed class AutoDestroy : Component
{
	protected override void OnStart()
	{
		DestroyAfterDelay(3f);
	}	
	public async void DestroyAfterDelay(float seconds = 3f)
	{
		await GameTask.DelaySeconds(seconds);

		if (GameObject.IsValid())
		{
			GameObject.Destroy();
		}
	}
}
