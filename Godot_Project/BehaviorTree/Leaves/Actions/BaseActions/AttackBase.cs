using Godot;

namespace BehaviorTree
{
	public abstract class AttackBase<T> : MovementActionBase where T : Node
	{
		[Export] public string weaponName = "Weapon Name";
		[Export] public float damage = 1.0f;
		[Export] public string attackGroup = "Player";
		[Export] public string damageMethod = "OnDamage";


		internal protected override void ResetNode()
		{
			base.ResetNode();
		}


		IWeapon GetAttackerNode()
		{
			int childCount = GetChildCount();

			for (int i = 0; i < childCount; i++)
			{
				if (GetChild(i) is IWeapon potentialAttacker)
				{
					return potentialAttacker;
				}
			}

			return null;
		}


		protected override void MovementActionExecute(float delta)
		{
			T target = GetTarget();

			if (target != null)
			{
				AttackData attackData = new AttackData
				{
					attacker = this,
					target = GetTarget(),
					damage = damage,
					method = damageMethod,
					weaponName = weaponName,
					deltaTime = delta
				};

				behaviorTree.navigator.Attack(delta, attackData);
			}
			else
			{
				GD.Print("No target");
			}
		}


		protected T GetTarget()
		{
			object[] targets = GetTree().GetNodesInGroup(attackGroup);

			for (int i = 0; i < targets.Length; i++)
			{
				if (targets[i] is T result)
				{
					return result;
				}
			}

			return null;
		}
	}
}