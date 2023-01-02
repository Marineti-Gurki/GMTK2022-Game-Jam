using Godot;
using System;
using System.Linq;

public class Floor : Spatial
{
	MeshInstance[] FloorMeshes;
	public int CurrentFace;
	CollisionShape FloorCollision;
	public override void _Ready()
	{
		FloorMeshes = GetChildren().Cast<MeshInstance>().ToArray();
	}

	public override void _Process(float delta)
	{
		FloorMeshes[CurrentFace].Hide();
		FloorMeshes[CurrentFace].GetNode<StaticBody>("StaticBody").GetNode<CollisionShape>("CollisionShape").Disabled = true;
		GD.Print(FloorMeshes[CurrentFace]);

		// NOTE: This solution wont work in Godot. Needs to be done via instancing as objects cannot be enabled/disabled during runtime...
	}


	public void RandomFloorDisappear(int FloorCount)
	{
		// for (int i = 0; i < FloorCount; i++)
		// {
		//     FloorMeshes[i].Hide();
		//     FloorMeshes[i].GetNode<StaticBody>("StaticBody").GetNode<CollisionShape>("CollisionShape").Disabled = true;
		// }
		CurrentFace = FloorCount;
		// FloorMeshes[2].Hide();
		// FloorMeshes[2].GetNode<StaticBody>("StaticBody").GetNode<CollisionShape>("CollisionShape").Disabled = true;
	}
}
