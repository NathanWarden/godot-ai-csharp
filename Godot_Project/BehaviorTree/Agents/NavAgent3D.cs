using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public class NavAgent3D : Spatial, INavAgent
	{
		[Export]
		float moveSpeed = 5;

		Navigation navigation;
		Spatial navTarget = null;
		List<Vector3> path = new List<Vector3>();

		float nextPathUpdate = -1;
		float time = 0;


		public override void _Ready()
		{
			navigation = GetParent().GetParent() as Navigation;
		}


		public override void _Process(float delta)
		{
			time += delta;
			MoveAlongPath(delta);
		}


		public void TargetUpdated(Node target)
		{
			navTarget = target as Spatial;
		}


		public Vector3 GetPosition()
		{
			return Translation;
		}


		void UpdatePath()
		{
			if (time > nextPathUpdate && navTarget != null)
			{
				path = new List<Vector3>(navigation.GetSimplePath(Translation, navTarget.Translation, false));
				nextPathUpdate = time + 0.1f;
			}
		}


		void MoveAlongPath(float delta)
		{
			UpdatePath();

			while (path.Count > 0 && Translation.DistanceSquaredTo(path[0]) < 4)
			{
				path.RemoveAt(0);
			}

			if (path.Count > 0)
			{
				int lastFrameDrawn = Engine.GetFramesDrawn();
				Vector3 targetPoint = path[0];
				Vector3 moveDirection = (targetPoint - this.Translation).Normalized();

				Vector3 r = Rotation;
				LookAt(targetPoint, new Vector3(0, 1, 0));
				Vector3 rd = Rotation;

				Translation += moveDirection * delta * moveSpeed;
				UpdatePath();
			}
		}
	}
}