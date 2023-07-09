using Godot;
using System;
using System.Collections.Generic;
using GMTKGameJam2023.Scripts;
using GMTKGameJam2023.Scripts.Enums;
using GMTKGameJam2023.Units;

public partial class CameraControl : Camera3D
{
    [Export] private float cameraSpeed = 100.0f;

    [Export] private float zoomSpeed = 1000f;
    [Export] private float maxZoom = 2.0f;
    [Export] private float minZoom = 0.5f;

    private Vector2 _previousMousePosition;
    private List<Units> _selection = new List<Units>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var cameraTranslation = new Vector3();
        if (Input.IsActionPressed(Controls.Up.ToString()))
        {
            cameraTranslation.Z -= cameraSpeed * (float)delta;
        }

        if (Input.IsActionPressed(Controls.Down.ToString()))
        {
            cameraTranslation.Z += cameraSpeed * (float)delta;
        }

        if (Input.IsActionPressed(Controls.Left.ToString()))
        {
            cameraTranslation.X -= cameraSpeed * (float)delta;
        }

        if (Input.IsActionPressed(Controls.Right.ToString()))
        {
            cameraTranslation.X += cameraSpeed * (float)delta;
        }

        //ZoomCamera(zoomAmount * zoomSpeed * delta);
        Vector2 mousePosition = GetViewport().GetMousePosition();
        if (Input.IsMouseButtonPressed(MouseButton.Middle))
        {
            Vector2 deltaMouse = mousePosition - _previousMousePosition;
            cameraTranslation.X = -deltaMouse.X * 0.1f;
            cameraTranslation.Z = -deltaMouse.Y * 0.1f;
            // RotateY(Mathf.DegToRad();
            // RotateX(Mathf.DegToRad();
        }

        _previousMousePosition = mousePosition;
        GD.Print($"Camera Position: {Position}, Translating to {cameraTranslation}");
        Position += cameraTranslation;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
            {
                var cameraTranslation = new Vector3();
                cameraTranslation.Z += zoomSpeed * (float)GetProcessDeltaTime();
                Translate(cameraTranslation);
            }

            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
            {
                var cameraTranslation = new Vector3();
                cameraTranslation.Z -= zoomSpeed * (float)GetProcessDeltaTime();
                Translate(cameraTranslation);
            }

            base._Input(@event);
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed)
            {
                var unit = GetUnitUnderMouse(mouseButtonEvent.Position);
                
                if (_selection.Count > 0 && unit == null)
                {
                    var intersection = this.CastRay(GetWorld3D().DirectSpaceState, mouseButtonEvent.Position);

                    if (intersection != null && intersection.Count > 0)
                    {
                        var pos = intersection["position"].AsVector3();
                        GD.Print($"Clicked on: X: {pos.X}, Y: {pos.Y}, Z: {pos.Z} ");

                        MoveUnits(pos);
                    }
                    //MovementTarget = new Vector3(mouseButtonEvent.Position.X, 0, mouseButtonEvent.Position.Y);
                }
                else
                {
                    if (unit == null)
                    {
                        return;
                    }
                    _selection.Clear();
                    _selection.Add(unit);
                }

            }
        }
    }

    public void MoveUnits(Vector3 targetPos)
    {
        foreach (var select in _selection)
        {
            select.MovementTarget = targetPos;
        }
    }

    public Units GetUnitUnderMouse(Vector2 mousePosition)
    {
        var res = this.CastRay(GetWorld3D().DirectSpaceState, mousePosition);
        if (res.Count == 0)
        {
            return null;
        }

        if (res["collider"].Obj is not CharacterBody3D) return null;
        var unit = res["collider"].As<Units>();
        return unit.IsInGroup("Horde") ? unit : null;

    }
}