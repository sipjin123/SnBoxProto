using Sandbox;

public sealed class CustomInputComp : Component
{
	DebuggerComponent dbg;
	public GameObject ViewObject { get; set; }
	protected override void OnStart()
	{
		//dbg = Scene.GetAll<DebuggerComponent>().FirstOrDefault();
		dbg = GameObject.Components.Get<DebuggerComponent>();
		Log.Info("----- Started!");
	}

	protected override void OnUpdate()
	{
		if (Input.Pressed("jump")) Log.Info("Jump");
		if (Input.Pressed("attack1")) Log.Info("Click");
		
		// if ( Input.Keyboard.Down( "F" ) )


		if ( Input.Keyboard.Pressed( "R" ) )
		{
			var cam = Scene.Camera;
			var tr = cam.GameObject.Transform;

#pragma warning disable CS0618
			var start = tr.Position;
			var dir   = tr.Rotation.Forward;
#pragma warning restore CS0618
			
			//var dbg = Scene.GetAll<DebuggerComponent>().FirstOrDefault();

			var end = start + dir * traceDist;
			dbg?.Line( start, end, Color.Green, 2f );
		}

		if ( Input.Keyboard.Pressed( "E" ) )
		{
#pragma warning disable CS0618
			var tr = GameObject.Transform;
			var start = tr.Position;
			var end   = start + tr.Rotation.Forward * 500f;
#pragma warning restore CS0618

			DebugDrawUtil.Line(start, end, Color.Green, 2f);
		}
		if ( Input.Keyboard.Pressed( "F" ) )
		{
			Log.Info( "F is down!" );
			CheckNearby();
		}

		if (!Network.IsOwner) return;
		Log.Info("Tick");

	}
	
	public void CheckNearby()
	{
		DoSphereTrace();
	}
	const float traceDist = 500f;
	const float debugTime = 2;
	const float spherRadius = 25f;
	public void DoSphereTrace()
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
		
		DebugOverlay.Sphere(new Sphere(end, spherRadius), trace.Hit ? Color.Red : Color.Green, debugTime);
		DebugOverlay.Line(start, end, Color.Green, debugTime);
		DebugOverlay.Sphere(new Sphere(start, spherRadius), Color.Blue, debugTime);
	}
}
