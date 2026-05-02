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
		ShootRequest(start, dir);
	}

	[Rpc.Broadcast]
	void ShootRequest(Vector3 SourceLoc, Vector3 SourceDir)
	{
    	if (IsProxy) return; // 🔥 server only

		Log.Info( "Only server can Spawn Projectile" );
  		SpawnProjectile(SourceLoc, SourceDir);
	}

	void SpawnProjectile(Vector3 SourceLoc, Vector3 SourceDir)
	{
		GameObject bullet = GenericProjectile.Clone(TransformUtil.GetPointInFront(SourceLoc, SourceDir, 500f));
	}
}
