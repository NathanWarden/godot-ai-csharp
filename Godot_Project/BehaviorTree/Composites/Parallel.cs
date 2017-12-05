using Godot;

namespace BehaviorTree
{
	public enum ParallelExitCondition
	{
		FirstChild,
		AnyChild,
		AllChildren
	}


	public class Parallel : BaseComposite
	{
		[Export] public ParallelExitCondition successCondition = ParallelExitCondition.AnyChild;
		[Export] public ParallelExitCondition failureCondition = ParallelExitCondition.AllChildren;


		protected override void ExecuteComposite(float delta)
		{
			int successes = 0;
			int failures = 0;

			if (successCondition == ParallelExitCondition.FirstChild)
			{
				if (nodes[0].GetStatus() == BehaviorStatus.Success)
				{
					status = BehaviorStatus.Success;
					return;
				}
			}

			if (failureCondition == ParallelExitCondition.FirstChild)
			{
				if (nodes[0].GetStatus() == BehaviorStatus.Failure)
				{
					status = BehaviorStatus.Failure;
					return;
				}
			}

			for (int i = 0; i < nodes.Count; i++)
			{
				BehaviorStatus nodeState = nodes[i].GetStatus();

				if (nodeState == BehaviorStatus.Running)
				{
					nodes[i].ProcessLogic(delta);
				}
				else
				{
					if (nodeState == BehaviorStatus.Success)
					{
						if (successCondition == ParallelExitCondition.AnyChild)
						{
							status = BehaviorStatus.Success;
							return;
						}
						else
						{
							successes++;
						}
					}
					else if (nodeState == BehaviorStatus.Failure)
					{
						if (failureCondition == ParallelExitCondition.AnyChild)
						{
							status = BehaviorStatus.Failure;
							return;
						}
						else
						{
							failures++;
						}
					}
				}
			}

			if (successCondition == ParallelExitCondition.AllChildren && successes == nodes.Count)
			{
				status = BehaviorStatus.Success;
			}

			if (failureCondition == ParallelExitCondition.AllChildren && failures == nodes.Count)
			{
				status = BehaviorStatus.Failure;
			}
		}
	}
}