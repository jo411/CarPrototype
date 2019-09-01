using CarProto.CustomComponents;
using GeonBit.Core;
using GeonBit.Core.Graphics.Materials;
using GeonBit.ECS;
using GeonBit.ECS.Components.Graphics;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private GameObject cameraObject;
        private GameObject carObject;

        private ProgressBar handling;
        private ProgressBar weight;
        private ProgressBar damageReduction;
        // private CarGameObjectBuilder carBuilder;
        public CarBuilder(GameState gameState)
        {            
            this.gameState = gameState;
            init();
        }

        void init()
        {
            addSelectorUI();
            addStatDisplay();
            addCarModelAndCamera();
            updateStatDisplay();
        }

        void addCamera()
        {
            if(cameraObject!=null)
            {
                cameraObject.Destroy();
            }
            cameraObject = new GameObject("camera", SceneNodeType.Simple);
            cameraObject.AddComponent(new Camera());           
            cameraObject.SceneNode.Position = carObject.SceneNode.Position + new Vector3(0, -35, 30);
            cameraObject.SceneNode.Rotation = new Vector3(Util.degToRad(60), Util.degToRad(0), Util.degToRad(0));

            carObject.SceneNode.Position += new Vector3(0, -12, 20);

            cameraObject.Parent = Root;           
        }

        void addCarModelAndCamera()
        {
            Vector3 savedRotation = new Vector3(0, 0, 0);
            if(carObject!=null)
            {
                savedRotation = carObject.SceneNode.Rotation;
                carObject.Destroy();
            }else
            {
                savedRotation = new Vector3(Util.degToRad(0f), Util.degToRad(270f), Util.degToRad(270f));
            }
            
           
            carObject = gameState.carState.getCarGameObject();
           
            carObject.AddComponent(new RotatingObject(.5f, 0, 0));

            carObject.SceneNode.Rotation = savedRotation;
            carObject.Parent = Root;

            addCamera();
        }
        void updateStatDisplay()
        {
            handling.Min = (uint)(gameState.carState.getMinTurningSpeed())-10;
            handling.Max = (uint)(gameState.carState.getMaxTurningSpeed() );
            handling.Value = (int)(gameState.carState.getCarTurnSpeed() );


            float drMin = ((gameState.carState.getMaxDamageReduction()) * 100);//reversed to convert from reduction to durability
            float drMax =((gameState.carState.getMinDamageReduction()) * 100);
            float drRange = drMax - drMin;
            float currentDR = (gameState.carState.getCarDamageReduction() * 100);
            damageReduction.Min = 0;
            damageReduction.Max =  (uint)drRange;
            damageReduction.Value = (int)(currentDR-drMin);

            weight.Min = (uint)(gameState.carState.getMinWeight() * 100)-10;
            weight.Max = (uint)(gameState.carState.getMaxWeight() * 100);
            weight.Value = (int)(gameState.carState.getCarWeight() * 100);

        }
        void addStatDisplay()
        {
            Vector2 size = new Vector2(600, 50);
            Panel panel = new Panel(new Vector2(650, 500), PanelSkin.Default, Anchor.TopLeft);
            handling= new ProgressBar(0, 10, size,Anchor.TopCenter,new Vector2(0,50));
            weight = new ProgressBar(0, 10,size, Anchor.Center);
            damageReduction = new ProgressBar(0, 10, size, Anchor.BottomCenter, new Vector2(0, 50));
            panel.AddChild(weight);
            panel.AddChild(damageReduction);
            panel.AddChild(handling);

            Paragraph handlingLabel = new Paragraph("Handling", Anchor.TopCenter, new Vector2(350, 100), new Vector2(0, 0));
            Paragraph weightLabel = new Paragraph("Weight", Anchor.Center, new Vector2(350, 100), new Vector2(0, -70));
            Paragraph damageReductionLabel = new Paragraph("Durability", Anchor.BottomCenter, new Vector2(350, 100), new Vector2(0, 100));

            handling.Locked = true;
            weight.Locked = true;
            damageReduction.Locked = true;


            panel.AddChild(weightLabel);
            panel.AddChild(damageReductionLabel);
            panel.AddChild(handlingLabel);

            this.UserInterface.AddEntity(panel);
        }     
        void addSelectorUI()
        {
            Panel panel = new Panel(new Vector2(1200, 450), PanelSkin.Default, Anchor.BottomCenter);
            this.UserInterface.AddEntity(panel);

            panel.AddChild(new Header("CAR BUILDER"));
           
          
            SelectList bodySelect = new SelectList(new Vector2(350, 150), Anchor.Center, new Vector2(0, 0));
            //bodyDrop.DefaultText = ("Choose a Body");
            foreach(string text in gameState.carState.getBodyStrings())
            {
                bodySelect.AddItem(text);
            }
            
            bodySelect.ToolTipText = ("Choose a Body");

            bodySelect.OnValueChange = (Entity e) =>
            {
                gameState.carState.updateSelectedBody(bodySelect.SelectedIndex);
                addCarModelAndCamera();
                updateStatDisplay();
            };
            

            SelectList frontWheelSelect = new SelectList(new Vector2(300, 150), Anchor.CenterLeft, new Vector2(0, 0));
            //frontWheelDrop.DefaultText = ("Choose Front Wheels");
            foreach (string text in gameState.carState.getWheelStrings())
            {
                frontWheelSelect.AddItem(text);
            }
            frontWheelSelect.ToolTipText = ("Choose Front Wheels");

            frontWheelSelect.OnValueChange = (Entity e) =>
            {
                gameState.carState.updateSelectedWheel(frontWheelSelect.SelectedIndex,true);
                addCarModelAndCamera();
                updateStatDisplay();
            };

            SelectList backWheelSelect = new SelectList(new Vector2(300, 150), Anchor.CenterRight, new Vector2(0, 0));
            //backWheelDrop.DefaultText = ("Choose Back Wheels");
            foreach (string text in gameState.carState.getWheelStrings())
            {
                backWheelSelect.AddItem(text);                   
            }
            backWheelSelect.ToolTipText = ("Choose Back Wheels");

            backWheelSelect.OnValueChange = (Entity e) =>
            {
                gameState.carState.updateSelectedWheel(backWheelSelect.SelectedIndex, false);
                addCarModelAndCamera();
                updateStatDisplay();
            };

            int labelOffsetY = -100;
            int labelOffsetX = 70;
            Color labelColor = Color.Plum;
            Paragraph bodyLabel = new Paragraph("Body", Anchor.Center, new Vector2(80, 20), new Vector2(0, labelOffsetY));
            bodyLabel.FillColor = labelColor;
            
            Paragraph fwLabel = new Paragraph("Front Wheels", Anchor.CenterLeft, new Vector2(200, 20), new Vector2(labelOffsetX, labelOffsetY));
            fwLabel.FillColor = labelColor;

            Paragraph bwLabel = new Paragraph("Back Wheels", Anchor.CenterRight, new Vector2(200, 20), new Vector2(labelOffsetX, labelOffsetY));
            bwLabel.FillColor = labelColor;

            panel.AddChild(bodyLabel);
            panel.AddChild(fwLabel);
            panel.AddChild(bwLabel);

            panel.AddChild(bodySelect);
            panel.AddChild(frontWheelSelect);
            panel.AddChild(backWheelSelect);

            Button closeTut = new Button("Click to play!", ButtonSkin.Fancy, Anchor.BottomCenter);
            closeTut.OnClick = (Entity btn) =>
            {
                btn.Parent.Visible = false;
                gameState.changeScene(State.GAME);
            };
            panel.AddChild(closeTut);
            Button randomButton = new Button("Random Car", ButtonSkin.Fancy, Anchor.TopLeft, new Vector2(350,50));
            randomButton.OnClick = (Entity btn) =>
            {               
                gameState.carState.buildRandomCar();
                bodySelect.SelectedIndex = (int)gameState.carState.body;
                frontWheelSelect.SelectedIndex = (int)gameState.carState.frontWheels;
                backWheelSelect.SelectedIndex = (int)gameState.carState.backWheels;
                addCarModelAndCamera();
                updateStatDisplay();
            };
            panel.AddChild(randomButton);

            //Panel wip = new Panel(new Vector2(700, 450),PanelSkin.Default, Anchor.CenterLeft,new Vector2(0,-100));

            //wip.AddChild(new Header("--Woah slow down there!--"));
            //panel.AddChild(new HorizontalLine());
            //var tutorialText = new Paragraph("This feature isn't quite ready yet!\n\n Here you would build your car from various parts.\n\nEach part affects how your car handles and how much damage it takes.\n\n For now go ahead and click play, we'll make you a random car.");
            //wip.AddChild(tutorialText);
            //UserInterface.AddEntity(wip);

        }
    }
}
