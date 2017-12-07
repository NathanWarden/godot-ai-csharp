using Godot;


namespace BehaviorTree
{
	public class Patrol3D : PatrolBase<Spatial>
	{
		protected override Vector3 GetNodePosition(Spatial patrolNode)
		{
			return patrolNode.Translation;
		}
	}
}