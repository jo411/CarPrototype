using GeonBit.ECS.Components;

namespace CarProto.CustomComponents
{
    class GameManager : BaseComponent
    {
        public PlayerController pc { get; set; }
        public bool gameIsRunning = true;
       

        public bool winFlag { get; private set; } = false;
        protected override void OnAddToScene()
        {
            pc = _GameObject.ParentScene.Root.Find("player").GetComponent<PlayerController>();
        }

        public bool isGameOver()
        {
            if (pc == null)
            {
                return false;
            }
            else
            {
                if (pc.dead || winFlag)
                {
                    gameIsRunning = false;
                    return true;
                }
                return false;
            }
        }

        public void setWin(bool playerWon)
        {
            winFlag = playerWon;
        }

        public override BaseComponent Clone()
        {
            return new GameManager();
        }
    }
}
