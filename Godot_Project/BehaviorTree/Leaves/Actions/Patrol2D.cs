using Godot;


namespace BehaviorTree
{
	public class Patrol2D : PatrolBase<Node2D>
	{
		protected override Vector3 GetNodePosition(Node2D patrolNode)
		{
			return patrolNode.Position.ToVector3();
		}
	}
}