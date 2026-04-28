using Sandbox;

public sealed class DebuggerComponent : Component
{
	public void Line(Vector3 start, Vector3 end, Color color, float duration = 0f)
	{
		DebugOverlay.Line(start, end, color, duration);
	}

	public void Sphere(Vector3 center, float radius, Color color, float duration = 0f)
	{
		DebugOverlay.Sphere(new Sphere(center, radius), color, duration);
	}

	public void Trace(Vector3 start, Vector3 end, float radius, float duration = 0f)
	{
		// draw path
		DebugOverlay.Line(start, end, Color.Green, duration);

		// draw start/end spheres
		DebugOverlay.Sphere(new Sphere(start, radius), Color.Blue, duration);
		DebugOverlay.Sphere(new Sphere(end, radius), Color.Red, duration);
	}
}
