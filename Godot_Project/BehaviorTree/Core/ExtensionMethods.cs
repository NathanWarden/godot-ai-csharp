using Godot;


namespace BehaviorTree
{
	public static class ExtensionMethods
	{
		public static Vector3 ToVector3(this Vector2 vec2)
		{
			return new Vector3(vec2.x, vec2.y, 0);
		}
	}
}