using Godot;

namespace BehaviorTree
{
	public class WithinDistance3D : WithinDistanceBase<Spatial>
	{
		protected override Vector3 GetNodePosition(Spatial node)
		{
			return node.Translation;
		}
	}
}