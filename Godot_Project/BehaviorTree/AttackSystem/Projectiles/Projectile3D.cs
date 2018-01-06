using Godot;

namespace BehaviorTree
{
	public class Projectile3D : Spatial, IProjectile
	{
		[Export] public float lifetime = 10.0f;
		float lifetimeRemaining;

		AttackData attackData;


		public override void _Ready()
		{
			Reset();
		}


		public override void _Process(float delta)
		{
			lifetimeRemaining -= delta;
		}


		public void Reset()
		{
			lifetimeRemaining = lifetime;
			Visible = true;
		}


		public void Deactivate()
		{
			Visible = false;
		}


		public bool IsAlive()
		{
			return lifetimeRemaining > 0;
		}


		public void SetAttackData(AttackData data)
		{
			attackData = data;
		}


		public AttackData GetAttackData()
		{
			return attackData;
		}
	}
}
