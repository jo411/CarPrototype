using GeonBit.ECS.Components;
using GeonBit.Managers;
using GeonBit.UI.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto.CustomComponents
{
    class uiUpdate : BaseComponent
    {
        public Paragraph timeDisplay { get; set; }
        public Paragraph damageDisplay { get; set; }
        public Paragraph speedDisplay { get; set;}
        
        PlayerController pc;

        public uiUpdate(Paragraph timeDisplay, Paragraph damageDisplay, Paragraph speedDisplay)
        {                                      
            this.timeDisplay = timeDisplay;
            this.damageDisplay = damageDisplay;
            this.speedDisplay = speedDisplay;
        }
        protected override void OnAddToScene()
        {
            pc = _GameObject.ParentScene.Root.Find("player").GetComponent<PlayerController>();
        }

        public override BaseComponent Clone()
        {
            return new uiUpdate(timeDisplay, damageDisplay, speedDisplay);
        }

        /// <summary>
        /// Do on-frame based update.
        /// </summary>
        protected override void OnUpdate()
        {
            timeDisplay.Text = getFormattedTimeString();
            damageDisplay.Text = pc.damage.ToString()+"% Damaged";
            speedDisplay.Text = pc.movingSpeed.ToString("####.#") + "MPH";
          
            timeDisplay.MarkAsDirty();
            damageDisplay.MarkAsDirty();
            speedDisplay.MarkAsDirty();
        }

        private string getFormattedTimeString()
        {
            TimeSpan current = Managers.TimeManager.TotalTime;
            return current.ToString(@"mm\:ss\:ff");            
        }
    }
}
