using GeonBit.ECS.Components;
using GeonBit.UI.Entities;
using System;

namespace CarProto.CustomComponents
{
    class UiUpdate : BaseComponent
    {
        public Paragraph timeDisplay;
        public Paragraph damageDisplay;
        public Paragraph speedDisplay;
        public Panel gameOver;


        PlayerController pc;

        public UiUpdate(Paragraph timeDisplay, Paragraph damageDisplay, Paragraph speedDisplay, Panel gameOver)
        {
            this.timeDisplay = timeDisplay;
            this.damageDisplay = damageDisplay;
            this.speedDisplay = speedDisplay;
            this.gameOver = gameOver;
        }
        protected override void OnAddToScene()
        {
            pc = _GameObject.ParentScene.Root.Find("player").GetComponent<PlayerController>();
        }

        public override BaseComponent Clone()
        {
            return new UiUpdate(timeDisplay, damageDisplay, speedDisplay, gameOver);
        }

        /// <summary>
        /// Do on-frame based update.
        /// </summary>
        protected override void OnUpdate()
        {
            updateSpeedDisplay();
            updateTimeDisplay();
            updateDamageDisplay();

        }

        private void updateTimeDisplay()
        {
            TimeSpan current = Managers.TimeManager.TotalTime;
            timeDisplay.Text = current.ToString(@"mm\:ss\:ff");

            timeDisplay.MarkAsDirty();
        }

        private void updateSpeedDisplay()
        {
            speedDisplay.Text = pc.movingSpeed.ToString("####.#") + " MPH";
            speedDisplay.MarkAsDirty();
        }

        private void updateDamageDisplay()
        {
            float damage = pc.damage;
            damageDisplay.Text = damage.ToString() + "% Damaged";


            if (damage < 50)
            {
                damageDisplay.FillColor = Microsoft.Xna.Framework.Color.Green;
            }
            else if (damage < 75)
            {
                damageDisplay.FillColor = Microsoft.Xna.Framework.Color.Yellow;
            }
            else
            {
                damageDisplay.FillColor = Microsoft.Xna.Framework.Color.Red;
            }

            damageDisplay.MarkAsDirty();
        }

        public void DisplayGameOver()
        {
            gameOver.Find<Paragraph>("gameover").Text += "\n Final Time: " + Managers.TimeManager.TotalTime.ToString(@"mm\:ss\:ff");
            gameOver.Visible = true;
        }

    }
}
