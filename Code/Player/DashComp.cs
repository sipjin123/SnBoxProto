using Sandbox;

public sealed class DashComp : Component
{
	[Sync]
	public bool IsDashing { get; set; }

	[Property]
	public GameObject ChildObj;

	private Vector3 _dashDirection;
	private TimeUntil _dashEnd;

	public void TryDash()
	{
		// local prediction
		StartDash();

		// notify server
		DashServerRpc( ChildObj.WorldRotation.Forward );
	}

	[Rpc.Broadcast]
	private void DashServerRpc( Vector3 direction )
	{
		if ( Networking.IsHost )
		{
			StartDash( direction );
		}
	}

	private void StartDash( Vector3? direction = null )
	{
		IsDashing = true;

		_dashDirection = direction ?? ChildObj.WorldRotation.Forward;

		_dashEnd = 0.2f;
	}

	protected override void OnUpdate()
	{
		if ( IsDashing )
		{
			WorldPosition +=
				_dashDirection * 1200f * Time.Delta;

			if ( _dashEnd )
			{
				IsDashing = false;
			}
		}
	}
}