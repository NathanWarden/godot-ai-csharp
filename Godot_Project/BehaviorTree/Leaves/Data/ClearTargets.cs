namespace BehaviorTree
{
	public class ClearTargets : BaseLeaf
	{
		public string groupName = "Player";


		protected override void Execute(float delta)
		{
			behaviorTree.targets.Remove(groupName);
			status = BehaviorStatus.Success;
		}
	}
}