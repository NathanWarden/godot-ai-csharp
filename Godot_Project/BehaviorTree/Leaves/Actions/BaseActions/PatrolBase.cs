using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public enum PatrolEndMode
	{
		Loop,
		PingPong,
		Random,
		Once
	}


	public enum ContinuePatrolMode
	{
		ContinuePrevious,
		NearestNode,
		Random,
		Reset
	}


	public abstract class PatrolBase<T> : MovementActionBase where T : Node
	{
		[Export] public float waypointThreshold = 3.0f;
		[Export] public PatrolEndMode patrolEndMode = PatrolEndMode.Loop;
		[Export] public ContinuePatrolMode continuePatrolMode = ContinuePatrolMode.ContinuePrevious;

		int currentPatrolIndex;
		int patrolDirection = 1;

		List<T> patrolTargets = new List<T>();
		Node currentTarget;


		protected abstract Vector3 GetNodePosition(T patrolNode);


		internal protected override void ResetNode()
		{
			currentTarget = null;
			base.ResetNode();
		}


		void HandleContinuePatrolMode()
		{
			switch (continuePatrolMode)
			{
				case ContinuePatrolMode.Reset:
					patrolDirection = 1;
					currentPatrolIndex = 0;
					break;

				case ContinuePatrolMode.NearestNode:
					Vector3 thisPos = behaviorTree.navigator.GetPosition();
					int nearestNode = 0;
					float nearestSqrMagnitude = float.MaxValue;

					for (int i = 1; i < patrolTargets.Count; i++)
					{
						Vector3 targetPosition = GetNodePosition(patrolTargets[i]);
						float thisSqrMagnitude = thisPos.DistanceSquaredTo(targetPosition);

						if (thisSqrMagnitude < nearestSqrMagnitude)
						{
							nearestSqrMagnitude = thisSqrMagnitude;
							nearestNode = i;
						}
					}

					if (continuePatrolMode == ContinuePatrolMode.NearestNode)
						currentPatrolIndex = nearestNode;
					else if (patrolTargets.Count != 0)
						currentPatrolIndex = (nearestNode + 1) % patrolTargets.Count;

					break;

				case ContinuePatrolMode.ContinuePrevious:
					break;

				case ContinuePatrolMode.Random:
					currentPatrolIndex = new System.Random().Next(0, patrolTargets.Count);
					break;
			}

			currentTarget = patrolTargets[currentPatrolIndex];
		}


		protected override void Execute(float delta)
		{
			if (patrolTargets.Count > 0)
			{
				if (currentTarget == null)
				{
					HandleContinuePatrolMode();

					if (currentTarget != null)
					{
						behaviorTree.navigator.SetTarget(currentTarget);
					}
				}

				if (currentTarget == null)
				{
					currentTarget = patrolTargets[0];
					behaviorTree.navigator.SetTarget(currentTarget);
					status = BehaviorStatus.Running;
					return;
				}

				status = CheckPoints();
				return;
			}
		}


		public override void AssignChildNodes()
		{
			InitPatrolPoints();
		}


		protected float GetSqrDistanceToNode(T patrolNode)
		{
			return behaviorTree.navigator.GetPosition().DistanceSquaredTo(GetNodePosition(patrolNode));
		}


		void InitPatrolPoints()
		{
			int childCount = GetChildCount();

			for (int i = 0; i < childCount; i++)
			{
				Node node = GetChild(i);

				if (node is T)
				{
					AddTarget(node as T);
				}
			}
		}


		void AddTarget(T target)
		{
			if (!patrolTargets.Contains(target))
			{
				patrolTargets.Add(target);
			}
		}


		BehaviorStatus CheckPoints()
		{
			if (behaviorTree.navigator == null)
			{
				GD.Print("*** Failure!");
				return BehaviorStatus.Failure;
			}

			if (patrolTargets.Count > 0)
			{
				float sqrDistance = GetSqrDistanceToNode(patrolTargets[currentPatrolIndex]);
				float sqrDistanceThreshold = waypointThreshold * waypointThreshold;

				if (sqrDistance <= sqrDistanceThreshold)
				{
					Node newTarget;
					bool patrolEnded = false;

					currentPatrolIndex += patrolDirection;

					if (patrolDirection == 1)
					{
						patrolEnded = currentPatrolIndex >= patrolTargets.Count;
					}
					else if (patrolDirection == -1)
					{
						patrolEnded = currentPatrolIndex < 0;
					}

					if (patrolEnded)
					{
						if (patrolEndMode == PatrolEndMode.Once)
						{
							return BehaviorStatus.Success;
						}
						else if (patrolEndMode == PatrolEndMode.PingPong)
						{
							currentPatrolIndex -= patrolDirection;
							patrolDirection *= -1;
						}
					}

					currentPatrolIndex = currentPatrolIndex % patrolTargets.Count;

					newTarget = GetCurrentTarget();

					if (currentTarget != newTarget)
					{
						currentTarget = newTarget;
						behaviorTree.navigator.SetTarget(newTarget);
					}
				}
			}

			return BehaviorStatus.Running;
		}


		Node GetCurrentTarget()
		{
			return patrolTargets[currentPatrolIndex];
		}
	}
}