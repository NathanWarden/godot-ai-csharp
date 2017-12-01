namespace BehaviorTree
{
	public class Timer : BaseLeaf
	{
		[Godot.Export]
		public float waitTime = 1.0f;
		public float timeRemaining;


		protected override void Execute(float delta)
		{
			timeRemaining -= delta;

			status = timeRemaining <= 0 ? BehaviorStatus.Success : BehaviorStatus.Running;
		}


		internal protected override void ResetNode()
		{
			ResetTimer();
			base.ResetNode();
		}


		void ResetTimer()
		{
			timeRemaining = waitTime;
		}
	}
}