using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public class BehaviorTree : Node
	{
		/// <summary>
		/// If true and the first node has a failure status the tree will be reset. Otherwise, the AI will cease to function.
		/// </summary>
		[Export] public bool resetOnSuccess = true;
		[Export] public bool resetOnFailure = true;

		[Export] string navigatorPath = "";
		public INavAgent navigator;

		BehaviorTreeNode rootNode;
		BehaviorStatus status = BehaviorStatus.Running;

		public Dictionary<string, object[]> targets = new Dictionary<string, object[]>();


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
			try
			{
				if (status == BehaviorStatus.Running)
				{
					status = rootNode.ProcessLogic(delta);

					if (status != BehaviorStatus.Running)
					{
						bool reset = false;

						if (status == BehaviorStatus.Success && resetOnSuccess)
						{
							reset = true;
						}
						else
						{
							reset = (status == BehaviorStatus.Failure && resetOnFailure);
						}

						if (reset)
						{
							rootNode.ResetNode();
							status = BehaviorStatus.Running;
						}
						else
						{
							SetProcess(false);
						}
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
				if (string.IsNullOrEmpty(navigatorPath))
				{
					navigator = GetRootNode<INavAgent>();
				}
				else
				{
					navigator = GetNode(navigatorPath) as INavAgent;
				}
			}

			if (rootNode != null)
			{
				rootNode.AssignChildNodes();
				AssignBehaviorTreeOnChildren(rootNode);
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


		void AssignBehaviorTreeOnChildren(BehaviorTreeNode node)
		{
			List<BehaviorTreeNode> nodes = node.GetChildNodesByType<BehaviorTreeNode>();

			node.SetBehaviorTree(this);

			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].SetBehaviorTree(this);
				AssignBehaviorTreeOnChildren(nodes[i]);
			}
		}
	}
}