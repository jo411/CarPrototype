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
        PlayerController pc;
        public bool gameIsRunning = false;


        protected override void OnAddToScene()
        {
            pc = _GameObject.ParentScene.Root.Find("player").GetComponent<PlayerController>();
        }

        public bool isGameOver()
        {
            return pc==null?false:pc.dead;
        }

        public override BaseComponent Clone()
        {
            return new gameManager();
        }
    }
}
