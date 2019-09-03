using GeonBit.ECS;
using GeonBit.ECS.Components;

namespace CarProto.CustomComponents
{
    class GrassCollision : BaseComponent
    {
        GameObject player;

        public GrassCollision(GameObject p)
        {
            player = p;
        }

        public override BaseComponent Clone()
        {
            return new GrassCollision(player);
        }

        protected override void OnUpdate()
        {
            if (player.SceneNode.PositionX - _GameObject.SceneNode.PositionX < 3 &&
                player.SceneNode.PositionX - _GameObject.SceneNode.PositionX > -3 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY < 3 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY > -3)
            {
                player.GetComponent<PlayerController>().ReduceSpeed();
                this.Destroy();
            }
        }
    }
}
