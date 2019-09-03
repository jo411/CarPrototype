using CarProto.CustomComponents;
using GeonBit.ECS;
using Microsoft.Xna.Framework;
using GeonBit.ECS.Components.Physics;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.Core;
using GeonBit.ECS.Components.Graphics;

namespace CarProto.CustomGameObjects
{
    class Grass : GameObject
    {
        public Grass(float x, float y, GameObject p, GameScene parent)
        {
            Model grassModel = ResourcesManager.Instance.GetModel("Models/grass");
            ModelRenderer grassModelrender = new ModelRenderer(grassModel);
            AddComponent(grassModelrender);
            SceneNode.Scale = new Vector3(0.06f, 0.06f, 0.06f);
            SceneNode.Rotation = new Vector3(Util.degToRad(90f), Util.degToRad(0f), Util.degToRad(0f));

            SceneNode.Position += y * Vector3.Up;
            SceneNode.Position += x * Vector3.Right;
            SceneNode.PositionZ = 1.5f;
            Parent = parent.Root;
            AddComponent(new GrassCollision(p));
        }
    }
}
