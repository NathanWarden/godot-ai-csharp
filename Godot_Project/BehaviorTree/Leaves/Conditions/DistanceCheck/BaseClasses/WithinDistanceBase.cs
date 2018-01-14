using Godot;

namespace BehaviorTree
{
	public abstract class WithinDistanceBase<T> : DistanceConditionBase<T> where T : Node
	{
		public WithinDistanceBase()
		{
			mode = DistanceMode.Any;
		}


		protected override bool CheckDistance(Vector3 navAgentPosition, Vector3 targetPosition, float sqrDistanceThreshold)
		{
			return navAgentPosition.DistanceSquaredTo(targetPosition) > sqrDistanceThreshold;
		}
	}
}