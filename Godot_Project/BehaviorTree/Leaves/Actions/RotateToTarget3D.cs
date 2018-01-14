using Godot;

namespace BehaviorTree
{
	public class RotateToTarget3D : RotateToTargetBase<Spatial>
	{
		Spatial agent;
		Vector3 startRotation;


		internal protected override void ResetNode()
		{
			base.ResetNode();
			agent = behaviorTree.navigator as Spatial;
			startRotation = agent.Rotation;
		}


		protected override void Rotate(float weight, Spatial target)
		{
			Vector3 targetPoint = target.Translation;

			agent.LookAt(targetPoint, new Vector3(0, 1, 0));
			Vector3 targetRotation = agent.Rotation;
			agent.Rotation = startRotation.LinearInterpolate(agent.Rotation, weight);

			if (weight >= 1.0f)
			{
				GD.Print("Within angle!");
				status = BehaviorStatus.Success;
			}
		}
	}
}