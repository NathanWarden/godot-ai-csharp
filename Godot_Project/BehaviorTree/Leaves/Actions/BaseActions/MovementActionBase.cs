using Godot;

namespace BehaviorTree
{
	public abstract class MovementActionBase : BaseLeaf
	{
		[Export] public bool overrideBaseSpeed;
		[Export] public float speed;


		protected sealed override void Execute(float delta)
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

			MovementActionExecute(delta);
		}


		protected virtual void MovementActionExecute(float delta) {}
	}
}