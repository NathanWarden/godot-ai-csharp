using Godot;


namespace BehaviorTree
{
	public abstract class SeekBase<T> : MovementActionBase where T : Node
	{
		[Export] public string seekGroup = "";
		[Export] public float targetReachedThreshold = 2;
		[Export] public BehaviorStatus targetReachedStatus = BehaviorStatus.Success;
		[Export] public BehaviorStatus targetNotFoundStatus = BehaviorStatus.Failure;

		T currentTarget;


		internal protected override void ResetNode()
		{
			base.ResetNode();

			object[] nodes = GetTree().GetNodesInGroup(seekGroup);

			currentTarget = FindNearestTarget(nodes);

			if (currentTarget != null)
			{
				behaviorTree.navigator.SetTarget(currentTarget);
			}
			else
			{
				status = targetNotFoundStatus;
			}
		}


		protected override void MovementActionExecute(float delta)
		{
			if (currentTarget != null)
			{
				float sqrDistanceThreshold = targetReachedThreshold * targetReachedThreshold;

				if (behaviorTree.navigator.GetPosition().DistanceSquaredTo(GetNodePosition(currentTarget)) < sqrDistanceThreshold)
				{
					status = targetReachedStatus;
				}
			}
		}


		T FindNearestTarget(object[] nodes)
		{
			Vector3 navigatorPosition = behaviorTree.navigator.GetPosition();
			float nearestSqrTargetDistance = float.MaxValue;
			T nearestTarget = null;

			for (int i = 0; i < nodes.Length; i++)
			{
				T potentialTarget = nodes[i] as T;

				if (potentialTarget != null)
				{
					Vector3 targetPosition = GetNodePosition(potentialTarget);
					float sqrDistance = navigatorPosition.DistanceSquaredTo(targetPosition);

					if (sqrDistance < nearestSqrTargetDistance)
					{
						nearestSqrTargetDistance = sqrDistance;
						nearestTarget = potentialTarget;
					}
				}
			}

			return nearestTarget;
		}


		protected abstract Vector3 GetNodePosition(T target);
	}
}