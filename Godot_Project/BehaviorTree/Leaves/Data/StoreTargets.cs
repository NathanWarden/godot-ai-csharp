using Godot;


namespace BehaviorTree
{
	public enum StoreSuccessCondition
	{
		FailIfNoneFound,
		ContinueUntilFound,
		AlwaysSuccess
	}


	public class StoreTargets : BehaviorTreeNode
	{
		[Export] public string groupName = "Player";
		[Export] public StoreSuccessCondition storeSuccessCondition = StoreSuccessCondition.FailIfNoneFound;


		protected override void Execute(float delta)
		{
			object[] targets = GetTargets();

			behaviorTree.targets[groupName] = targets;

			if (targets.Length == 0 && storeSuccessCondition == StoreSuccessCondition.FailIfNoneFound)
			{
				status = BehaviorStatus.Failure;
			}
			else if (targets.Length > 0 || storeSuccessCondition == StoreSuccessCondition.AlwaysSuccess)
			{
				status = BehaviorStatus.Success;
			}
		}


		protected virtual object[] GetTargets()
		{
			return GetTree().GetNodesInGroup(groupName);
		}
	}
}