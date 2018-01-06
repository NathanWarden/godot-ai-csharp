using Godot;


namespace BehaviorTree
{
	public interface INavAgent
	{
		Node GetNode();

		float GetBaseMovementSpeed();
		float GetMovementSpeed();
		void SetMovementSpeed(float speed);
		void SetTarget(Node target);
		Vector3 GetPosition();

		void Attack(float delta, AttackData attackData);
	}
}