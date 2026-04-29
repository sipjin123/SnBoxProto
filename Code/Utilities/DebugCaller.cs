using Sandbox;

public sealed class DebugCalleer : Component
{
	DebuggerComponent dbg;
	public GameObject ViewObject { get; set; }
	
	const float traceDist = 500f;
	const float debugTime = 2;
	const float spherRadius = 25f;
	protected override void OnStart()
	{
		//dbg = Scene.GetAll<DebuggerComponent>().FirstOrDefault();
		dbg = GameObject.Components.Get<DebuggerComponent>();
		Log.Info("----- Started!");
	}

	protected override void OnUpdate()
	{

		if ( Input.Keyboard.Pressed( "E" ) )
		{
#pragma warning disable CS0618
			var tr = GameObject.Transform;
			var start = tr.Position;
			var end   = start + tr.Rotation.Forward * 500f;
#pragma warning restore CS0618

			DebugDrawUtil.Line(start, end, Color.Green, 2f);
		}
		
		if ( Input.Keyboard.Pressed( "R" ) )
		{
			var cam = Scene.Camera;
			var tr = cam.GameObject.Transform;

#pragma warning disable CS0618
			var start = tr.Position;
			var dir   = tr.Rotation.Forward;
#pragma warning restore CS0618
			
			var end = start + dir * traceDist;
			dbg?.Line( start, end, Color.Green, 2f );
		}
	}
}
