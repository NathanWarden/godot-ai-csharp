using Godot;

namespace BehaviorTree
{
	public abstract class MovementActionBase : BaseLeaf
	{
		[Export] public bool overrideBaseSpeed;
		[Export] public float speed;

		internal protected override void ResetNode()
		{
			float newSpeed;

			if (overrideBaseSpeed)
			{
				newSpeed = speed;
			}
			else
			{
				newSpeed = behaviorTree.navigator.GetBaseMovementSpeed();
			}

			behaviorTree.navigator.SetMovementSpeed(newSpeed);

			base.ResetNode();
		}
	}
}