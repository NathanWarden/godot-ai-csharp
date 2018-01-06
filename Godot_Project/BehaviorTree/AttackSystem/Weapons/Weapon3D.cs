using Godot;

namespace BehaviorTree
{
	public abstract class Weapon3D : Spatial, IWeapon
	{
		[Export] public string weaponName = "Weapon Name";


		public string GetWeaponName()
		{
			return weaponName;
		}


		public bool CanAttack()
		{
			return true;
		}


		public virtual void ProcessAttack(AttackData attackData)
		{
			IDamageReceiver damageReceiver = attackData.target as IDamageReceiver;

			if (damageReceiver != null)
			{
				damageReceiver.OnDamage(attackData);
			}
			else
			{
				if (attackData.target.HasMethod(attackData.method))
				{
					attackData.target.Call(attackData.method, attackData);
				}
			}
		}
	}
}