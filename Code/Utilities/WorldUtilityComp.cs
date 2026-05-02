using Sandbox;

public sealed class WorldUtilityComp : Component
{
	protected override void OnUpdate()
	{

	}
}

public static class TransformUtil
{
	public static Vector3 GetPointInFront(Vector3 SourceLoc, Vector3 SourceDir, float distance = 100f)
	{
		return SourceLoc + SourceDir * distance;
	}
}
