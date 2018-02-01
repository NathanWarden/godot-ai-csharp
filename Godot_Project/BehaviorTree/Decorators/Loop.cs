using Godot;


namespace BehaviorTree
{
	public class Loop : BaseDecorator
	{
		[Export] public int iterations;
		[Export] public bool infinite;

		int currentIteration;


		protected override void Execute(float delta)
		{
			if (leafNode == null)
			{
				status = BehaviorStatus.Success;
				return;
			}

			BehaviorStatus nodeState = leafNode.ProcessLogic(delta);

			if (nodeState == BehaviorStatus.Success)
			{
				if (currentIteration >= iterations && !infinite)
				{
					status = BehaviorStatus.Success;
					return;
				}

				currentIteration++;

				leafNode.ResetNode();
			}
			else if (nodeState == BehaviorStatus.Failure)
			{
				status = BehaviorStatus.Failure;
				return;
			}

			status = BehaviorStatus.Running;
		}
	}
}