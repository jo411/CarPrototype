using GeonBit;
using Microsoft.Xna.Framework;

namespace CarProto
{
    /// <summary>
    /// Your main game class!
    /// </summary>
    internal class Game1 : GeonBitGame
    {
        GameState gameState;

        /// <summary>
        /// Initialize your GeonBitGame properties here.
        /// </summary>
        public Game1()
        {
            InitParams.UiTheme = "hd";
            InitParams.DebugMode = true;
            InitParams.EnableVsync = true;
            InitParams.Title = "Death 'N' Derby";
            InitParams.FullScreen = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        override public void Update(GameTime gameTime)
        {
            switch (gameState.currentState)//Handle updates for each scene seperately. 
            {
                case State.MAIN_MENU:
                    if (ActiveScene.GetType() != typeof(MainMenu))//If the scene state has change begin the load to the correct scene
                    {
                        gameState.loadNewScene();
                    }
                    break;
                case State.CAR_BUILDER:
                    if (ActiveScene.GetType() != typeof(CarBuilder))
                    {
                        gameState.loadNewScene();
                    }
                    break;
                case State.GAME:
                    if (ActiveScene.GetType() != typeof(CarGame))
                    {
                        gameState.loadNewScene();
                    }
                    else
                    {
                        ((CarGame)gameState.currentScene).doUpdate();
                    }
                    break;
            }

            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Escape) || gameState.quitFlag)//Escape to quit from any scene
            {
                Exit();

            }
        }

        /// <summary>
        /// Initialize to implement per main type.
        /// </summary>
        override public void Initialize()
        {
            CreateGameStateMachine();
        }


        void CreateGameStateMachine()
        {
            gameState = new GameState();
        }


        /// <summary>
        /// Draw function to implement per main type.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        override public void Draw(GameTime gameTime)
        {
            /// TBD add any custom drawing functionality here.
            /// Note: since GeonBit handle its own drawing internally, usually you don't need to do anything here.
        }
    }
}
