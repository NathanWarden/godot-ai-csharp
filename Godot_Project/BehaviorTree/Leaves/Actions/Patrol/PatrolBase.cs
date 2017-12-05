using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public enum PatrolEndMode
	{
		Once,
		Loop,
		PingPong,
		Random
	}


	public enum ContinuePatrolMode
	{
		Reset,
		ContinuePrevious,
		NearestNode,
		NearestNextNode,
		Random
	}


	public abstract class PatrolBase<T> : BehaviorTreeNode where T : Node
	{
		[Export] public float waypointThreshold = 2.0f;
		[Export] public bool overrideBaseSpeed;
		[Export] public float patrolSpeed;
		[Export] public PatrolEndMode patrolEndMode = PatrolEndMode.Loop;
		[Export] public ContinuePatrolMode continuePatrolMode;

		int currentPatrolIndex;
		int patrolDirection = 1;

		List<T> patrolTargets = new List<T>();
		Node currentTarget;


		internal protected override void ResetNode()
		{
			float speed;

			if (overrideBaseSpeed)
			{
				speed = patrolSpeed;
			}
			else
			{
				speed = behaviorTree.navigator.GetBaseMovementSpeed();
			}

			behaviorTree.navigator.SetMovementSpeed(speed);

			HandleContinuePatrolMode();

			if (currentTarget != null)
			{
				behaviorTree.navigator.TargetUpdated(currentTarget);
			}

			base.ResetNode();

			GD.Print("----- " + status);
		}


		void HandleContinuePatrolMode()
		{
			switch (continuePatrolMode)
			{
				case ContinuePatrolMode.Reset:
					patrolDirection = 1;
					currentPatrolIndex = 0;
					break;

				/*case ContinuePatrolMode.NearestNode:
				case ContinuePatrolMode.NearestNextNode:
					Vector3 thisPos = behaviorTree.navigator.GetPosition();
					int nearestNode = 0;
					float nearestSqrMagnitude = Mathf.Infinity;

					for (int i = 1; i < patrolPoints.Length; i++)
					{
						float thisSqrMagnitude = (thisPos - patrolPoints[i].position).sqrMagnitude;

						if (CheckIfWithinThreshold(thisPos, patrolPoints[i].position, nearestSqrMagnitude))
						{
							nearestSqrMagnitude = thisSqrMagnitude;
							nearestNode = i;
						}
					}

					if (continuePatrolMode == ContinuePatrolMode.NearestNode)
						currentPatrolIndex = nearestNode;
					else if (patrolPoints.Length != 0)
						currentPatrolIndex = (nearestNode + 1) % patrolPoints.Length;

					break;

				case ContinuePatrolMode.ContinuePrevious:
					break;

				case ContinuePatrolMode.Random:
					currentPatrolIndex = Random.Range(0, patrolPoints.Length);
					break;*/
			}
		}


		protected override void Execute(float delta)
		{
			if (patrolTargets.Count > 0)
			{
				if (currentTarget == null)
				{
					currentTarget = patrolTargets[0];
					behaviorTree.navigator.TargetUpdated(currentTarget);
					status = BehaviorStatus.Running;
					return;
				}
				else
				{
					//status = CheckPoints();
					return;
				}
			}

			GD.Print("*** Success!");
			//status = BehaviorStatus.Success;
		}


		public override void UpdateChildNodes()
		{
			InitPatrolPoints();
		}


		protected abstract float GetSqrDistanceToNode(T patrolNode);


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
						behaviorTree.navigator.TargetUpdated(newTarget);
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