using Godot;


namespace BehaviorTree
{
	public interface INavAgent
	{
		float GetBaseMovementSpeed();
		float GetMovementSpeed();
		void SetMovementSpeed(float speed);

		void SetTarget(Node target);
		Vector3 GetPosition();
	}
}