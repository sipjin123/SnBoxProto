using Sandbox;

public partial class BaseProjectile : Component, Component.ITriggerListener
{
	[Sync]
	public GameObject Owner { get; set; }
	protected override void OnStart()
	{
	}	
	protected override void OnUpdate()
	{
	}
}
