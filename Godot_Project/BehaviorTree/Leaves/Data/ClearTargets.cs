namespace BehaviorTree
{
	public class ClearTargets : BehaviorTreeNode
	{
		public string groupName = "Player";


		protected override void Execute(float delta)
		{
			behaviorTree.targets.Remove(groupName);
			status = BehaviorStatus.Success;
		}
	}
}