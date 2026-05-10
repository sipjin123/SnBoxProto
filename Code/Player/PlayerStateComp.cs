using Sandbox;

public sealed class PlayerStateComp : Component
{
	public struct DamageEvent
	{
		public float Damage;
		public float Time;
	}
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
	
	[Sync]
	public DamageEvent LatestDamage
	{
		get => _latestDamage;
		set
		{
			_latestDamage = value;

			Log.Info( $"Damage Sync: {value.Damage}" );
		}
	}

	private DamageEvent _latestDamage;
	void OnIsHPChanged( float oldValue, float newValue )
	{
		// The value of IsRunning has changed...

		Log.Info( "Player HP is now: " + newValue );
	}
	[Rpc.Broadcast]
	public void ApplyDamageRpc( float Dmg )
	{
		if ( IsProxy ) return; // server only
		float clampedHP = MathX.Clamp( Health - Dmg, 0f, 100f );
		Health = clampedHP;
		LatestDamage = new DamageEvent
		{
			Damage = Dmg,
			Time = Time.Now	
		};
		Log.Info( "Apply damage HP is now: " + Dmg + " -- " + Health );
	}
}
