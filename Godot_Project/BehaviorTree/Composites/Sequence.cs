namespace BehaviorTree
{
	public class Sequence : BaseComposite
	{
		int currentNode;

		protected virtual BehaviorStatus GetContinueStatus() { return BehaviorStatus.Success; }
		protected virtual BehaviorStatus GetEndStatus() { return BehaviorStatus.Failure; }


		internal protected override void ResetNode()
		{
			currentNode = 0;

			if (nodes.Count > 0)
			{
				nodes[currentNode].ResetNode();
			}

			base.ResetNode();
		}


		protected override void ExecuteComposite(float delta)
		{
			BehaviorStatus nodeState = nodes[currentNode].ProcessLogic(delta);

			if (nodeState == GetEndStatus())
			{
				status = nodeState;
				return;
			}

			if (nodeState == GetContinueStatus())
			{
				currentNode++;

				if (currentNode >= nodes.Count)
				{
					status = nodeState;
					return;
				}

				nodes[currentNode].ResetNode();
			}
		}
	}
}