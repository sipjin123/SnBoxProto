using Sandbox;
using Sandbox.Network;
public sealed class AbilityComp : Component
{

	[Property] 
	public GameObject GenericProjectile, GenericBomb;

	public void ProcessPlayerShoot()
	{
#pragma warning disable CS0618
		var tr = Scene.Camera.GameObject.Transform;
		var start = GameObject.Transform.Position + new Vector3(0,0,30f);
		var rot   = tr.Rotation;
#pragma warning restore CS0618

		var NewPlayerId = GameObject.Components.Get<PlayerStateComp>().PlayerId;
		ShootRequest(NewPlayerId, start, rot);
	}

	[Rpc.Broadcast]
	void ShootRequest( int NewPlayerId, Vector3 SourceLoc, Rotation SourceRot )
	{
		bool UseLegacy = false;
		if ( UseLegacy )
		{
			if ( IsProxy ) return; // 🔥 server only
			SpawnProjectile( NewPlayerId, SourceLoc, SourceRot );
		}
		else
		{
			if ( Networking.IsHost )
			{
				BulletPoolManager.Instance.Get(
					WorldPosition + new Vector3(0,0,30f),
					SourceRot, SourceRot.Forward, NewPlayerId, GameObject
				);
			}
		}
	}

	void SpawnProjectile(int NewPlayerId, Vector3 SourceLoc, Rotation SourceRot)
	{
		GameObject bullet = GenericProjectile.Clone(TransformUtil.GetPointInFront(SourceLoc, SourceRot.Forward, 50f), SourceRot);
		bullet.NetworkSpawn();
    	bullet.Network.TakeOwnership();

		var refVar = bullet.Components.Get<PlayerProjectile>();
		if (refVar != null){
			refVar.PlayerId = NewPlayerId;

			refVar.Fire(SourceRot.Forward );
			//Log.Info( "Success Projectile: " + NewPlayerId );
		}
	}
	
	public void ProcessBomb()
	{
#pragma warning disable CS0618
		var tr = Scene.Camera.GameObject.Transform;
		var start = tr.Position;
		var dir   = tr.Rotation.Forward;
#pragma warning restore CS0618

		var NewPlayerId = Components.Get<PlayerStateComp>().PlayerId;
		SpawnBomb(NewPlayerId, start, dir);
	}
	
	void SpawnBomb(int NewPlayerId, Vector3 SourceLoc, Vector3 SourceDir)
	{
		Log.Info( "Spawning Projectile now" );
		//GameObject bullet = GenericProjectile.Clone(TransformUtil.GetPointInFront(SourceLoc, SourceDir, 500f));
		GameObject bullet = GenericProjectile.Clone(TransformUtil.GetPointInFront(SourceLoc, SourceDir, 500f));
		bullet.NetworkSpawn();
		bullet.Network.TakeOwnership();

		var refVar = bullet.Components.Get<DamageBox>();
		//var refVar = bullet.GetComponent<DamageBox>();
		if (refVar != null){
			refVar.PlayerId = NewPlayerId;

			Log.Info( "Success Projectile: " + NewPlayerId );
		} else{

			Log.Info( "FAILED Projectile" );
		}
	}
}
