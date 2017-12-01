using System.Collections.Generic;


namespace BehaviorTree
{
	public abstract class BaseDecorator : BehaviorTreeNode
	{
		protected BehaviorTreeNode leafNode;


		public override void UpdateChildNodes()
		{
			List<BehaviorTreeNode> nodes = GetChildNodesByType<BehaviorTreeNode>();
			int nodeCount = nodes.Count;

			if (nodeCount == 0)
			{
				Godot.GD.Print("Warning: a Decorator node needs one (and only one) Leaf node. '" + GetName() + "' will automatically return SUCCESS");
				leafNode = null;
			}
			else
			{
				leafNode = nodes[0];

				if (nodeCount > 1)
				{
					Godot.GD.Print("Warning: a Decorator node only accepts one sub node, only the first one will be used: '" + GetName() + "' will only use '" + leafNode.GetName() + "'");
				}
			}

			for (int i = 0; i < nodeCount; i++)
			{
				nodes[i].UpdateChildNodes();
			}

			base.UpdateChildNodes();
		}


		internal protected override void ResetNode()
		{
			if (leafNode != null)
			{
				leafNode.ResetNode();
			}

			base.ResetNode();
		}
	}
}