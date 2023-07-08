using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace GMTKGameJam2023
{
    public static class CameraExtensions
    {
        public static Dictionary CastRay(this Camera3D camera, PhysicsDirectSpaceState3D spaceState, Vector2 mousePosition )
        {
            var rayOrigin = camera.ProjectRayOrigin(mousePosition);
            var rayEnd = rayOrigin + camera.ProjectRayNormal(mousePosition) * 2000;
            var intersection = spaceState.IntersectRay(new PhysicsRayQueryParameters3D()
            {
                From = rayOrigin,
                To = rayEnd
            });

            return intersection;
            

            
        }
    }
}