using GeonBit.ECS;
using GeonBit.ECS.Components;

namespace CarProto.CustomComponents
{
    class ObstacleCollision : BaseComponent
    {
        GameObject player;

        public ObstacleCollision(GameObject p)
        {
            player = p;
        }

        public override BaseComponent Clone()
        {
            return new ObstacleCollision(this.player);
        }

        protected override void OnUpdate()
        {
            if (player.SceneNode.PositionX - _GameObject.SceneNode.PositionX < 7.5 &&
                player.SceneNode.PositionX - _GameObject.SceneNode.PositionX > -7.5 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY < 10 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY > -10)
            {
                player.GetComponent<PlayerController>().addDamage(10);
                _GameObject.Destroy();
            }
        }
    }
}
