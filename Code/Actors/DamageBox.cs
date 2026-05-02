using Sandbox;
using Sandbox.Network;
public sealed class DamageBox : Component, Component.ITriggerListener, IActor
{
	[Sync] public int PlayerId { get; set; }
	
	public void OnTriggerEnter(Collider other)
	{
		if ( other.Components.Get<IPlayer>() == null)
			return;
		var myOwner = GameObject.Network.Owner;
		var otherOwner = other.GameObject.Network.Owner;
		if (myOwner == otherOwner) return;
		if (!Network.IsOwner) return;
		{
			var owner = GameObject.Network.Owner;
			Log.Info( $"Server Collide Enter: {other.GameObject.Name} ---  {PlayerId.ToString()} --- {owner.DisplayName} --- {owner.Id} --- {other.Components.Get<PlayerStateComp>().PlayerId.ToString()}");
		}
	}
}
