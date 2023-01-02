using Godot;
using System;

public class Player : KinematicBody
{
	float Gravity = 98;
	int Speed = 200;
	float Acceleration = 20;
	Vector3 CurrentPos;
	Vector3 TargetPosition = Vector3.Zero;
	Vector3 direction = Vector3.Zero;

	Label AttemptsLbl;
	Spatial Dice;
	Label CurrentRoll;
	AudioStreamPlayer SnapSound;

	int CurrentSide;

	Vector3 oneSide = new Vector3(0, 0, 90);
	Vector3 twoSide = new Vector3(270, 0, 0);
	Vector3 threeSide = new Vector3(0, 0, 270);
	Vector3 fourSide = new Vector3(180, 0, 0);
	Vector3 fiveSide = new Vector3(0, 0, 0);
	Vector3 sixSide = new Vector3(90, 0, 0);
	Vector3[] sides = new Vector3[6];
	int Attempts = 6;

	bool IsMovingX = false;
	bool IsMovingZ = false;
	bool GameOver = false;
	public override void _Ready()
	{
		GD.Randomize();
		CurrentRoll = GetNode<Label>("CurrentRoll");
		AttemptsLbl = GetNode<Label>("../AttemptsLbl");
		SnapSound = GetNode<AudioStreamPlayer>("../AudioStreamPlayer");
		Dice = GetNode<Spatial>("Dice");
		this.Translation = Vector3.Zero;
		sides[0] = oneSide;
		sides[1] = twoSide;
		sides[2] = threeSide;
		sides[3] = fourSide;
		sides[4] = fiveSide;
		sides[5] = sixSide;
		RandomizeDice();
		CurrentRoll.Text = GetDiceSide().ToString();
		Reset();
	}

	public override void _Process(float delta)
	{
		AttemptsLbl.Text = "Attempts:" + Attempts;
	}

	public override void _PhysicsProcess(float delta)
	{
		Movement(delta);
	}

	public void Movement(float delta)
	{
		CurrentPos = Translation;
		if (Input.IsActionPressed("MoveRight") && !IsMovingZ)
		{
			direction = Vector3.Zero;
			direction += Transform.basis.x;
			IsMovingX = true;
		}
		else if (Input.IsActionPressed("MoveLeft") && !IsMovingZ)
		{
			direction = Vector3.Zero;
			direction -= Transform.basis.x;
			IsMovingX = true;
		}
		if (Input.IsActionPressed("MoveUp") && !IsMovingX)
		{
			direction = Vector3.Zero;
			direction -= Transform.basis.z;
			IsMovingZ = true;
		}
		else if (Input.IsActionPressed("MoveDown") && !IsMovingX)
		{
			direction = Vector3.Zero;
			direction += Transform.basis.z;
			IsMovingZ = true;
		}
		var velocity = direction * Speed;
		velocity = velocity.LinearInterpolate(velocity, 2);

		if (IsMovingX)
		{
			RandomizeDice();
			MoveAndSlide(velocity * delta, Vector3.Up);
			if (Mathf.Abs(CurrentPos.x - TargetPosition.x) <= 0.01f || Mathf.Abs(CurrentPos.x - TargetPosition.x) > 1.5f)
			{
				IsMovingX = false;
				SnapSound.Play();
			}
		}
		if (IsMovingZ)
		{
			RandomizeDice();
			MoveAndSlide(velocity * delta, Vector3.Up);
			if (Mathf.Abs(CurrentPos.z - TargetPosition.z) <= 0.01f || Mathf.Abs(CurrentPos.z - TargetPosition.z) > 1.5f)
			{
				IsMovingZ = false;
				SnapSound.Play();
			}
		}
		Vector3 downVec = Vector3.Zero;
		downVec.y -= Gravity * delta;
		MoveAndSlide(downVec, Vector3.Up);
		CurrentRoll.Text = GetDiceSide().ToString();
	}

	public override void _Input(InputEvent input)
	{

		if (input.IsActionPressed("MoveDown"))
		{
			CurrentPos.z = Mathf.Round(CurrentPos.z / 1) * 1;
			TargetPosition.z = (Mathf.Round(CurrentPos.z / 1) * 1) + 1;
			Floor floorRandomiser = new Floor();
			floorRandomiser.RandomFloorDisappear(CurrentSide);
		}
		if (input.IsActionPressed("MoveUp"))
		{
			CurrentPos.z = -Mathf.Round(CurrentPos.z / 1) * 1;
			TargetPosition.z = -(Mathf.Round(CurrentPos.z / 1) * 1) - 1;
			Floor floorRandomiser = new Floor();
			floorRandomiser.RandomFloorDisappear(CurrentSide);
		}
		if (input.IsActionPressed("MoveRight"))
		{
			CurrentPos.x = Mathf.Round(CurrentPos.x / 1) * 1;

			TargetPosition.x = (Mathf.Round(CurrentPos.x / 1) * 1) + 1;
			Floor floorRandomiser = new Floor();
			floorRandomiser.RandomFloorDisappear(CurrentSide);
		}
		if (input.IsActionPressed("MoveLeft"))
		{
			CurrentPos.x = -Mathf.Round(CurrentPos.x / 1) * 1;

			TargetPosition.x = -(Mathf.Round(CurrentPos.x / 1) * 1) - 1;
			Floor floorRandomiser = new Floor();
			floorRandomiser.RandomFloorDisappear(CurrentSide);
		}
	}

	public void RandomizeDice()
	{
		for (int i = 0; i < GD.RandRange(0, sides.Length); i++)
		{
			Dice.RotationDegrees = sides[i];
		}
	}

	public int GetDiceSide()
	{
		Vector3 diceSide;
		for (int i = 0; i < sides.Length; i++)
		{
			if (Dice.RotationDegrees == sides[i])
			{
				diceSide = sides[i];
				if (sides[i] == oneSide)
				{
					CurrentSide = 1;
					return 1;
				}
				if (sides[i] == twoSide)
				{
					CurrentSide = 2;
					return 2;
				}
				if (sides[i] == threeSide)
				{
					CurrentSide = 3;
					return 3;
				}
				if (sides[i] == fourSide)
				{
					CurrentSide = 4;
					return 4;
				}
				if (sides[i] == fiveSide)
				{
					CurrentSide = 5;
					return 5;
				}
				if (sides[i] == sixSide)
				{
					CurrentSide = 6;
					return 6;
				}
			}
		}
		return 0;
	}
	public void Reset()
	{
		var currentPos = Translation;
		currentPos = Vector3.Zero;
		Translation = currentPos;
		// GD.Print(Attempts);
	}
	private void _on_DeathZone_body_entered(object body)
	{
		Reset();
		if (Attempts > 0)
		{
			Attempts -= 1;
		}
		else
		{
			GameOver = true;
		}
	}

	private void _on_Area_body_entered(object body)
	{
		if (body is Player player)
		{
			GetTree().ChangeScene("res://Control.tscn");
		}
	}
	private void _on_Area2_body_entered(object body)
	{
		if (body is Player player)
		{
			GetTree().ChangeScene("res://Control.tscn");
		}
	}
	private void _on_Area3_body_entered(object body)
	{
		if (body is Player player)
		{
			GetTree().ChangeScene("res://Control.tscn");
		}
	}
	private void _on_Area4_body_entered(object body)
	{
		if (body is Player player)
		{
			GetTree().ChangeScene("res://Control.tscn");
		}
	}

}
