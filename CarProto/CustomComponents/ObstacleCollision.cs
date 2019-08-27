using GeonBit.ECS;
using GeonBit.ECS.Components;

namespace CarProto.CustomComponents
{
    class ObstacleCollision : BaseComponent
    {
        GameObject player;
        int damage;

        public ObstacleCollision(GameObject p, int damage)
        {
            player = p;
            this.damage = damage;
        }

        public override BaseComponent Clone()
        {
            return new ObstacleCollision(this.player, this.damage);
        }

        protected override void OnUpdate()
        {
            if (player.SceneNode.PositionX - _GameObject.SceneNode.PositionX < 4 &&
                player.SceneNode.PositionX - _GameObject.SceneNode.PositionX > -4 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY < 5 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY > -5)
            {
                player.GetComponent<PlayerController>().addDamage(damage);
                _GameObject.Destroy();
            }
        }
    }
}
