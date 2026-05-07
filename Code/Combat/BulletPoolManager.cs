using Sandbox;

public sealed class BulletPoolManager : Component
{
	[Property]
	public GameObject ProjectilePrefab { get; set; }
	public static BulletPoolManager Instance { get; private set; }
	[Property]
	public int PoolSize { get; set; } = 32;

	private readonly Queue<PlayerProjectile> _available = new();

	protected override void OnStart()
	{
		Instance = this;
		
		// SERVER ONLY
		if ( !Networking.IsHost )
			return;

		for ( int i = 0; i < PoolSize; i++ )
		{
			var obj = ProjectilePrefab.Clone();

			var projectile = obj.Components.Get<PlayerProjectile>();
			projectile.Pool = this;


			// Networked object
			obj.NetworkSpawn();

			obj.Enabled = false;

			_available.Enqueue( projectile );
		}
	}

	public PlayerProjectile Get(
		Vector3 position,
		Rotation rotation,
		Vector3 direction
	)
	{
	 if ( _available.Count <= 0 )
    {
        var newProjectile = CreateProjectile();
		if (newProjectile != null){
        _available.Enqueue( newProjectile );}else{
        
        Log.Info( "Fail to Generate Pool Entry" );
        }
    }

    var projectile = _available.Dequeue();

    projectile.GameObject.Transform.Position = position;
    projectile.GameObject.Transform.Rotation = rotation;

    projectile.Fire( direction );

    return projectile;
	}

private PlayerProjectile CreateProjectile()
{
    var obj = ProjectilePrefab.Clone();
var projectile = obj.Components.Get<PlayerProjectile>();
    projectile.Pool = this;

    obj.Enabled = false;

    obj.NetworkSpawn();

    return projectile;
}

	public void Return( PlayerProjectile projectile )
	{
		projectile.GameObject.Enabled = false;

		_available.Enqueue( projectile );
	}
}
