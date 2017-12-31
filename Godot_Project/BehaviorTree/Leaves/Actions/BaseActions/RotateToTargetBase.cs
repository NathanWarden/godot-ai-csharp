using Godot;

namespace BehaviorTree
{
	public abstract class RotateToTargetBase<T> : BaseLeaf where T : Node
	{
		[Export] public string targetGroup = "Player";
		[Export] public float rotationLerpRate = 5.0f;
		[Export] public float successAngle = 1.0f;
		protected float sqrSuccessAngle;


		internal protected override void ResetNode()
		{
			base.ResetNode();
			sqrSuccessAngle = successAngle * successAngle;
		}


		protected sealed override void Execute(float delta)
		{
			T target = GetTarget();
			Rotate(delta, target);
		}


		protected abstract void Rotate(float delta, T target);


		protected T GetTarget()
		{
			object[] targets = GetTree().GetNodesInGroup(targetGroup);

			for (int i = 0; i < targets.Length; i++)
			{
				if (targets[i] is T result)
				{
					return result;
				}
			}

			return null;
		}
	}
}