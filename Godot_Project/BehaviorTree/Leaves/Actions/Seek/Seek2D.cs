using Godot;

namespace BehaviorTree
{
	public class Seek2D : SeekBase<Node2D>
	{
		protected override Vector3 GetNodePosition(Node2D target)
		{
			return target.Position.ToVector3();
		}
	}
}