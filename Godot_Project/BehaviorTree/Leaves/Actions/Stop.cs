namespace BehaviorTree
{
	public class Stop : BaseLeaf
	{
		[Godot.Export]
		public float stopRate = 5;

		protected override void Execute(float delta)
		{
			behaviorTree.navigator.Stop(stopRate);
			status = BehaviorStatus.Success;
		}
	}
}