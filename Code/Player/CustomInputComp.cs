using Sandbox;
using Sandbox.Network;
using Sandbox.Citizen;
using static Sandbox.Citizen.CitizenAnimationHelper;

public sealed class CustomInputComp : Component, IPlayer
{
	DebuggerComponent dbg;
	private ScreenLoggerComp ScreenLoggerComp;
	private NetMsgComp NetMsgComp;
	private PlayerStateComp PlayerStateComp;
	private AbilityComp AbilityComp;
	public GameObject ViewObject { get; set; }
	[Property] 
	public GameObject PlayerStatHUD;
	[Property] 
	public GameObject UnitHUD;
	[Property] 
	public GameObject prefabToSpawn;
	const float traceDist = 500f;
	const float debugTime = 2;
	const float spherRadius = 25f;

	[Property] 
	GameObject MeleeWeapon;
	private SkinnedModelRenderer SkinnedModelRenderer;
	protected override void OnStart()
	{
		//dbg = Scene.GetAll<DebuggerComponent>().FirstOrDefault();
		dbg = GameObject.Components.Get<DebuggerComponent>();
		NetMsgComp = GameObject.Components.Get<NetMsgComp>();
		ScreenLoggerComp = GameObject.Components.Get<ScreenLoggerComp>();
		PlayerStateComp = GameObject.Components.Get<PlayerStateComp>();
		AbilityComp = GameObject.Components.Get<AbilityComp>();
		SkinnedModelRenderer = Components.GetInChildren<SkinnedModelRenderer>();
		Log.Info("----- Started!");


		GameObject WorldHudObj = UnitHUD.Clone();
		WorldHudObj.SetParent(GameObject); // 👈 parent to caller
		WorldHudObj.LocalPosition = new Vector3(0, 0, 70f);
		WorldHudObj.Components.Get<UnitHP>().player = PlayerStateComp;

		var citizen = Components.Get<Dresser>();
		//citizen.Randomize();
		if ( citizen != null )
		{
			citizen.Apply();
		}
		if (!Network.IsOwner)
		{
		}
		else
		{
			bool ShouldSpawnUI = true;
			if (ShouldSpawnUI)
			{
				Log.Info("----- Spawning my own HUD!");
				GameObject HudObj = PlayerStatHUD.Clone();
				HudObj.Components.Get<RazorBinderOBJ>().BindObjToRazor(GameObject.Components.Get<PlayerStateComp>());
			}
		}
		//SkinnedModelRenderer.SceneModel.SetAnimParameter( "holdtype", 1 );
		/*
		Pistol 1
		Rifle 2
		Shotgun 3
		Melee 4
		Fists 5
		Swing, 6
		RPG, 7
		Physgun 8
		*/ 

		//b_reload / b_deploy / hit / b_noclip(fly) / b_reloading(hold) /
		SkinnedModelRenderer.SceneModel.SetAnimParameter( "holdtype", 6 );
		//MeleeWeapon.SetParent( SkinnedModelRenderer.GameObject, false );
		
		/*
		var bone = SkinnedModelRenderer.GetBoneObject( "hold_r" );
		MeleeWeapon.SetParent( bone, false );
		MeleeWeapon.LocalPosition = Vector3.Zero;
		MeleeWeapon.LocalRotation = Rotation.Identity;*/

	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		
		if (SkinnedModelRenderer != null && SkinnedModelRenderer.TryGetBoneTransform( "hold_r", out var tx ) )
		{
				MeleeWeapon.WorldTransform = tx;
		}
		
		//SkinnedModelRenderer.SceneModel?.SetAnimParameter( "move_speed", 1f );
		//SkinnedModelRenderer.SceneModel?.SetAnimParameter( "wish_speed", 1f );
	}

	protected override void OnUpdate()
	{
		//if (Input.Pressed("jump")) Log.Info("Jump");
		//if (Input.Pressed("attack1")) Log.Info("Click");

		if (!Network.IsOwner) return;
		
		
		if (Input.Keyboard.Pressed("Q"))
		{
			//Log.Info("Send msg to server");
			//ScreenLoggerComp.Print("aAaa");
			//NetMsgComp.SendRequestToServer();
		}
		if ( Input.Keyboard.Pressed( "R" ) )
		{
			if ( SkinnedModelRenderer != null )
			{
				//SkinnedModelRenderer.PlaybackRate = 2.0f;
				//SkinnedModelRenderer.SceneModel?.SetAnimParameter( "attack_speed", .1f );
				//SkinnedModelRenderer.SceneModel?.SetAnimParameter( "playback_rate", .1f );
				//SkinnedModelRenderer.SceneModel?.SetAnimParameter( "attack_playback_rate", .1f );
				
				SkinnedModelRenderer.SceneModel?.SetAnimParameter( "atk_spd", .3f );
				SkinnedModelRenderer.SceneModel?.SetAnimParameter( "b_attack", true );
			}
			//GameObject bullet = prefabToSpawn.Clone( GetPointInFront(200f) );
			//AbilityComp.ProcessBomb();
			AbilityComp.ProcessPlayerShoot();
		}
		if ( Input.Keyboard.Pressed( "E" ) )
		{
			DoSphereTraceMulti();
		}

		if ( Input.Keyboard.Pressed( "V" ) )
		{
			
			SkinnedModelRenderer.SceneModel?.SetAnimParameter( "b_attack", true );
		}

		if ( Input.Keyboard.Pressed( "F" ) )
		{
			Log.Info( "F is down!" );
			
			DoSphereTraceOnce();
		}


		if ( Input.Keyboard.Pressed( "G" ) )
		{
			SkinnedModelRenderer.SceneModel?.SetAnimParameter( "b_reload", true );
			
			//NetMsgComp.RpcSendToServerNew();
		}

		if ( Input.Keyboard.Pressed( "H" ) )
		{
			SkinnedModelRenderer.SceneModel?.SetAnimParameter( "b_deploy", true );
			//NetMsgComp.RpcSendToProxy();
		}
		
		if ( Input.Keyboard.Pressed( "J" ) )
		{
			
			//SkinnedModelRenderer.SceneModel.SetAnimParameter( "holdtype", 0 );
			SkinnedModelRenderer.SceneModel.SetAnimParameter(
				"special_movement_states",
				(int)SpecialMoveStyle.Slide
			);

			//HoldTypes.

			//SkinnedModelRenderer.SceneModel?.SetAnimParameter( "b_panic", true );
			//NetMsgComp.RpcSendToOnlyHOST();
		}
		
		if ( Input.Keyboard.Pressed( "K" ) )
		{
			
			SkinnedModelRenderer.SceneModel.SetAnimParameter( "holdtype", 0 );
			SkinnedModelRenderer.SceneModel.SetAnimParameter(
				"special_movement_states",
				(int)SpecialMoveStyle.Roll
			);
			//NetMsgComp.RpcSendToOnlyHOST();
		}
		if ( Input.Keyboard.Pressed( "I" ) )
		{
			NetMsgComp.RpcSendToNOTProxy();
		}


		if ( Input.Keyboard.Pressed( "M" ) )
		{
			SkinnedModelRenderer.SceneModel?.SetAnimParameter( "hit_strength", 1f );
			SkinnedModelRenderer.SceneModel?.SetAnimParameter( "hit", true );
			PlayerStateComp.ApplyDamageRpc(5f);
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
