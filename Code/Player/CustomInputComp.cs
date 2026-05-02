using Sandbox;
using Sandbox.Network;
public sealed class CustomInputComp : Component, IPlayer
{
	DebuggerComponent dbg;
	private ScreenLoggerComp ScreenLoggerComp;
	private NetMsgComp NetMsgComp;
	private PlayerStateComp PlayerStateComp;
	private AbilityComp AbilityComp;
	public GameObject ViewObject { get; set; }
	
	[Property] 
	public GameObject prefabToSpawn;
	const float traceDist = 500f;
	const float debugTime = 2;
	const float spherRadius = 25f;
	protected override void OnStart()
	{
		//dbg = Scene.GetAll<DebuggerComponent>().FirstOrDefault();
		dbg = GameObject.Components.Get<DebuggerComponent>();
		NetMsgComp = GameObject.Components.Get<NetMsgComp>();
		ScreenLoggerComp = GameObject.Components.Get<ScreenLoggerComp>();
		PlayerStateComp = GameObject.Components.Get<PlayerStateComp>();
		AbilityComp = GameObject.Components.Get<AbilityComp>();
		
		Log.Info("----- Started!");
	}

	protected override void OnUpdate()
	{
		//if (Input.Pressed("jump")) Log.Info("Jump");
		//if (Input.Pressed("attack1")) Log.Info("Click");


		if (!Network.IsOwner) return;
		
		
		if (Input.Keyboard.Pressed("Q"))
		{
			Log.Info("Send msg to server");
			ScreenLoggerComp.Print("aAaa");
			NetMsgComp.SendRequestToServer();
		}
		if ( Input.Keyboard.Pressed( "R" ) )
		{
			//GameObject bullet = prefabToSpawn.Clone( GetPointInFront(200f) );
			AbilityComp.ProcessPlayerShoot();
		}
		if ( Input.Keyboard.Pressed( "E" ) )
		{
			DoSphereTraceMulti();
		}


		if ( Input.Keyboard.Pressed( "F" ) )
		{
			Log.Info( "F is down!" );
			
			DoSphereTraceOnce();
		}


		if ( Input.Keyboard.Pressed( "G" ) )
		{
			NetMsgComp.RpcSendToServerNew();
		}

		if ( Input.Keyboard.Pressed( "H" ) )
		{
			NetMsgComp.RpcSendToProxy();
		}
		
		if ( Input.Keyboard.Pressed( "J" ) )
		{
			NetMsgComp.RpcSendToOnlyHOST();
		}
		if ( Input.Keyboard.Pressed( "I" ) )
		{
			NetMsgComp.RpcSendToNOTProxy();
		}
		//if (!Network.IsOwner) return;
		//Log.Info("Tick");

	}
	
	public void DoSphereTraceMulti()
	{
		var cam = Scene.Camera;
		var tr = cam.GameObject.Transform;

#pragma warning disable CS0618
		var start = tr.Position;
		var dir   = tr.Rotation.Forward;
#pragma warning restore CS0618
		
		//var start = GameObject.Transform.Position;
		var end = start + dir * traceDist;
		var hits = Scene.Trace
			.Sphere(spherRadius, start, end)
			.IgnoreGameObjectHierarchy(GameObject)
			.RunAll();

		
		dbg.Sphere(end, spherRadius, Color.Red, 2f);
		foreach (var hit in hits)
		{
			Log.Info(hit.GameObject.Name);
		}
	}
	
	public void DoSphereTraceOnce()
	{
		var cam = Scene.Camera;
		var tr = cam.GameObject.Transform;

#pragma warning disable CS0618
		var start = tr.Position;
		var dir   = tr.Rotation.Forward;
#pragma warning restore CS0618
		
		//var start = GameObject.Transform.Position;
		var end = start + dir * traceDist;
		
		var trace = Scene.Trace
			.Sphere(spherRadius, start, end).IgnoreGameObject(GameObject) 
			.Run();

		if (trace.Hit)
		{
			Log.Info($"Hit: {trace.GameObject.Name}");
		}
		
		dbg.Trace(start, end, spherRadius, 2f);
		DebugOverlay.Sphere(new Sphere(end, spherRadius), trace.Hit ? Color.Red : Color.Green, debugTime);
		DebugOverlay.Line(start, end, Color.Green, debugTime);
		DebugOverlay.Sphere(new Sphere(start, spherRadius), Color.Blue, debugTime);
	}
	
	public Vector3 GetPointInFront(float distance = 100f)
	{
		
		var cam = Scene.Camera;
		var tr = cam.GameObject.Transform;
		
#pragma warning disable CS0618
		var start = tr.Position;
		var dir   = tr.Rotation.Forward;
		
		return start + dir * traceDist;
#pragma warning restore CS0618
	} 
}
