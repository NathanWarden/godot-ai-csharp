using Godot;

namespace BehaviorTree
{
	public class RotateToTarget3D : RotateToTargetBase<Spatial>
	{
		protected override void Rotate(float delta, Spatial target)
		{
			Spatial agent = behaviorTree.navigator as Spatial;
			Vector3 r = agent.Rotation;
			Vector3 targetPoint = target.Translation;
			Vector3 angleDifference;

			agent.LookAt(targetPoint, new Vector3(0, 1, 0));
			angleDifference = r - agent.Rotation;
			agent.Rotation = r.LinearInterpolate(agent.Rotation, delta * rotationLerpRate);

			if (Mathf.Rad2Deg(angleDifference.LengthSquared()) < sqrSuccessAngle)
			{
				status = BehaviorStatus.Success;
			}
		}
	}
}