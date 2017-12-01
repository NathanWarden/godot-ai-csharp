using Godot;
using System.Collections.Generic;

namespace BehaviorTree
{
	public abstract class BehaviorTreeNode : Node
	{
		[Export] public bool enabled = true;
		public BehaviorStatus status = BehaviorStatus.Running;
		protected BehaviorTree behaviorTree;


		public override void _Ready()
		{
			ResetNode();
		}


		internal protected virtual BehaviorStatus ProcessLogic(float delta)
		{
			Execute(delta);
			return GetStatus();
		}


		protected abstract void Execute(float delta);


		internal protected virtual void ResetNode()
		{
			status = BehaviorStatus.Running;
		}


		public BehaviorStatus GetStatus()
		{
			return status;
		}


		internal void SetBehaviorTree(BehaviorTree pBehaviorTree)
		{
			behaviorTree = pBehaviorTree;
		}


		public virtual void UpdateChildNodes() {}


		public List<T> GetChildNodesByType<T>() where T : BehaviorTreeNode
		{
			List<T> nodes = new List<T>();
			int nodeCount = GetChildCount();

			for (int i = 0; i < nodeCount; i++)
			{
				T node = GetChild(i) as T;

				if ( node != null && node.enabled )
				{
					nodes.Add(node);
				}
			}

			return nodes;
		}
	}
}