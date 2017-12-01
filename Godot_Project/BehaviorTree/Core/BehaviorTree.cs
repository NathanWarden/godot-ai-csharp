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
		BehaviorStatus status = BehaviorStatus.Running;


		public override void _Ready()
		{
			UpdateBehaviorTree();

			if (rootNode != null)
			{
				rootNode.ResetNode();
				rootNode.EnterNode();
			}
		}


		public override void _Process(float delta)
		{
			try
			{
				if (status == BehaviorStatus.Running)
				{
					status = rootNode.ProcessLogic(delta);

					if (status != BehaviorStatus.Running && resetOnFailure)
					{
						rootNode.ResetNode();
						status = BehaviorStatus.Running;
					}
				}
			}
			catch (System.Exception e)
			{
				GD.Print(e.Message);
				GD.Print(e.StackTrace);
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