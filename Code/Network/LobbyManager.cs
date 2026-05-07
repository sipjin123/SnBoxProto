using Sandbox;
using Sandbox.Network;

public sealed class LobbyManager : Component
{
	protected override void OnUpdate()
	{
		if ( Input.Keyboard.Pressed( "1" ) )
		{
			Log.Info( "Start as HOST!" );
			HostGame();
		}
	}

	public void HostGame()
	{
		Log.Info( "Hosting game change scene..." );

		Networking.CreateLobby( new LobbyConfig()
		{
			MaxPlayers = 8, Privacy = LobbyPrivacy.Public, Name = "My Lobby Name"
		});
		Scene.LoadFromFile( "scenes/testerer.scene" );
	}
}
