using Godot;
namespace BehaviorTree
{
	public class HasTarget : BaseLeaf
	{
		[Export] public string groupName = "Player";


		protected override void Execute(float delta)
		{
			if(behaviorTree.targets.ContainsKey(groupName) && behaviorTree.targets[groupName].Length > 0)
			{
				status = BehaviorStatus.Success;
			}
			else
			{
				status = BehaviorStatus.Failure;
			}
		}
	}
}