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
			PlayerStateComp otherComp = other.Components.Get<PlayerStateComp>();
			var owner = GameObject.Network.Owner;
			Log.Info( $"Server Collide Enter: {other.GameObject.Name} ---  {PlayerId.ToString()} --- {owner.DisplayName} --- {owner.Id} --- {otherComp.PlayerId.ToString()}");
			otherComp.ApplyDamageRpc(10f);
			Destroy();
		}
	}
}
