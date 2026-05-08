using Sandbox;

public partial class BaseProjectile : Component, Component.ITriggerListener
{
	[Sync]
	public GameObject Owner { get; set; }
	[Property] 
	public GameObject VisualObj;
	[Property] 
	public Collider Collider;
	protected override void OnStart()
	{
	}	
	protected override void OnUpdate()
	{
	}
}
