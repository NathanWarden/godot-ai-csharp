using Godot;

namespace BehaviorTree
{
	public abstract class BeyondDistanceBase<T> : DistanceConditionBase<T> where T : Node
	{
		public BeyondDistanceBase()
		{
			mode = DistanceMode.All;
		}


		protected override bool CheckDistance(Vector3 navAgentPosition, Vector3 targetPosition, float sqrDistanceThreshold)
		{
			return navAgentPosition.DistanceSquaredTo(targetPosition) > sqrDistanceThreshold;
		}
	}
}