using Sandbox;

public sealed class NetMsgComp : Component
{
	// CLIENT → SERVER
	public void SendRequestToServer()
	{
		RpcSendToServer();
	}
//Rpc.Host
//Rpc.Owner
//[Rpc.Broadcast( NetFlags.Unreliable | NetFlag.OwnerOnly )]
	[Rpc.Broadcast] // goes to server when called from client
	void RpcSendToServer()
	{
		Log.Info("Server received request");

		// SERVER → CLIENT
		RpcSendBackToClient();
	}

	[Rpc.Broadcast]
	void RpcSendBackToClient()
	{
		Log.Info("Client received success!");
	}

	[Rpc.Broadcast] // goes to server when called from client
	public void RpcSendToServerNew()
	{
		Log.Info("FU Server");
	}

	[Rpc.Broadcast] // goes to server when called from client
	public void RpcSendToProxy()
	{
	if (IsProxy)
		Log.Info("FU Proxy");
	}
	[Rpc.Broadcast] // goes to server when called from client
	public void RpcSendToNOTProxy()
	{
	if (!IsProxy)
		Log.Info("FU NOT Proxy");
	}
[Rpc.Broadcast( NetFlags.HostOnly )]
	public void RpcSendToOnlyHOST()
	{
		Log.Info("MSG TO HOST!!!!!");
	}

}
