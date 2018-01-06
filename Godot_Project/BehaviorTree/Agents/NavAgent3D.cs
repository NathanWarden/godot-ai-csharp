using Godot;
using System.Collections.Generic;


namespace BehaviorTree
{
	public class NavAgent3D : Spatial, INavAgent
	{
		[Export] float baseMoveSpeed = 5;
		float moveSpeed;
		[Export] float rotationLerpRate = 5.0f;

		[Export] string navigationNodePath = "";
		Navigation navigation;

		Spatial navTarget;
		List<Vector3> path = new List<Vector3>();

		float nextPathUpdate = -1;
		float time;

		[Export] public bool deepSearchForWeapons;
		Dictionary<string, IWeapon> weapons = new Dictionary<string, IWeapon>();


		public override void _Ready()
		{
			if ( string.IsNullOrEmpty(navigationNodePath) )
			{
				Node parent = GetParent();

				while (navigation == null && parent != null)
				{
					navigation = parent as Navigation;
					parent = parent.GetParent();
				}

				GD.Print(navigation == null);
			}
			else
			{
				navigation = GetNode(navigationNodePath) as Navigation;
			}

			CollectWeaponsInNode(this, weapons);
		}


		void CollectWeaponsInNode(Node node, Dictionary<string, IWeapon> weaponList)
		{
			int childCount = node.GetChildCount();

			for (int i = 0; i < childCount; i++)
			{
				if (GetChild(i) is IWeapon weapon)
				{
					weaponList[weapon.GetWeaponName()] = weapon;
				}

				if (deepSearchForWeapons)
				{
					CollectWeaponsInNode(GetChild(i), weaponList);
				}
			}
		}


		public override void _Process(float delta)
		{
			time += delta;

			if (navTarget != null)
			{
				MoveAlongPath(delta);
			}
		}


		public Node GetNode()
		{
			return this;
		}


		public float GetBaseMovementSpeed()
		{
			return baseMoveSpeed;
		}


		public float GetMovementSpeed()
		{
			return moveSpeed;
		}


		public void SetMovementSpeed(float speed)
		{
			moveSpeed = speed;
		}


		public void SetTarget(Node target)
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
				Vector3 moveDirection = (targetPoint - Translation).Normalized();
				Vector3 r = Rotation;

				Translation += moveDirection * delta * moveSpeed;
				LookAt(targetPoint, new Vector3(0, 1, 0));
				Rotation = r.LinearInterpolate(Rotation, delta * rotationLerpRate);

				UpdatePath();
			}
		}


		public void Attack(float delta, AttackData attackData)
		{
			if (weapons.ContainsKey(attackData.weaponName))
			{
				if (weapons[attackData.weaponName].CanAttack())
				{
					weapons[attackData.weaponName].ProcessAttack(attackData);
				}
			}
			else
			{
				GD.Print("No weapon found named '" + attackData.weaponName + "'");
			}
		}
	}
}