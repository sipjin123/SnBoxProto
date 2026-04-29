using Sandbox;

public sealed class TriggerHandlerComp : Component, Component.ITriggerListener
{
	
	protected override void OnStart()
	{
		Log.Info("----- Started Trigger Comp!");
	}

	public void OnTriggerEnter(Collider other)
	{
		Log.Info("----- Started Trigger !");
		Log.Info($"Entered: {other.GameObject.Name}");
	}

	public void OnTriggerExit(Collider other)
	{
		Log.Info($"Exited: {other.GameObject.Name}");
	}
}
