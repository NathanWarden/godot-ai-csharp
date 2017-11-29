namespace BehaviorTree
{
	public class Selector : Sequence
	{
		protected override BehaviorStatus GetContinueStatus() { return BehaviorStatus.Failure; }
		protected override BehaviorStatus GetEndStatus() { return BehaviorStatus.Success; }
	}
}