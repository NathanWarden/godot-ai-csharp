using Godot;

namespace BehaviorTree
{
	public class BeyondDistance2D : BeyondDistanceBase<Node2D>
	{
		protected override Vector3 GetNodePosition(Node2D node)
		{
			return node.Position.ToVector3();
		}
	}
}