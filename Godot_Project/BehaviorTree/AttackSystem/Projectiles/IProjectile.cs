namespace BehaviorTree
{
	public interface IProjectile
	{
		void Reset();
		void Deactivate();
		bool IsAlive();
		void SetAttackData(AttackData attackData);
		AttackData GetAttackData();
	}
}