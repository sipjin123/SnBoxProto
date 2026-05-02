using Sandbox;
using Sandbox.Network;
public sealed class CGameManager : Component
{
	[Property] 
	public GameObject PlayerPrefab;
	bool requestedSpawn = false;
	private int nextPlayerId = 0;
	protected override void OnStart()
	{
		if (IsProxy) return; // 🔥 only authoritative side (server)
			
		//if (IsProxy) return; // only run on server
		if (requestedSpawn) return;
		requestedSpawn = true;
		
		SpawnPlayer();
		Log.Info("Server Start MSG");
		//SpawnPlayer(); 1st working
		//RequestSpawn(); 2nd working but dupe
	}

	protected override void OnUpdate()
	{
		
		//if (Input.Pressed("jump")) Log.Info("Jump");
		//if (Input.Pressed("attack1")) Log.Info("Click");
		/*
		if (IsProxy) return; 
		
		if ( Input.Keyboard.Pressed( "X" ) )
		{
			//if (IsProxy) return; // only run on server
			if (requestedSpawn) return;
			requestedSpawn = true;
		
			RequestSpawn();
			Log.Info("Server Start MSG");
		}*/
	}
	
	//[Rpc.Broadcast( NetFlags.OwnerOnly )]
	//[Rpc.Broadcast( NetFlags.HostOnly )]
	[Rpc.Broadcast]
	void RequestSpawn()
	{
		if (IsProxy) return; // 🔥 only server runs this

		Log.Info("Request Spawn now");
		SpawnPlayer();
	}
	
	void OnClientJoined(Connection conn)
	{ 
		Log.Info("A CLIENT HAS JOINED: ");
	}
	
	void SpawnPlayer()
	{
		var time = Time.Now;
		Log.Info("Server Start Spawn, Curr id is: " +nextPlayerId);
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

		var newId = (int)time;
		Log.Info($"Server Start Spawn Player: {newId}");
		player.NetworkSpawn();
		
		var comp = player.Components.Get<PlayerStateComp>();
		comp.PlayerId = newId;
		
		// 👇 THIS GOES HERE
		///player.Network.TakeOwnership();
	}
}
