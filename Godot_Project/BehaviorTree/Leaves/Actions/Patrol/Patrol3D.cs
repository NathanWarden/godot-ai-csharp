using Godot;


namespace BehaviorTree
{
	public class Patrol3D : PatrolBase<Spatial>
	{
		protected override float GetSqrDistanceToNode(Spatial patrolNode)
		{
			GD.Print(behaviorTree.navigator.GetPosition().DistanceTo(patrolNode.Translation));
			return behaviorTree.navigator.GetPosition().DistanceSquaredTo(patrolNode.Translation);
		}
	}
}