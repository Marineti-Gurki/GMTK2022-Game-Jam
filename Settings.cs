using Godot;
using System;

public class Settings : Control
{
	public override void _Ready()
	{

	}

	private void _on_Exit_pressed()
	{
		GetTree().ChangeScene("res://MainMenu.tscn");
	}
}
