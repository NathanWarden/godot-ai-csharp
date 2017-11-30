using Godot;


namespace BehaviorTree
{
	public interface INavAgent
	{
		void TargetUpdated(Node target);
		Vector3 GetPosition();
	}
}