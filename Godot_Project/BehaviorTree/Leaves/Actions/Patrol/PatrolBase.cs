using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public abstract class PatrolBase<T> : BehaviorTreeNode where T : Node
	{
		[Export]
		public float waypoint_threshold = 2.0f;

		[Export]
		public LoopEndMode patrol_end_mode;

		[Export]
		public bool overrideBaseSpeed;

		[Export]
		public float patrolSpeed;

		int current_patrol_index;
		int patrol_direction = 1;

		List<T> patrol_targets = new List<T>();
		Node current_target;


		public enum LoopEndMode
		{
			Loop,
			Ping_Pong
		}


		internal protected override void EnterNode()
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
		}


		protected override void Execute(float delta)
		{
			if (patrol_targets.Count > 0)
			{
				if (current_target == null)
				{
					current_target = patrol_targets[0];
					behaviorTree.navigator.TargetUpdated(current_target);
					status = BehaviorStatus.Running;
					return;
				}
				else
				{
					status = CheckPoints();
					return;
				}
			}

			status = BehaviorStatus.Success;
		}


		internal protected override void ResetNode()
		{
			current_patrol_index = 0;
			current_target = null;
			base.ResetNode();
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
					_add_target(node as T);
				}
			}
		}


		void _add_target(T target)
		{
			if (!patrol_targets.Contains(target))
			{
				patrol_targets.Add(target);
			}
		}


		BehaviorStatus CheckPoints()
		{
			if (behaviorTree.navigator == null)
			{
				return BehaviorStatus.Failure;
			}

			if (patrol_targets.Count > 0)
			{
				float sqrDistance = GetSqrDistanceToNode(patrol_targets[current_patrol_index]);
				float sqrDistanceThreshold = waypoint_threshold * waypoint_threshold;

				if (sqrDistance <= sqrDistanceThreshold)
				{
					Node new_target;
					bool patrol_ended = false;

					current_patrol_index += patrol_direction;

					if (patrol_direction == 1)
					{
						patrol_ended = current_patrol_index >= patrol_targets.Count;
					}
					else if (patrol_direction == -1)
					{
						patrol_ended = current_patrol_index < 0;
					}

					if (patrol_ended && patrol_end_mode == LoopEndMode.Ping_Pong)
					{
						current_patrol_index -= patrol_direction;
						patrol_direction *= -1;
					}

					current_patrol_index = current_patrol_index % patrol_targets.Count;

					new_target = get_current_target();

					if (current_target != new_target)
					{
						current_target = new_target;
						behaviorTree.navigator.TargetUpdated(new_target);
					}
				}
			}

			return BehaviorStatus.Running;
		}


		Node get_current_target()
		{
			return patrol_targets[current_patrol_index];
		}
	}
}