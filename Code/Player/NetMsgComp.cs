using Sandbox;

public sealed class NetMsgComp : Component
{
	// CLIENT → SERVER
	public void SendRequestToServer()
	{
		RpcSendToServer();
	}

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
}
