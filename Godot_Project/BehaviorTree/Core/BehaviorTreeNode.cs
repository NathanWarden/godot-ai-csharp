using Godot;
using System.Collections.Generic;

namespace BehaviorTree
{
	public abstract class BehaviorTreeNode : Node
	{
		public BehaviorStatus status = BehaviorStatus.Running;


		protected abstract void Execute();


		protected internal virtual BehaviorStatus ProcessLogic()
		{
			GD.Print(GetName());

			Execute();

			return GetStatus();
		}


		protected internal virtual void ResetNode()
		{
			status = BehaviorStatus.Running;
		}


		public BehaviorStatus GetStatus()
		{
			return status;
		}


		public virtual void UpdateChildNodes() {}


		public List<T> GetChildNodesByType<T>() where T : BehaviorTreeNode
		{
			List<T> nodes = new List<T>();
			int nodeCount = GetChildCount();

			for (int i = 0; i < nodeCount; i++)
			{
				T node = GetChild(i) as T;

				if ( node != null )
				{
					nodes.Add(node);
				}
			}

			return nodes;
		}
	}
}