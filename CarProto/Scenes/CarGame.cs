using CarProto.CustomComponents;
using CarProto.CustomGameObjects;
using GeonBit.Core;
using GeonBit.ECS;
using GeonBit.ECS.Components.Graphics;
using GeonBit.ECS.Components.Physics;
using GeonBit.ECS.Components.Sound;
using GeonBit.Managers;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CarProto
{
    class CarGame : GameScene
    {
        private GameState gameState;

        GameObject carObject;
        GameObject cameraObject;
        GameObject trackObject;
        GameObject gameManager;
        GameObject finishLine;
        GameManager gm;

        UiUpdate UIUpdater;

        bool showTutorial = false;
        bool showDebug = false;
        //bool quited = false;

        public CarGame(GameState gameState)
        {
            this.gameState = gameState;
            init();
        }

        public void doUpdate()
        {          
                checkQuit();
        }

        void checkQuit()
        {
            if (!gm.gameIsRunning) { return; }

            if (gm.isGameOver())
            {
                UIUpdater.DisplayGameOver(gm.winFlag);
               
            }
        }

        void init()
        {
            addGameManager();
            addPlayer();
            addObstacles();
            addTrack();
            addCamera();
            addFinishLine();
            //addTutorialGui();
            addSound();
            addGameGui();
            addDebug();
        }

        void addGameManager()
        {
            gameManager = new GameObject("gameManager");
            gm = new GameManager();
            gameManager.AddComponent(gm);
        }

        void addDebug()
        {
            if (showDebug)
            {
                var diagnosticData = new GeonBit.UI.Entities.Paragraph("", GeonBit.UI.Entities.Anchor.BottomLeft, offset: Vector2.One * 10f, scale: 0.7f);
                diagnosticData.BeforeDraw = (GeonBit.UI.Entities.Entity entity) =>
                {
                    diagnosticData.Text = Diagnostic.Instance.GetReportString();
                };
                UserInterface.AddEntity(diagnosticData);

                Diagnostic.Instance.DebugRenderPhysics = true;
            }
        }

        void addCamera()
        {
            cameraObject = new GameObject("camera", SceneNodeType.Simple);
            cameraObject.AddComponent(new Camera());
            cameraObject.AddComponent(new CameraFollow());
            cameraObject.SceneNode.Rotation = new Vector3(Util.degToRad(60), Util.degToRad(0), Util.degToRad(0));

            CameraFollow cf = cameraObject.GetComponent<CameraFollow>();
            cf.offset = new Vector3(0, -35, 30);
            cf.dampingStrength = .07f;

            cameraObject.Parent = Root;
        }

        void addTrack()
        {
            trackObject = new GameObject("track");
            Model trackModel = ResourcesManager.Instance.GetModel("Models/Track");
            trackObject.AddComponent(new ModelRenderer(trackModel));
            trackObject.SceneNode.Rotation = new Vector3(Util.degToRad(0f), Util.degToRad(270f), Util.degToRad(270f));
            trackObject.SceneNode.Scale = new Vector3(.50f, .25f, .25f);
            trackObject.Parent = Root;
            trackObject.SceneNode.Position = new Vector3(25f, -60f, 0f);

            GameObject backgroundObject = new GameObject("background");
            Texture2D backgroundimage = ResourcesManager.Instance.GetTexture("Images/SpyHunter");
            SceneBackground background = new SceneBackground(backgroundimage)
            {
                DrawMode = BackgroundDrawMode.Tiled
            };
            backgroundObject.AddComponent(background);
            backgroundObject.Parent = Root;
        }
        void addObstacles()
        {
            //Don't look below. Dear lord I and a level editor. 
            // Add obstacle
            int baseDamage=30;

            List<Obstacle> obstacles = new List<Obstacle>();
            obstacles.Add(new Obstacle(10, 100, carObject, this,baseDamage));            
            obstacles.Add(new Obstacle(-5, 150, carObject, this,baseDamage));
            obstacles.Add(new Obstacle(10, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(8, 250, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(6, 230, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 350, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 350, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 400, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-15, 95, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 140, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(14, 195, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(16, 235, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-12, 240, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-35, 344, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 356, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 452, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(10, 25, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 25, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(0, 45, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-25, 60, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-15, 60, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-5, 60, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(5, 60, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 60, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 60, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(20, 80, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 80, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 80, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 80, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 80, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-30, 80, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(25, 100, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 100, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 100, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-15, 100, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-20, 100, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 100, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-30, 180, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 180, carObject, this, baseDamage));
            
            obstacles.Add(new Obstacle(10, 180, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 180, carObject, this, baseDamage));


            obstacles.Add(new Obstacle(-30, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(5, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 200, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-15, 305, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 340, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(14, 395, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(16, 335, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-12, 340, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-35, 315, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 312, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 306, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-25, 400, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-15, 400, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 400, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-5, 400, carObject, this, baseDamage));
            
            obstacles.Add(new Obstacle(20, 400, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(25, 400, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-30, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-20, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-15, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 450, carObject, this, baseDamage));
           
            obstacles.Add(new Obstacle(10, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(25, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 450, carObject, this, baseDamage));


            //add borders
            int farRight = 35;
            int farLeft = -35;
            int top = 0;
            int numRocks = 30;
            int width = 20;
            int damage = 75;
            for(int i=0;i<numRocks;i++)
            {
                Obstacle ob1 = new Obstacle(farRight, top + i * width, carObject, this, damage);
                Obstacle ob2 = new Obstacle(farLeft, top + i * width, carObject, this, damage);

                obstacles.Add(ob1);
                obstacles.Add(ob2);

                if (i % 4 == 0)
                {
                    ob1.AddComponent(new ObstacleMove());
                    ob2.AddComponent(new ObstacleMove());
                }
            }
            

            // Add body just for visual diagnostics
            Model rockModel = ResourcesManager.Instance.GetModel("Models/Rock");
            ModelRenderer rockModelrender = new ModelRenderer(rockModel);

            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.AddComponent(new ModelRenderer(rockModel));
                obstacle.SceneNode.Scale = new Vector3(4, 4, 4);
                obstacle.AddComponent(new StaticBody(new BoxInfo(new Vector3(1, 1, 1))));
            }
        }
        void addPlayer()
        {
            carObject = new GameObject("player");
            Model carModel = ResourcesManager.Instance.GetModel("Models/MuscleCar");
            carObject.AddComponent(new ModelRenderer(carModel));
            PlayerController pc = new PlayerController();

            gm.pc = pc;

            carObject.AddComponent(pc);
            carObject.SceneNode.Rotation = new Vector3(Util.degToRad(0f), Util.degToRad(270f), Util.degToRad(270f));
            carObject.Parent = Root;

            // Add body just for visual diagnostics
            KinematicBody playerBody = new KinematicBody(new BoxInfo(new Vector3(8, 8, 5)));
            playerBody.InvokeCollisionEvents = true;
            carObject.AddComponent(playerBody);
        }
        void addFinishLine()
        {
            finishLine = new FinishLine(550, carObject, this, gameManager);

            // Add body just for visual diagnostics
            finishLine.AddComponent(new KinematicBody(new BoxInfo(new Vector3(200, 8, 5))));
        }

        void addGameGui()
        {
            Panel statPanel = new Panel(new Vector2(600, 100), PanelSkin.Golden, Anchor.TopCenter);

            Paragraph timeDisplay = new Paragraph("", Anchor.Center);
            Paragraph speedDisplay = new Paragraph("", Anchor.CenterLeft);
            Paragraph damageDisplay = new Paragraph("", Anchor.CenterRight);

            statPanel.AddChild(timeDisplay);
            statPanel.AddChild(speedDisplay);
            statPanel.AddChild(damageDisplay);

            Panel gameOverPanel = new Panel(new Vector2(500, 500), PanelSkin.Fancy, Anchor.Center);
            Paragraph gameOverText = new Paragraph("");
            gameOverText.Identifier = "gameover";
            gameOverPanel.AddChild(gameOverText);
            gameOverPanel.Visible = false;

            Button menuButton = new Button("Return to Menu", ButtonSkin.Fancy, Anchor.BottomCenter);
            menuButton.OnClick = (Entity btn) =>
            {
                gameState.changeScene(State.MAIN_MENU);
            };

            Button quitButton = new Button("Quit", ButtonSkin.Fancy, Anchor.Center);
            quitButton.OnClick = (Entity btn) =>
            {
                gameState.quitFlag = true;
            };

            gameOverPanel.AddChild(quitButton);
            gameOverPanel.AddChild(menuButton);

            GameObject uiManager = new GameObject("ui");
            UIUpdater = new UiUpdate(timeDisplay, damageDisplay, speedDisplay, gameOverPanel);
            uiManager.AddComponent(UIUpdater);
            uiManager.Parent = Root;

            UserInterface.AddEntity(statPanel);
            UserInterface.AddEntity(gameOverPanel);
        }

        void addSound()
        {
            GameObject backMusic = new GameObject("background_music");
            BackgroundMusic backgroundMusic = new BackgroundMusic("Audio/BGM");
            backgroundMusic.Volume = 100;
            backgroundMusic.Play();
            backgroundMusic.IsRepeating = true;

            backMusic.AddComponent(backgroundMusic);
            backMusic.Parent = Root;
            backMusic.GetComponent<BackgroundMusic>().Play();
        }

        void addTutorialGui()
        {
            if (!showTutorial)
            {
                return;
            }

            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 600), PanelSkin.Default, Anchor.CenterLeft);
            UserInterface.AddEntity(panel);

            // add title and text
            panel.AddChild(new Header("   Welcome to \nDeath 'N' Derby!"));
            panel.AddChild(new HorizontalLine());
            var tutorialText = new Paragraph("Reach the end of the track without dying in a blaze of glory!\n\n" +
                                                "Use A and D to steer side to side to avoid objects.\n\n" +
                                                "Be careful not to take too much damage or you may find steering difficult...");
            panel.AddChild(tutorialText);

            // add a button at the bottom
            Button closeTut = new Button("Click to hide!", ButtonSkin.Fancy, Anchor.BottomCenter);
            closeTut.OnClick = (Entity btn) =>
            {
                btn.Parent.Visible = false;
            };
            panel.AddChild(closeTut);
        }

        void addGrid()
        {
            GameObject gridObject;
            gridObject = new GameObject("shape");
            Model planeModel = ResourcesManager.Instance.GetModel("GeonBit.Core/BasicMeshes/Plane");
            gridObject.AddComponent(new ModelRenderer(planeModel));
            Texture2D gridTex = ResourcesManager.Instance.GetTexture("Images/grid");
            gridObject.GetComponent<ModelRenderer>().GetMaterials()[0].Texture = gridTex;
            gridObject.GetComponent<ModelRenderer>().GetMaterials()[0].TextureEnabled = true;
            gridObject.SceneNode.Rotation = new Vector3(Util.degToRad(0), Util.degToRad(0), Util.degToRad(0));
            gridObject.SceneNode.Scale = new Vector3(100, 100, 1);
            gridObject.Parent = Root;
        }
    }
}
