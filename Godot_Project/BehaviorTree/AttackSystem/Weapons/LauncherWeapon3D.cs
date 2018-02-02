using Godot;
using System.Collections.Generic;

namespace BehaviorTree
{
	public class LauncherWeapon3D : Weapon3D
	{
		[Export] public float launchRate = 1.0f;
		[Export] public PackedScene projectileScene;
		[Export] public bool poolProjectiles = true;

		Dictionary<Spatial, IProjectile> projectiles = new Dictionary<Spatial, IProjectile>();
		Queue<Spatial> pooledProjectiles = new Queue<Spatial>();
		RayCast rayCast;
		float nextSpawnTime;


		public override void _Process(float delta)
		{
			nextSpawnTime -= delta;

			foreach (Spatial projectile in projectiles.Keys)
			{
				HandleProjectile(projectile);
			}
		}


		public override void ProcessAttack(AttackData attackData)
		{
			if (nextSpawnTime < 0.0f)
			{
				Spatial newProjectile;

				nextSpawnTime = launchRate;

				if (pooledProjectiles.Count > 0)
				{
					GD.Print("Got from pool");
					newProjectile = pooledProjectiles.Dequeue();
				}
				else
				{
					GD.Print("Got from instantiate");
					newProjectile = projectileScene.Instance() as Spatial;
				}

				if (newProjectile is IProjectile iProjectile)
				{
					iProjectile.SetAttackData(attackData);
					projectiles[newProjectile] = iProjectile;
					InitProjectile(newProjectile, attackData);
				}
			}
		}


		void InitProjectile(Spatial projectile, AttackData attackData)
		{
			Transform globalTfm = GetGlobalTransform();
			attackData.attacker.behaviorTree.navigator.GetNode().GetParent().AddChild(projectile);
			projectile.SetGlobalTransform(globalTfm);
			projectile.LookAt((attackData.target as Spatial).GlobalTransform.origin, new Vector3(0,1,0));
			(projectile as IProjectile).Reset();
		}


		protected virtual void HandleProjectile(Spatial projectile)
		{
			IProjectile iProjectile = projectiles[projectile];

			if (iProjectile.IsAlive())
			{
				AttackData attackData = iProjectile.GetAttackData();

				Vector3 b = -projectile.Transform.basis.GetAxis(2);
				projectile.Translation += b * attackData.deltaTime * 5;

				rayCast = projectile.GetNodeInChildrenByType<RayCast>();

				if ( rayCast != null && rayCast.IsColliding() )
				{
					if (!(attackData.target is IDamageReceiver))
					{
						Node collider = rayCast.GetCollider() as Node;

						if (collider is IDamageReceiver)
						{
							attackData.target = collider;
						}
					}

					base.ProcessAttack(attackData);
				}
			}
			else
			{
				iProjectile.Deactivate();
				projectiles.Remove(projectile);
				pooledProjectiles.Enqueue(projectile);
			}
		}
	}
}