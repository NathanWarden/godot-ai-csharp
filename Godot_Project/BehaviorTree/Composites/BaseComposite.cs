using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public abstract class BaseComposite : BehaviorTreeNode
	{
		public List<BehaviorTreeNode> nodes = new List<BehaviorTreeNode>();


		internal protected override void ResetNode()
		{
			foreach (BehaviorTreeNode node in nodes)
			{
				node.ResetNode();
			}

			base.ResetNode();
		}


		protected sealed override void Execute(float delta)
		{
			if (nodes.Count > 0)
			{
				ExecuteComposite(delta);
			}
			else
			{
				status = BehaviorStatus.Success;
			}
		}


		protected abstract void ExecuteComposite(float delta);


		public override void UpdateChildNodes()
		{
			int nodeCount;

			nodes = GetChildNodesByType<BehaviorTreeNode>();
			nodeCount = nodes.Count;

			if (nodeCount == 0)
			{
				GD.Print("Warning: a Composite node needs at least one Composite, Decorator, or Leaf node. '" + GetName() + "' will automatically return success");
			}

			for (int i = 0; i < nodeCount; i++)
			{
				nodes[i].UpdateChildNodes();
			}
		}
	}
}