using Godot;

namespace BehaviorTree
{
	public class BeyondDistance3D : BeyondDistanceBase<Spatial>
	{
		protected override Vector3 GetNodePosition(Spatial node)
		{
			return node.Translation;
		}
	}
}