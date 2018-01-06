using Godot;


namespace BehaviorTree
{
	public static class ExtensionMethods
	{
		public static Vector3 ToVector3(this Vector2 vec2)
		{
			return new Vector3(vec2.x, vec2.y, 0);
		}


		public static T GetNodeInChildrenByType<T>(this Node node) where T : Node
		{
			if (node is T)
			{
				return node as T;
			}

			int childCount = node.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				T result = GetNodeInChildrenByType<T>(node.GetChild(i));

				if (result != null)
				{
					return result;
				}
			}

			return null;
		}


		/*public static Vector3 ToVector3(this Quat quat)
		{
			Vector3 v;

			v.x = 2*(Qw*Vz*Qj + Qi*Vz*Qk - Qw*Vy*Qk + Qi*Vy*Qj) + Vx*(Qw^2 + Qi^2 - Qj^2 - Qk^2);
			v.y = 2*(Qw*Vx*Qk + Qi*Vx*Qj - Qw*Vz*Qi + Qj*Vz*Qk) + Vy*(Qw^2 - Qi^2 + Qj^2 - Qk^2);
			v.z = 2*(Qw*Vy*Qi - Qw*Vx*Qj + Qi*Vx*Qk + Qj*Vy*Qk) + Vz*(Qw^2 - Qi^2 - Qj^2 + Qk^2);
		}*/
	}
}