using Godot;

namespace BehaviorTree
{
	public class Seek3D : SeekBase<Spatial>
	{
		protected override Vector3 GetNodePosition(Spatial target)
		{
			return target.Translation;
		}
	}
}