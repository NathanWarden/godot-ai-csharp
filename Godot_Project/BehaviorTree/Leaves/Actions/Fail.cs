namespace BehaviorTree
{
	public abstract class Fail : BaseLeaf
	{
		protected override void Execute(float delta)
		{
			status = BehaviorStatus.Failure;
		}
	}
}