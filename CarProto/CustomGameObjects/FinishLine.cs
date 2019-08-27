using CarProto.CustomComponents;
using GeonBit.ECS;
using Microsoft.Xna.Framework;

namespace CarProto.CustomGameObjects
{
    class FinishLine : GameObject
    {
        public FinishLine(float y, GameObject p, GameScene parent, GameObject gm)
        {
            Parent = parent.Root;
            AddComponent(new FinishLineCollision(p, gm));
            SceneNode.Position += y * Vector3.Up;
            SceneNode.PositionZ = 1.5f;
        }
    }
}
