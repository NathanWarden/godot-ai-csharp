using Godot;

namespace BehaviorTree
{
	public class Stop : BaseLeaf
	{
		// The time it takes before this agent stops
		[Export] public float stopTime = 1;

		float initialSpeed;
		float stoppingTime;


		internal protected override void ResetNode()
		{
			base.ResetNode();

			stoppingTime = 0;
			initialSpeed = behaviorTree.navigator.GetMovementSpeed();
		}


		protected override void Execute(float delta)
		{
			float weight;

			stoppingTime += delta;
			weight = stoppingTime / stopTime;

			if (weight+float.Epsilon <= 1.0f)
			{
				behaviorTree.navigator.SetMovementSpeed(Mathf.Lerp(initialSpeed, 0, weight));
			}
			else
			{
				status = BehaviorStatus.Success;
			}
		}
	}
}