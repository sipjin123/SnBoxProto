using Sandbox;

public sealed class PlayerStateComp : Component
{
	public enum AmmoCount
	{
		Pistol,
		Rifle
	}
	[Sync] public int PlayerId { get; set; }
	[Sync] public NetList<int> List { get; set; } = new();
	[Sync] public NetDictionary<AmmoCount,int> Dictionary { get; set; } = new();
	[Sync] public int Kills { get; set; }
	[Sync, Change( "OnIsHPChanged" )] public float PlayerHealth { get; set; }
  
	private void OnIsHPChanged( bool oldValue, float newValue )
	{
		// The value of IsRunning has changed...
		
		Log.Info("Player HP is now: " + newValue);
	}
	protected override void OnUpdate()
	{

	}
}
