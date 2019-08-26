using CarProto.CustomComponents;
using GeonBit;
using GeonBit.ECS;
using GeonBit.ECS.Components;
using GeonBit.ECS.Components.Physics;
using GeonBit.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CarProto.CustomGameObjects
{
    class Obstacle : GameObject
    {
        public Obstacle(float x, float y, GameObject p, GameScene parent)
        {
            Parent = parent.Root;
            AddComponent(new ObstacleCollision(p));
            SceneNode.Position += y * Vector3.Up;
            SceneNode.Position += x * Vector3.Left;
        }
    }
}
