using GeonBit.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto.CustomComponents
{
    class gameManager : BaseComponent
    {
        public PlayerController pc { get; set; }
        public bool gameIsRunning = true;
        public bool quitFlag { get; set; } = false;

        protected override void OnAddToScene()
        {
            pc = _GameObject.ParentScene.Root.Find("player").GetComponent<PlayerController>();
        }

        public bool isGameOver()
        {
           
            if(pc==null)
            {
               return false;
            }
            else
            {
                if(pc.dead)
                {
                    gameIsRunning = false;
                    return true;
                }
                return false;
            }
        }

        public override BaseComponent Clone()
        {
            return new gameManager();
        }
    }
}
