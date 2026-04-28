using Sandbox;

public static class DebugDrawUtil
{
	public static void Line(Vector3 start, Vector3 end, Color color, float duration = 0f)
	{
		Gizmo.Draw.Line(start, end);
		//DebugOverlay.Line(start, end, color, duration);
	}

	public static void Sphere(Vector3 center, float radius, Color color, float duration = 0f)
	{
		//DebugOverlay.Sphere(new Sphere(center, radius), color, duration);
	}

	public static void Trace(Vector3 start, Vector3 end, float radius, float duration = 0f)
	{
		/*
		// draw path
		DebugOverlay.Line(start, end, Color.Green, duration);

		// draw start/end spheres
		DebugOverlay.Sphere(new Sphere(start, radius), Color.Blue, duration);
		DebugOverlay.Sphere(new Sphere(end, radius), Color.Red, duration);*/
	}
}
