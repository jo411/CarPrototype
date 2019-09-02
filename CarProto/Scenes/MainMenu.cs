using GeonBit.Core;
using GeonBit.ECS;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarProto
{
    class MainMenu : GameScene
    {
        private GameState gameState;
        public MainMenu(GameState gameState)
        {
            this.gameState = gameState;
            init();
        }

        void init()
        {
            Panel titlePanel = new Panel(new Vector2(1920, 1200),PanelSkin.None,Anchor.TopCenter);
            Texture2D titleTex = ResourcesManager.Instance.GetTexture("Images/title");
            Image titleImage = new Image(titleTex, new Vector2(900,400),ImageDrawMode.Stretch,Anchor.TopCenter);
            titlePanel.AddChild(titleImage);

            Panel panel = new Panel(new Vector2(400, 600), PanelSkin.Default, Anchor.Center,new Vector2(0,200));

            this.UserInterface.AddEntity(titlePanel);
            this.UserInterface.AddEntity(panel);
            
            // add title and text
            panel.AddChild(new Header("   Welcome to \nDeath 'N' Derby!"));
            panel.AddChild(new HorizontalLine());
            var tutorialText = new Paragraph("Reach the end of the track without dying in a blaze of glory!\n\n" +
                                                "Use A and D to steer side to side to avoid objects.\n\n" +
                                                "Be careful not to take too much damage or you may find steering difficult...");
            panel.AddChild(tutorialText);

            // add a button at the bottom
            Button closeTut = new Button("Click to Start!", ButtonSkin.Fancy, Anchor.BottomCenter);
            closeTut.OnClick = (Entity btn) =>
            {
                //btn.Parent.Visible = false;
                gameState.changeScene(State.CAR_BUILDER);
            };
            panel.AddChild(closeTut);
        }

    }
}
