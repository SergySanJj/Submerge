using Assets.Scripts.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Parameters
{
    [Serializable]
    public class DayNumber : Parameter
    {
        private Dictionary<int, bool> neededDates = new Dictionary<int, bool>
            {
                { 10, false },
                { 15, false },
                { 20, false },
                { 25, false },
                { 30, false },
                { 35, false },
                { 40, false },
                { 45, false },
                { 50, false },
                { 75, false },
                { 90, false },
                { 100, true },
                { 125, false },
                { 150, false },
                { 175, false },
                { 200, false },
                { 225, false },
                { 250, false },
                { 275, false },
                { 300, false },
                { 325, false },
                { 350, false },
                { 365, false },
        };

        public DayNumber(int initialValue) : base(initialValue)
        {
            lowerBoundary = 0;
            upperBoundary = 1000000;
        }

        public override string FormText()
        {
            string res = value.ToString();
            return res;
        }

        public override bool NoResources()
        {
            return false; // Day always correct
        }

        public override void SetValue(int val)
        {
            value = val;
            ClampValue();
            ObserveDayCheckpoints();
        }

        private void ObserveDayCheckpoints()
        {
            
            int currentDay = this.value;
            foreach (var item in neededDates.Where(kvp => currentDay >= kvp.Key).ToList())
            {
                Checkpoint dayN = (Checkpoint)ScriptableObject.CreateInstance(typeof(Checkpoint));
                dayN.SetParams("Day" + item.Key, item.Value, "You've survived for " + item.Key.ToString() + " days!", null);

                GameEvents.current.AddPassedCheckpoint(dayN);

                neededDates.Remove(item.Key);
            }
        }
    }
}
