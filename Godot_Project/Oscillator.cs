using Godot;

public class Oscillator : Spatial
{
	Vector3 startPosition;
	float time = 0;


	public override void _Ready()
	{
		startPosition = this.Translation;
	}


	public override void _Process(float delta)
	{
		time += delta;

		this.Translation = startPosition + new Vector3(1,0,0) * Mathf.Sin(time * 0.2f) * 15;
	}
}