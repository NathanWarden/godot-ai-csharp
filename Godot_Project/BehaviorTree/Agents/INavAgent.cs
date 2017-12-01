using Godot;


namespace BehaviorTree
{
	public interface INavAgent
	{
		float GetBaseMovementSpeed();
		void SetMovementSpeed(float speed);

		void TargetUpdated(Node target);
		Vector3 GetPosition();
		void Stop(float stopRate);
	}
}