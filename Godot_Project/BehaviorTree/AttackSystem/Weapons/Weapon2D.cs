using Godot;

namespace BehaviorTree
{
	public abstract class Weapon2D : Node2D, IWeapon
	{
		[Export] public string weaponName;


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

			damageReceiver.OnDamage(attackData);
		}
	}
}