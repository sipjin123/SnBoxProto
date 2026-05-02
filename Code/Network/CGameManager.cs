using Sandbox;

public sealed class CGameManager : Component
{
	[Property] 
	public GameObject PlayerPrefab;

	protected override void OnStart()
	{
		if (IsProxy) return; // only run on server

		Log.Info("Server Start MSG");
		SpawnPlayer();
	}

	void SpawnPlayer()
	{

		Log.Info("Server Start Spawn");
	#pragma warning disable CS0618
		var pos = new Vector3(0, 0, 100);
		var rot = Rotation.Identity;
	#pragma warning restore CS0618

		Log.Info("Server Start Cloning");
		var player = GameObject.Clone(
			"prefabs/player controller.prefab", // 👈 prefab goes here
			new CloneConfig(
				new Transform(pos, rot),
				null,       // parent
				true,       // start enabled
				"Player"    // name (optional)
			)
		);

		Log.Info("Server Start Spawn Player");
		player.NetworkSpawn();
		
		// 👇 THIS GOES HERE
		///player.Network.TakeOwnership();
	}
}
