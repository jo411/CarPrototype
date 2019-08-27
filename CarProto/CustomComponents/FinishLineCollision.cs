using GeonBit.ECS;
using GeonBit.ECS.Components;
using System;

namespace CarProto.CustomComponents
{
    class FinishLineCollision : BaseComponent
    {
        GameObject player;
        GameObject gameManager;

        public FinishLineCollision(GameObject p, GameObject gm)
        {
            player = p;
            gameManager = gm;
        }

        public override BaseComponent Clone()
        {
            return new FinishLineCollision(player, gameManager);
        }

        protected override void OnUpdate()
        {
            if (player.SceneNode.PositionY > _GameObject.SceneNode.PositionY)
            {
                gameManager.GetComponent<GameManager>().setWin(true);
                player.GetComponent<PlayerController>().dead = true;
            }
        }
    }
}

