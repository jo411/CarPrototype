using CarProto.CustomComponents;
using GeonBit.ECS;
using Microsoft.Xna.Framework;
using GeonBit.ECS.Components.Physics;

namespace CarProto.CustomGameObjects
{
    class Boundary : GameObject
    {
        GameObject pos1;
        GameObject pos2;

        public Boundary(Vector3 from, Vector3 to, GameObject p, GameScene parent, int damage = 10)
        {
            Parent = parent.Root;
            AddComponent(new BoundaryCollision(p, from, to, damage));

            pos1 = new GameObject();
            pos2 = new GameObject();

            pos1.Parent = parent.Root;
            pos2.Parent = parent.Root;
            
            pos1.SceneNode.Position += from.Y * Vector3.Up;
            pos1.SceneNode.Position -= from.X * Vector3.Left;
            
            pos2.SceneNode.Position += to.Y * Vector3.Up;
            pos2.SceneNode.Position -= to.X * Vector3.Left;
            
            pos1.SceneNode.PositionZ = 1.5f;
            pos2.SceneNode.PositionZ = 1.5f;
            
            //pos1.AddComponent(new StaticBody(new BoxInfo(new Vector3(5, 5, 5))));
            //pos2.AddComponent(new StaticBody(new BoxInfo(new Vector3(5, 5, 5))));
        }
    }
}
