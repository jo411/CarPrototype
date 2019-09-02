using CarProto.CustomComponents;
using CarProto.CustomGameObjects;
using GeonBit.Core;
using GeonBit.Core.Graphics.Materials;
using GeonBit.ECS;
using GeonBit.ECS.Components.Graphics;
using GeonBit.ECS.Components.Misc;
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

        CarObject carObject;
        GameObject cameraObject;
        GameObject trackObject;
        GameObject gameManager;
        GameObject finishLine;
        public GameManager gm { get; private set; }

        UiUpdate uiUpdater;

        bool showTutorial = false;
        bool showDebug = true;
        private bool flyCam=false;

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
            if (gm.isGameOver() && !uiUpdater.gameOverPanel.Visible)
            {
                uiUpdater.DisplayGameOver(gm.winFlag);
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

            addBoundary();
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
           
            cameraObject.SceneNode.Rotation = new Vector3(Util.degToRad(60), Util.degToRad(0), Util.degToRad(0));

           

            if(flyCam)
            {
                cameraObject.AddComponent(new CameraEditorController());
            }
            else
            {
                cameraObject.AddComponent(new CameraFollow());
                CameraFollow cf = cameraObject.GetComponent<CameraFollow>();
                cf.offset = new Vector3(0, -35, 30);
                cf.dampingStrength = .07f;
            }

            
           
            cameraObject.Parent = Root;
        }

        void addTrack()
        {
            trackObject = new GameObject("track");
            Model trackModel = ResourcesManager.Instance.GetModel("Models/track_03");
            trackObject.AddComponent(new ModelRenderer(trackModel));
            trackObject.SceneNode.Rotation = new Vector3(Util.degToRad(90f), Util.degToRad(0f), Util.degToRad(0f));
            trackObject.SceneNode.Scale = new Vector3(.05f, .05f, .05f);
            trackObject.Parent = Root;

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
            //Don't look below. Dear lord I need a level editor. 
            // Add obstacle
            int baseDamage = 30;

            List<Obstacle> obstacles = new List<Obstacle>();
            obstacles.Add(new Obstacle(10, 100, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-5, 150, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(8, 250, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(6, 230, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 350, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 350, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 400, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-15, 95, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(0, 140, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(14, 195, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(16, 235, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-12, 240, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-35, 344, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 356, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 452, carObject, this, baseDamage));

            //obstacles.Add(new Obstacle(10, 25, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-10, 25, carObject, this, baseDamage));

            //obstacles.Add(new Obstacle(0, 45, carObject, this, baseDamage));

            //obstacles.Add(new Obstacle(-25, 60, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-15, 60, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-5, 60, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(5, 60, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(10, 60, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 60, carObject, this, baseDamage));

            //obstacles.Add(new Obstacle(20, 80, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(15, 80, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(10, 80, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 80, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-25, 80, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-30, 80, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(25, 100, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(20, 100, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 100, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-15, 100, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-20, 100, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-25, 100, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-30, 180, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 180, carObject, this, baseDamage));

            //obstacles.Add(new Obstacle(10, 180, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 180, carObject, this, baseDamage));


            //obstacles.Add(new Obstacle(-30, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 200, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-10, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(5, 200, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 200, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(20, 200, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-15, 305, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 340, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(14, 395, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(16, 335, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-12, 340, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-35, 315, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 312, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 306, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-25, 400, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-15, 400, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-10, 400, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-5, 400, carObject, this, baseDamage));

            //obstacles.Add(new Obstacle(20, 400, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(25, 400, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-30, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-25, 450, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-20, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-15, 450, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-10, 450, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(10, 450, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(15, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 450, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(25, 450, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(20, 450, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-15, 500, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-40, 510, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-13, 520, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-23, 530, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-40, 540, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 540, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-50, 550, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-75, 550, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-26, 550, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(10, 580, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-5, 580, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 580, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-18, 590, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 600, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(-25, 610, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-50, 620, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-25, 630, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-30, 630, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-40, 640, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 650, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-50, 660, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-75, 660, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-30, 670, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(10, 680, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-5, 690, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 700, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-18, 710, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 710, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(0, 680, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(5, 690, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(3, 700, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-3, 710, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(3, 710, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-5, 660, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(2, 670, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(0, 690, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(24, 700, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(10, 710, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(13, 680, carObject, this, baseDamage));

            obstacles.Add(new Obstacle(15, 710, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-20, 720, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(-5, 730, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 730, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(-5, 740, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(0, 750, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(20, 760, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(5, 760, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(13, 770, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(15, 780, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(17, 790, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(25, 800, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(6, 810, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(15, 810, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(30, 780, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(30, 790, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(35, 800, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(30, 810, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(47, 810, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(26, 810, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(55, 810, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(70, 780, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(60, 790, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(55, 800, carObject, this, baseDamage));
            //obstacles.Add(new Obstacle(50, 810, carObject, this, baseDamage));
            obstacles.Add(new Obstacle(57, 810, carObject, this, baseDamage));

            // Add body just for visual diagnostics
            Model rockModel = ResourcesManager.Instance.GetModel("Models/obstacle");
            ModelRenderer rockModelrender = new ModelRenderer(rockModel);

            Obstacle movingob1 = new Obstacle(7, 150, carObject, this, baseDamage);
            movingob1.AddComponent(new ObstacleMove(8, 20));
            obstacles.Add(movingob1);
            Obstacle movingob7 = new Obstacle(-7, 130, carObject, this, baseDamage);
            movingob7.AddComponent(new ObstacleMove(8, 20));
            obstacles.Add(movingob7);
            Obstacle movingob2 = new Obstacle(-10, 200, carObject, this, baseDamage);
            movingob2.AddComponent(new ObstacleMove(12, 24));
            obstacles.Add(movingob2);
            Obstacle movingob3 = new Obstacle(16, 340, carObject, this, baseDamage);
            movingob3.AddComponent(new ObstacleMove(12, 24));
            obstacles.Add(movingob3);
            Obstacle movingob4 = new Obstacle(10, 430, carObject, this, baseDamage);
            movingob4.AddComponent(new ObstacleMove(12, 24));
            obstacles.Add(movingob4);
            Obstacle movingob5 = new Obstacle(-3, 450, carObject, this, baseDamage);
            movingob5.AddComponent(new ObstacleMove(12, 24));
            obstacles.Add(movingob5);
            Obstacle movingob6 = new Obstacle(-30, 550, carObject, this, baseDamage);
            movingob6.AddComponent(new ObstacleMove(12, 24));
            obstacles.Add(movingob6);

            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.AddComponent(new ModelRenderer(rockModel));
                obstacle.SceneNode.Scale = new Vector3(4, 4, 4);
                //obstacle.AddComponent(new StaticBody(new BoxInfo(new Vector3(1, 1, 1))));
            }
        }

        void addPlayer()
        {
            carObject = gameState.carState.getCarGameObject();

            foreach (GameObject wheelObject in carObject.getWheelObjects())
            {
                wheelObject.AddComponent(new RotatingObject(0, 0, 0f));
            }

            PlayerController pc = new PlayerController(gameState.carState);

            gm.pc = pc;
            carObject.AddComponent(pc);
            carObject.SceneNode.Rotation = new Vector3(Util.degToRad(0f), Util.degToRad(270f), Util.degToRad(270f));
            carObject.Parent = Root;

            // Add body just for visual diagnostics
            //KinematicBody playerBody = new KinematicBody(new BoxInfo(new Vector3(8, 8, 5)))
            //{
            //    InvokeCollisionEvents = true
            //};
            //carObject.AddComponent(playerBody);
        }

        void addFinishLine()
        {
            finishLine = new FinishLine(900, carObject, this, gameManager);
            Model finishLineModel = ResourcesManager.Instance.GetModel("Models/finalLine");
            GameObject FinishLineGO = new GameObject();
            FinishLineGO.AddComponent(new ModelRenderer(finishLineModel));
            FinishLineGO.SceneNode.Scale = new Vector3(.05f, .03f, .05f);
            FinishLineGO.SceneNode.Rotation = new Vector3(Util.degToRad(270f), Util.degToRad(90f), Util.degToRad(90f));
            FinishLineGO.SceneNode.Position = new Vector3(-10, 10, 0);
            FinishLineGO.Parent = finishLine;
            finishLine.Parent = Root;

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

            Panel endImagePanel = new Panel(new Vector2(1920, 1200), PanelSkin.None, Anchor.TopCenter);
            Texture2D winTex = ResourcesManager.Instance.GetTexture("Images/victory");
            Image winImage = new Image(winTex, new Vector2(400 , 200), ImageDrawMode.Stretch, Anchor.TopCenter, new Vector2(0,70));
            winImage.Visible = false;
            winImage.Identifier = "win";

            Texture2D loseTex = ResourcesManager.Instance.GetTexture("Images/lose");
            Image loseImage = new Image(loseTex, new Vector2(400, 200), ImageDrawMode.Stretch, Anchor.TopCenter, new Vector2(0, 70));
            loseImage.Visible = false;
            loseImage.Identifier = "lose";

            endImagePanel.AddChild(winImage);
            endImagePanel.AddChild(loseImage);


            Button retryButton = new Button("Retry", ButtonSkin.Fancy, Anchor.Center)
            {
                OnClick = (Entity btn) =>
                {
                    gameState.changeScene(State.GAME);
                }
            };

            // 80 down from the Center
            Vector2 menuLocationOffset = new Vector2(0f, 95f);
            Button menuButton = new Button("Return to Menu", ButtonSkin.Fancy, Anchor.Center, null, menuLocationOffset)
            {
                OnClick = (Entity btn) =>
                {
                    gameState.changeScene(State.MAIN_MENU);
                }
            };

            Button quitButton = new Button("Quit", ButtonSkin.Fancy, Anchor.BottomCenter)
            {
                OnClick = (Entity btn) =>
                {
                    gameState.quitFlag = true;
                }
            };

            gameOverPanel.AddChild(retryButton);
            gameOverPanel.AddChild(menuButton);
            gameOverPanel.AddChild(quitButton);

            GameObject uiManager = new GameObject("ui");
            uiUpdater = new UiUpdate(timeDisplay, damageDisplay, speedDisplay, gameOverPanel, endImagePanel);
            uiManager.AddComponent(uiUpdater);
            uiManager.Parent = Root;

            UserInterface.AddEntity(endImagePanel);
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

        void addBoundary()
        {
            GameObject bound1 = new Boundary(new Vector3(-30, -100, 0), new Vector3(-30, 420, 0), carObject, this);
            GameObject bound2 = new Boundary(new Vector3(27, -100, 0), new Vector3(27, 420, 0), carObject, this);
            GameObject bound3 = new Boundary(new Vector3(-30, 419, 0), new Vector3(-18, 467, 0), carObject, this);
            GameObject bound4 = new Boundary(new Vector3(27, 419, 0), new Vector3(37, 467, 0), carObject, this);
            GameObject bound5 = new Boundary(new Vector3(-18, 467, 0), new Vector3(10, 527, 0), carObject, this);
            GameObject bound6 = new Boundary(new Vector3(37, 467, 0), new Vector3(65, 527, 0), carObject, this);
            GameObject bound7 = new Boundary(new Vector3(10, 527, 0), new Vector3(7, 580, 0), carObject, this);
            GameObject bound8 = new Boundary(new Vector3(65, 527, 0), new Vector3(63, 580, 0), carObject, this);
            GameObject bound9 = new Boundary(new Vector3(7, 580, 0), new Vector3(-16, 630, 0), carObject, this);
            GameObject bound10 = new Boundary(new Vector3(63, 580, 0), new Vector3(40, 630, 0), carObject, this);
            GameObject bound11 = new Boundary(new Vector3(-16, 630, 0), new Vector3(-29, 680, 0), carObject, this);
            GameObject bound12 = new Boundary(new Vector3(40, 630, 0), new Vector3(25, 680, 0), carObject, this);
            GameObject bound13 = new Boundary(new Vector3(-29, 680, 0), new Vector3(-42, 730, 0), carObject, this);
            GameObject bound14 = new Boundary(new Vector3(25, 680, 0), new Vector3(12, 730, 0), carObject, this);
            GameObject bound15 = new Boundary(new Vector3(-42, 730, 0), new Vector3(-70, 780, 0), carObject, this);
            GameObject bound16 = new Boundary(new Vector3(12, 730, 0), new Vector3(-16, 780, 0), carObject, this);
            GameObject bound17 = new Boundary(new Vector3(-70, 780, 0), new Vector3(-58, 840, 0), carObject, this);
            GameObject bound18 = new Boundary(new Vector3(-16, 780, 0), new Vector3(-8, 840, 0), carObject, this);
            GameObject bound19 = new Boundary(new Vector3(-58, 840, 0), new Vector3(-33, 890, 0), carObject, this);
            GameObject bound20 = new Boundary(new Vector3(-8, 840, 0), new Vector3(17, 890, 0), carObject, this);
        }
    }
}
