using Godot;
using System;

public partial class Units : Godot.CharacterBody3D
{
	int hp;
	int carryweight;
	int reach;
	public float _movementSpeed = 3.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	NavigationAgent3D _navigationAgent;

	private Vector3 _movementTargetPosition = new Vector3(-3.0f, 0.0f, 2.0f);

	public Vector3 MovementTarget
	{
		get { return _navigationAgent.TargetPosition; }
		set { _navigationAgent.TargetPosition = value; }
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		_navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		// These values need to be adjusted for the actor's speed
		// and the navigation layout.
		_navigationAgent.PathDesiredDistance = 0.5f;
		_navigationAgent.TargetDesiredDistance = 0.5f;

		// Make sure to not await during _Ready.
		Callable.From(ActorSetup).CallDeferred();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (_navigationAgent.IsNavigationFinished())
		{
			return;
		}

		Vector3 currentAgentPosition = GlobalTransform.Origin;
		Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();

		Vector3 newVelocity = (nextPathPosition - currentAgentPosition).Normalized();
		newVelocity *= _movementSpeed;

		Velocity = newVelocity;

		MoveAndSlide();
	}
	private async void ActorSetup()
	{
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		// Now that the navigation map is no longer empty, set the movement target.
		MovementTarget = _movementTargetPosition;
	}
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			var spaceState = GetWorld3D().DirectSpaceState;
			var camera = GetTree().Root.GetCamera3D();
			var rayOrigin = camera.ProjectRayOrigin(mouseButtonEvent.Position);
			var rayEnd = rayOrigin + camera.ProjectRayNormal(mouseButtonEvent.Position) * 2000;
			var intersection = spaceState.IntersectRay(new PhysicsRayQueryParameters3D()
			{
				From = rayOrigin,
				To = rayEnd
			});

			if (intersection != null)
			{
				var pos = intersection["position"].AsVector3();
				GD.Print($"Clicked on: X: {pos.X}, Y: {pos.Y}, Z: {pos.Z} ");

				MovementTarget = pos;
			}
			//MovementTarget = new Vector3(mouseButtonEvent.Position.X, 0, mouseButtonEvent.Position.Y);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
