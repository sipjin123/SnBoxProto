using Sandbox;
using Sandbox.Network;
public sealed class AbilityComp : Component
{

	[Property] 
	public GameObject GenericProjectile;

	public void ProcessPlayerShoot()
	{
		#pragma warning disable CS0618
		var tr = Scene.Camera.GameObject.Transform;
		var start = tr.Position;
		var dir   = tr.Rotation.Forward;
		#pragma warning restore CS0618

		var NewPlayerId = Components.Get<PlayerStateComp>().PlayerId;
		ShootRequest(NewPlayerId, start, dir);
	}

	[Rpc.Broadcast]
	void ShootRequest(int NewPlayerId, Vector3 SourceLoc, Vector3 SourceDir)
	{
    	if (IsProxy) return; // 🔥 server only

		Log.Info( "Only server can Spawn Projectile" );
  		SpawnProjectile(NewPlayerId, SourceLoc, SourceDir);
	}

	void SpawnProjectile(int NewPlayerId, Vector3 SourceLoc, Vector3 SourceDir)
	{

		Log.Info( "Spawning Projectile now" );
		//GameObject bullet = GenericProjectile.Clone(TransformUtil.GetPointInFront(SourceLoc, SourceDir, 500f));
		GameObject bullet = GenericProjectile.Clone(TransformUtil.GetPointInFront(SourceLoc, SourceDir, 500f));
		bullet.NetworkSpawn();
    	bullet.Network.TakeOwnership();

		var refVar = bullet.Components.Get<DamageBox>();
		if (refVar != null){
		refVar.PlayerId = NewPlayerId;

		Log.Info( "Success Projectile: " + NewPlayerId );
} else{

		Log.Info( "FAILED Projectile" );
}
		
	}
}
