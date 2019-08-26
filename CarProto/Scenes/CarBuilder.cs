using GeonBit.ECS;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto
{
    class CarBuilder : GameScene
    {
        private GameState gameState;
        public CarBuilder(GameState gameState)
        {
            this.gameState = gameState;
            init();
        }

        void init()
        {
            Panel panel = new Panel(new Vector2(600, 400), PanelSkin.Default, Anchor.Center);
            this.UserInterface.AddEntity(panel);

            // add title and text
            panel.AddChild(new Header("CAR BUILDER"));
            panel.AddChild(new HorizontalLine());
            var tutorialText = new Paragraph("Combine different car parts together to create your racer.\n\n These stats will determine how your car handles and takes damage. \n\n Editor Coming soon for now your car is randomized!", Anchor.Center);
            panel.AddChild(tutorialText);

            // add a button at the bottom

            Button closeTut = new Button("Click to play!", ButtonSkin.Fancy, Anchor.BottomCenter);
            closeTut.OnClick = (Entity btn) =>
            {
                btn.Parent.Visible = false;
                gameState.changeScene(State.GAME);
            };
            panel.AddChild(closeTut);
        }
    }
}
