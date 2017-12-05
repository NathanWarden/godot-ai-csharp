using Godot;

namespace BehaviorTree
{
	public class WithinDistance2D : WithinDistanceBase<Node2D>
	{
		protected override Vector3 GetNodePosition(Node2D node)
		{
			return node.Position.ToVector3();
		}
	}
}