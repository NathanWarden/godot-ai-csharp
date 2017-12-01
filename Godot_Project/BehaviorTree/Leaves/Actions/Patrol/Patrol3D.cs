using Godot;


namespace BehaviorTree
{
	public class Patrol3D : PatrolBase<Spatial>
	{
		protected override float GetSqrDistanceToNode(Spatial patrolNode)
		{
			return behaviorTree.navigator.GetPosition().DistanceSquaredTo(patrolNode.Translation);
		}
	}
}