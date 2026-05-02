using Sandbox;

public sealed class TriggerHandlerComp : Component, Component.ITriggerListener
{
	
	protected override void OnStart()
	{
		if (!Network.IsOwner) return;
		Log.Info("----- Started Trigger Comp!");
	}

	public void OnTriggerEnter(Collider other)
	{
		if ( other.Components.Get<IActor>() == null)
			return;
		
		if ( IsProxy )
		{
			Log.Info($"Server Collide Enter: {other.GameObject.Name}");
		}

		if (Network.IsOwner)
		{
			Log.Info($"Client Collide Enter: {other.GameObject.Name}");
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if ( other.Components.Get<IActor>() == null)
			return;
		
		if ( IsProxy )
		{
			Log.Info($"Server Collide Exit: {other.GameObject.Name}");
		}

		if (Network.IsOwner)
		{
			Log.Info($"Client Collide Exit: {other.GameObject.Name}");
		}
	}
}
