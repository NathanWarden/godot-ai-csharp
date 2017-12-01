namespace BehaviorTree
{
	public class Inverter : BaseDecorator
	{
		protected override void Execute(float delta)
		{
			if (leafNode != null)
			{
				int statusInt = (int)leafNode.ProcessLogic(delta);
				status = (BehaviorStatus)statusInt;
			}
			else
			{
				status = BehaviorStatus.Running;
			}
		}
	}
}