using Sandbox;
using System.Collections.Generic;

public sealed class ScreenLoggerComp : Component
{
	protected override void OnUpdate()
	{
		float y = 100;

		foreach (var msg in Messages)
		{
			DebugOverlay.Text(new Vector2( 50, 50 ), msg);
			y += 20;
		}
	}
	public static List<string> Messages = new();

	public static void Print(string msg)
	{
		Messages.Add(msg);

		if (Messages.Count > 10)
			Messages.RemoveAt(0);
	}
}
