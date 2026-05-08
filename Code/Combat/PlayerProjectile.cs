using Sandbox;

public sealed class PlayerProjectile : BaseProjectile, Component.ITriggerListener
{    
	[Property]
	public float Speed { get; set; } = 2000f;

	[Property]
	public float LifeTime { get; set; } = 5f;
	private bool _isActive;
	[Sync]
	private Vector3 Direction { get; set; }

	private TimeSince _spawnTime;
    public BulletPoolManager Pool { get; set; }
	[Sync] public int PlayerId { get; set; }
	/// <summary>
	/// Called right after spawning
	/// </summary>
	public void Fire( Vector3 direction )
	{
		Direction = direction.Normal;
		_spawnTime = 0;
		_isActive = true;
		// Ensure this object replicates
		Network.AssignOwnership( Connection.Host );
		GameObject.Enabled = true;
	}
	protected override void OnStart()
	{
		base.OnStart();
	}	
	protected override void OnUpdate()
	{
		base.OnUpdate();
		// Server authoritative movement
		if ( !Networking.IsHost )
			return;
		if ( !_isActive )
			return;

		Transform.Position += Direction * Speed * Time.Delta;
	
		// Simple lifetime cleanup
		if ( _spawnTime >= LifeTime )
		{
			//Log.Info($"End Life: {GameObject.Name}");
			RePool();
		}
	}

	public void OnTriggerEnter( Collider other )
	{
		if ( other.Components.Get<IPlayer>() == null )
		{
			//RePool();
			return;
		}

		var myOwner = GameObject.Network.Owner;
		var otherOwner = other.GameObject.Network.Owner;
		if (myOwner == otherOwner) return;
		if (!Network.IsOwner) return;
		{
			
			PlayerStateComp otherComp = other.Components.Get<PlayerStateComp>();
			var owner = GameObject.Network.Owner;
			Log.Info( $"Server Collide Enter: {other.GameObject.Name} ---  {PlayerId.ToString()} --- {owner.DisplayName} --- {owner.Id} --- {otherComp.PlayerId.ToString()}");
			otherComp.ApplyDamageRpc(10f);
			Destroy();
		}
		RePool();
		/*
		if ( !Networking.IsHost )
			return;

		Log.Info($"Hit: {other.GameObject.Name}");

		;*/
	}

	void RePool()
	{
		if ( !Networking.IsHost )
			return;
		_isActive = false;
		GameObject.Enabled = false;
		Pool?.Return( this );
	}
}