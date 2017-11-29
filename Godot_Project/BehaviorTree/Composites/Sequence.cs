namespace BehaviorTree
{
	public class Sequence : BaseComposite
	{
		int currentNode = 0;

		protected virtual BehaviorStatus GetContinueStatus() { return BehaviorStatus.Success; }
		protected virtual BehaviorStatus GetEndStatus() { return BehaviorStatus.Failure; }

		protected override void ExecuteComposite()
		{
			BehaviorStatus node_state = nodes[currentNode].ProcessLogic();

			if (node_state == GetEndStatus())
			{
				status = GetEndStatus();
				return;
			}

			if (node_state == GetContinueStatus())
			{
				currentNode++;

				if (currentNode >= nodes.Count)
				{
					status = GetContinueStatus();
					return;
				}
				else
				{
					nodes[currentNode].ResetNode();
				}
			}
		}
	}
}
