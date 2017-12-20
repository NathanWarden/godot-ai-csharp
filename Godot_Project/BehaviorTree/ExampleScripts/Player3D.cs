using Godot;

namespace BehaviorTree
{
	public class Player3D : Spatial, IDamageReceiver
	{
		public float health = 100.0f;

		public void OnDamage(AttackData attackData)
		{
			health -= attackData.damage * attackData.deltaTime * 10;

			GD.Print(health);

			if (health < 0)
			{
				QueueFree();
			}
		}
	}
}