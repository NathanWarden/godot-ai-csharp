namespace BehaviorTree
{
	public interface IWeapon
	{
		string GetWeaponName();
		void Attack(AttackData attackData);
	}
}