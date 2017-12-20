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


		public void Attack(AttackData attackData)
		{
			IDamageReceiver damageReceiver = attackData.target as IDamageReceiver;

			damageReceiver.OnDamage(attackData);
		}
	}
}