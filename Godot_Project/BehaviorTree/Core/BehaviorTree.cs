using Godot;

namespace BehaviorTree
{
	public class BehaviorTree : Node
	{
		[Export]
		public bool resetOnFailure;

		BehaviorTreeNode rootNode;


		public override void _Ready()
		{
			UpdateBehaviorTree();

			if (rootNode != null)
			{
				rootNode.ResetNode();
			}
		}


		public override void _Process(float delta)
		{
			if ( rootNode.ProcessLogic() != BehaviorStatus.Running && resetOnFailure )
			{
				rootNode.ResetNode();
			}
		}


		void UpdateBehaviorTree()
		{
			if (rootNode == null)
			{
				rootNode = GetRootNode();
			}

			if (rootNode != null)
			{
				rootNode.UpdateChildNodes();
				//set_behavior_tree_on_children(root_node);
			}
			else
			{
				GD.Print("The Behavior Tree '" + GetName() + "' needs a root BehaviorTreeNode!");
			}
		}


		BehaviorTreeNode GetRootNode()
		{
			object[] nodes = GetChildren();

			for (int i = 0; i < nodes.Length; i++)
			{
				if ( nodes[i] is BehaviorTreeNode )
				{
					return (BehaviorTreeNode)nodes[i];
				}
			}

			return null;
		}
	}
}