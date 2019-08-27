using CarProto.CustomComponents;
using GeonBit.ECS;
using Microsoft.Xna.Framework;

namespace CarProto.CustomGameObjects
{
    class Obstacle : GameObject
    {
        public Obstacle(float x, float y, GameObject p, GameScene parent, int damage)
        {
            Parent = parent.Root;
            AddComponent(new ObstacleCollision(p,damage));
            SceneNode.Position += y * Vector3.Up;
            SceneNode.Position += x * Vector3.Left;
            SceneNode.PositionZ = 1.5f;
        }
    }
}
