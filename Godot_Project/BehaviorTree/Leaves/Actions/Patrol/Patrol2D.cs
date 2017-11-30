using Godot;


namespace BehaviorTree
{
	public class Patrol2D : PatrolBase<Node2D>
	{
		protected override float GetSqrDistanceToNode(Node2D patrolNode)
		{
			return behaviorTree.navigator.GetPosition().DistanceSquaredTo(patrolNode.Position.ToVector3());
		}
	}
}