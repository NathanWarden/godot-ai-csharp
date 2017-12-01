using Godot;

namespace BehaviorTree
{
	public class Parallel : BaseComposite
	{
		[Export] public ExitCondition successCondition = ExitCondition.Any_Child;
		[Export] public ExitCondition failureCondition = ExitCondition.All_Children;


		public enum ExitCondition
		{
			First_Child,
			Any_Child,
			All_Children
		}


		protected override void ExecuteComposite(float delta)
		{
			int successes = 0;
			int failures = 0;

			if (successCondition == ExitCondition.First_Child)
			{
				if (nodes[0].GetStatus() == BehaviorStatus.Success)
				{
					status = BehaviorStatus.Success;
					return;
				}
			}

			if (failureCondition == ExitCondition.First_Child)
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
						if (successCondition == ExitCondition.Any_Child)
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
						if (failureCondition == ExitCondition.Any_Child)
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

			if (successCondition == ExitCondition.All_Children && successes == nodes.Count)
			{
				status = BehaviorStatus.Success;
			}

			if (failureCondition == ExitCondition.All_Children && failures == nodes.Count)
			{
				status = BehaviorStatus.Failure;
			}
		}
	}
}