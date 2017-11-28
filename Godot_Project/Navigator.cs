using Godot;
using System;
using System.Collections.Generic;


public class Navigator : Spatial
{
    [Export]
    float moveSpeed = 5;

    Navigation navigation;
	Spatial navTarget1;
	Spatial navTarget2;
    List<Vector3> path = new List<Vector3>();

    float nextPathUpdate = -1;
    float time = 0;


	public override void _Ready()
	{
        navigation = GetParent() as Navigation;
		navTarget1 = navigation.GetNode("NavTarget1") as Spatial;
		navTarget2 = navigation.GetNode("NavTarget2") as Spatial;
	}


	public override void _Process(float delta)
	{
        time += delta;
        MoveAlongPath(delta);
	}


    void UpdatePath()
    {
        if (time > nextPathUpdate)
        {
            path = new List<Vector3>(navigation.GetSimplePath(Translation, navTarget1.Translation, false));
            nextPathUpdate = time + 0.1f;
        }
	}


	void MoveAlongPath(float delta)
    {
        UpdatePath();

        while (path.Count > 0 && Translation.DistanceSquaredTo(path[0]) < 4)
        {
            path.RemoveAt(0);
        }

		if (path.Count > 0)
        {
			int lastFrameDrawn = Engine.GetFramesDrawn();
            Vector3 targetPoint = path[0];
            Vector3 moveDirection = (targetPoint - this.Translation).Normalized();

            Vector3 r = Rotation;
            LookAt(targetPoint, new Vector3(0, 1, 0));
            Vector3 rd = Rotation;

            if (r != rd)
            {
//                GD.Print("===============");
//                GD.Print(r);
//                GD.Print(rd);
            }

            Translation += moveDirection * delta * moveSpeed;
            UpdatePath();
        }
        else
        {
            Spatial temp = navTarget1;
            navTarget1 = navTarget2;
            navTarget2 = temp;
        }
	}
}