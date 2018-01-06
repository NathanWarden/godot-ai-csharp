using Godot;

namespace BehaviorTree
{
	public struct AttackData
	{
		public BehaviorTreeNode attacker;
		public Node target;
		public float damage;
		public string method;
		public string weaponName;
		public float deltaTime;
	}
}