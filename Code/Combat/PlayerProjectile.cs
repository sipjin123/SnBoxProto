using Sandbox;

public sealed class PlayerProjectile : BaseProjectile, Component.ITriggerListener
{
	[Property]
	public float Speed { get; set; } = 2000f;

	[Property]
	public float LifeTime { get; set; } = 3f;
	private bool _isActive;
	[Sync]
	public bool IsActive
	{
		get => _isActive;
		set
		{
			_isActive = value;

			VisualObj.Enabled = value;
			Collider.Enabled = value;
		}
	}
	private Vector3 _startPosition;
	[Sync] public Vector3 StartPosition 
	{	
		get => _startPosition;
		set
		{
			_startPosition = value;
			WorldPosition = value;
		}
	}
	[Sync]
	public Rotation StartRotation
	{
		get => _startRotation;
		set
		{
			_startRotation = value;
			WorldRotation = value;
		}
	}

	private Rotation _startRotation;

	[Sync]
	private Vector3 Direction { get; set; }
	private Vector3 _smoothPosition;
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
		IsActive = true;
		_smoothPosition = WorldPosition;
		// Ensure this object replicates
		Network.AssignOwnership( Connection.Host );

		StartPosition = WorldPosition;
		StartRotation = WorldRotation;
		if ( !Networking.IsHost )
		{
			//VisualObj.WorldPosition = WorldPosition + Direction * 2f;
		}
		GameObject.Enabled = true;
	}
	protected override void OnStart()
	{
		base.OnStart();
		_smoothPosition = WorldPosition;
	}
	protected override void OnUpdate()
	{
		base.OnUpdate();
		// Server authoritative movement
		if ( !IsActive )
			return;
		if ( Networking.IsHost )
		{
			WorldPosition += Direction * Speed * Time.Delta;
		}
		else
		{
			
			WorldPosition += Direction * Speed * Time.Delta;
			//_smoothPosition = Vector3.Lerp(_smoothPosition, WorldPosition, Time.Delta * 25f);
			//VisualObj.WorldPosition = _smoothPosition;
		}

		if ( Networking.IsHost )
		{
			// Simple lifetime cleanup
			if ( _spawnTime >= LifeTime )
			{
				//Log.Info($"End Life: {GameObject.Name}");
				RePool();
			}
		}
	}

	public void OnTriggerEnter( Collider other )
	{
		if ( other.Components.Get<IPlayer>() == null )
		{
			//RePool();
			return;
		}

		/*
		var myOwner = GameObject.Network.Owner;
		var otherOwner = other.GameObject.Network.Owner;
		if ( myOwner == otherOwner ) return;
		*/

		if ( other.GameObject == Owner ) return;
		if ( !Network.IsOwner ) return;

		PlayerStateComp otherComp = other.Components.Get<PlayerStateComp>();
		if ( otherComp != null )
		{
			var owner = GameObject.Network.Owner;
			if ( owner != null )
			{
				Log.Info( $"Server Collide Enter: {other.GameObject.Name} ---  {PlayerId.ToString()} --- {owner.DisplayName} --- {owner.Id} --- {otherComp.PlayerId.ToString()}" );
				otherComp.ApplyDamageRpc( 10f );
			}
			else
			{
				Log.Info( $"This object Cant find OWNER: {GameObject.Name}" );
			}
		}
		else
		{
			Log.Info( $"This object Cant find a State Comp: {other.GameObject.Name}" );
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

		IsActive = false;
		GameObject.Enabled = false;
		Pool?.Return( this );
	}
}