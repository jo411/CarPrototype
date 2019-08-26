using GeonBit.ECS;

namespace CarProto
{
    public enum State
    {
        MAIN_MENU,
        CAR_BUILDER,
        GAME,
    }

    class GameState
    {
        public State currentState { get; private set; }
        public GameScene currentScene;
        public GameState()
        {
            currentState = State.MAIN_MENU;
            currentScene = new GameScene();
        }

        public void changeScene(State newState)
        {
            currentState = newState;
        }

        public void loadNewScene()
        {
            GameScene old = currentScene;

            switch (currentState)
            {
                case State.MAIN_MENU:
                    currentScene = new MainMenu(this);
                    break;

                case State.CAR_BUILDER:
                    currentScene = new CarBuilder(this);
                    break;

                case State.GAME:
                    currentScene = new CarGame(this);
                    break;
            }

            currentScene.Load();
            old.Destroy();
        }
    }
}
