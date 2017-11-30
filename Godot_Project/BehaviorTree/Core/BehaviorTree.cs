using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public class BehaviorTree : Node
	{
		[Export]
		public bool resetOnFailure;

		public INavAgent navigator;

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
			if ( rootNode.ProcessLogic(delta) != BehaviorStatus.Running && resetOnFailure )
			{
				rootNode.ResetNode();
			}
		}


		void UpdateBehaviorTree()
		{
			if (rootNode == null)
			{
				rootNode = GetRootNode<BehaviorTreeNode>();
			}

			if (navigator == null)
			{
				navigator = GetRootNode<INavAgent>();
			}

			if (rootNode != null)
			{
				rootNode.UpdateChildNodes();
				SetBehaviorTreeOnChildren(rootNode);
			}
			else
			{
				GD.Print("The Behavior Tree '" + GetName() + "' needs a root BehaviorTreeNode!");
			}
		}


		T GetRootNode<T>()
		{
			object[] nodes = GetChildren();

			for (int i = 0; i < nodes.Length; i++)
			{
				if ( nodes[i] is T )
				{
					return (T)nodes[i];
				}
			}

			return default(T);
		}


		void SetBehaviorTreeOnChildren(BehaviorTreeNode node)
		{
			List<BehaviorTreeNode> nodes = node.GetChildNodesByType<BehaviorTreeNode>();

			node.SetBehaviorTree(this);

			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].SetBehaviorTree(this);
				SetBehaviorTreeOnChildren(nodes[i]);
			}
		}
	}
}