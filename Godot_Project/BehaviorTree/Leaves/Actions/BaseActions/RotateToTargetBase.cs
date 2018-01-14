using Godot;

namespace BehaviorTree
{
	public abstract class RotateToTargetBase<T> : BaseLeaf where T : Node
	{
		[Export] public string targetGroup = "Player";
		[Export] public float rotationTime = 0.25f;
		protected float lerpTime;


		internal protected override void ResetNode()
		{
			base.ResetNode();
			lerpTime = 0.0f;
		}


		protected sealed override void Execute(float delta)
		{
			T target = GetTarget();
			lerpTime += delta;
			Rotate(lerpTime/rotationTime, target);
		}


		protected abstract void Rotate(float weight, T target);


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