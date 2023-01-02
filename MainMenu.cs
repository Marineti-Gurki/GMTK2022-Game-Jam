using Godot;
using System;

public class MainMenu : Control
{
	bool Confirm = false;
	private void _on_Play_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Game.tscn");
	}
	private void _on_Settings_pressed()
	{
		GetTree().ChangeScene("res://Settings.tscn");
	}
	private void _on_Exit_pressed()
	{
		ConfirmationDialog confirmationDialog = GetNode<ConfirmationDialog>("ConfirmationDialog");
		confirmationDialog.Show();
	}

	private void _on_ConfirmationDialog_confirmed()
	{
		Confirm = true;
		if (Confirm)
		{
			GetTree().Quit();
		}
	}
}
