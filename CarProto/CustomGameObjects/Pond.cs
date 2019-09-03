using CarProto.CustomComponents;
using GeonBit.ECS;
using Microsoft.Xna.Framework;
using GeonBit.ECS.Components.Physics;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.Core;
using GeonBit.ECS.Components.Graphics;

namespace CarProto.CustomGameObjects
{
    class Pond : GameObject
    {
        GameObject pos1;
        GameObject pos2;

        public Pond(float x, float y, GameObject p, GameScene parent)
        {
            Model pondModel = ResourcesManager.Instance.GetModel("Models/Pool_01");
            ModelRenderer pondModelrender = new ModelRenderer(pondModel);
            AddComponent(pondModelrender);
            SceneNode.Scale = new Vector3(0.04f, 0.02f, 0.04f);
            SceneNode.Rotation = new Vector3(Util.degToRad(93f), Util.degToRad(0f), Util.degToRad(0f));

            SceneNode.Position += y * Vector3.Up;
            SceneNode.Position += x * Vector3.Right;
            SceneNode.PositionZ = 0.2f;

            Parent = parent.Root;

            Vector3 from = new Vector3(x, y - 14, 0);
            Vector3 to = new Vector3(x, y + 4, 0);

            AddComponent(new PondCollision(p, from, to));

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

            //pos1.AddComponent(new StaticBody(new BoxInfo(new Vector3(10, 10, 10))));
            //pos2.AddComponent(new StaticBody(new BoxInfo(new Vector3(10, 10, 10))));
        }
    }
}
