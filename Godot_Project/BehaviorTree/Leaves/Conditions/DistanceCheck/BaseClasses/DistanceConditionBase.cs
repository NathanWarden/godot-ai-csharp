using Godot;
using System.Collections.Generic;

namespace BehaviorTree
{
	public enum DistanceMode
	{
		Any,
		All
	}


	public abstract class DistanceConditionBase<T> : BaseLeaf where T : Node
	{
		[Export]
		public float distance = 1.0f;

		[Export]
		public DistanceMode mode = DistanceMode.Any;

		[Export]
		public string targetGroup = "";

		T[] targets = new T[0];

		internal protected override void ResetNode()
		{
			CollectTargets();
			base.ResetNode();
		}


		void CollectTargets()
		{
			List<Node> targetNodes = new List<Node>();
			object[] nodesInGroup = GetTree().GetNodesInGroup(targetGroup);

			for (int i = 0; i < targetNodes.Count; i++)
			{
				targetNodes.Add(targetNodes[i] as T);
			}
		}


		protected override void Execute(float delta)
		{
			status = CheckDistances();
		}


		BehaviorStatus CheckDistances()
		{
			float sqrDistance = distance * distance;
			int targetCount;
			int successCount = 0;

			targetCount = targets.Length;

			for (int i = 0; i < targetCount; i++)
			{
				bool result = CheckDistance(behaviorTree.navigator.GetPosition(), GetNodePosition(targets[i]), sqrDistance);

				if (!result && mode == DistanceMode.All)
				{
					return BehaviorStatus.Failure;
				}

				if (result)
				{
					if (mode == DistanceMode.Any)
					{
						return BehaviorStatus.Success;
					}

					successCount++;

					if (successCount == targetCount)
					{
						return BehaviorStatus.Success;
					}
				}
			}

			return BehaviorStatus.Failure;
		}


		protected abstract Vector3 GetNodePosition(T node);
		protected abstract bool CheckDistance(Vector3 navAgentPosition, Vector3 targetPosition, float sqrDistanceThreshold);
	}
}