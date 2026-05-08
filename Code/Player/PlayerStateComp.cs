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
	[Sync] public NetDictionary<AmmoCount, int> Dictionary { get; set; } = new();
	[Sync] public int Kills { get; set; }
	[Sync, Change( "OnIsHPChanged" )] public float Health { get; set; } = 100f;

	void OnIsHPChanged( float oldValue, float newValue )
	{
		// The value of IsRunning has changed...

		Log.Info( "Player HP is now: " + newValue );
	}
	protected override void OnStart()
	{
		var citizen = Components.Get<Dresser>();
		//citizen.Randomize();
		if ( citizen != null )
			citizen.Apply();
	}
	protected override void OnUpdate()
	{
	}
	[Rpc.Broadcast]
	public void ApplyDamageRpc( float Dmg )
	{
		if ( IsProxy ) return; // server only
		float clampedHP = MathX.Clamp( Health - Dmg, 0f, 100f );
		Health = clampedHP;
		Log.Info( "Apply damage HP is now: " + Dmg + " -- " + Health );
	}
}
