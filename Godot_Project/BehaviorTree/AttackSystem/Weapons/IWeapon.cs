namespace BehaviorTree
{
	public interface IWeapon
	{
		string GetWeaponName();
		bool CanAttack();
		void ProcessAttack(AttackData attackData);
	}
}