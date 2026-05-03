using Sandbox;

public sealed class RazorBinderOBJ : Component
{
	protected override void OnUpdate()
	{
		
	}

	public void BindObjToRazor(PlayerStateComp newComp)
	{
		PlayerStatHUD hud = Components.Get<PlayerStatHUD>();
		hud.player =  newComp;
	}
}
