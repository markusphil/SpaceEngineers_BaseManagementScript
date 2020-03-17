using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class PowerModule
        {
            private readonly List<IMyBatteryBlock> _batteries = new List<IMyBatteryBlock>();
            private readonly List<IMyPowerProducer> _generators = new List<IMyPowerProducer>();
            private readonly IMyTextSurface _powerTextPanel;
            private readonly IMyTextSurface _batteryTextPanel;

            public float TotalStoredPower;
            public float TotalCapacity;
            public float ChargePercentage;
            public float TotalBatteryInput;
            public float TotalBatteryOutput;
            public float TotalPowerProduction;
            public float TotalLoad;


            public PowerModule(IMyGridTerminalSystem grid, IMyProgrammableBlock me)
            {
                grid.GetBlocksOfType(_batteries, block => block.IsSameConstructAs(me));
                grid.GetBlocksOfType(_generators, block => block.IsSameConstructAs(me) && !(block is IMyBatteryBlock));
                _powerTextPanel = grid.GetBlockWithName("Power LCD") as IMyTextPanel;
                _powerTextPanel.ContentType = ContentType.TEXT_AND_IMAGE;
                _batteryTextPanel = grid.GetBlockWithName("Battery LCD") as IMyTextPanel;
                _batteryTextPanel.ContentType = ContentType.TEXT_AND_IMAGE;
            }

            public void CheckPowerProduction()
            {
                float totalOutput = 0;
                float totalMaxOutput = 0;

                _powerTextPanel.WriteText("Power Production \n ------------------------ \n", false);
                foreach (var block in _generators)
                {
                    var name = block.CustomName;
                    var currentOutput = block.CurrentOutput;
                    var maxOutput = block.MaxOutput;
                    

                    _powerTextPanel.WriteText("\n" + name + ": " + currentOutput.ToString("0.00") + " / " + maxOutput.ToString("0.00") + " MW", true);
                    
                    totalOutput += currentOutput;
                    totalMaxOutput += maxOutput;
                }

                TotalPowerProduction = totalOutput;
                TotalLoad = (totalOutput / totalMaxOutput) * 100;

                _powerTextPanel.WriteText("\n------------------------\n Total Power Production: " + totalOutput.ToString("0.00") + " / " + totalMaxOutput.ToString("0.00") + " MW", true);
                _powerTextPanel.WriteText("\n Power Load: " + TotalLoad + "%", true);

                
  
            }

            public void CheckBatteryCapacity()
            {

                float totalStoredPower = 0;
                float totalCapacity = 0;
                float totalInput = 0;
                float totalOutput = 0;

                _batteryTextPanel.WriteText("Battery Charge \n ------------------------ \n", false);
                foreach (var bat in _batteries)
                {
                    var name = bat.CustomName;
                    var currentCharge = bat.CurrentStoredPower;
                    var maxCharge = bat.MaxStoredPower;
                    var currentOutput = bat.CurrentOutput;
                    var currentInput = bat.CurrentInput;
                    var isCharging = bat.IsCharging;

                    var chargedIn = isCharging ? TimeSpan.FromHours(maxCharge - currentCharge / currentInput).ToString(@"hh\:mm") + " h" :"";
                    var depletedIn = isCharging ? "" : TimeSpan.FromHours(currentCharge / currentOutput).ToString(@"hh\:mm") + " h"; 


                    _batteryTextPanel.WriteText("\n" + name + ": " + currentCharge.ToString("0.00") + " / " + maxCharge.ToString("0.00 MW") + (isCharging ? " charged in " + chargedIn : " depleted in " + depletedIn), true);
                    totalStoredPower += currentCharge;
                    totalCapacity += maxCharge;
                    totalInput += currentInput;
                    totalOutput += currentOutput;
                }

                var deltaP = totalInput - totalOutput;
                var totalChargedIn = deltaP>0 ? TimeSpan.FromHours(totalCapacity - totalStoredPower / totalInput).ToString(@"hh\:mm") + "h" :"";
                var totalDepletedIn = deltaP > 0 ? "" : TimeSpan.FromHours(totalStoredPower / totalOutput).ToString(@"hh\:mm") + "h";


                _batteryTextPanel.WriteText("\n------------------------\n Total Capacity: " + totalStoredPower.ToString("0.00") + " / " + totalCapacity.ToString("0.00 MW"), true);
                _batteryTextPanel.WriteText( "\n Total dP: " + deltaP.ToString("0.00 MW") + "\n" + (deltaP > 0 ?"charged in " + totalChargedIn : "depleted in " + totalDepletedIn), true);

                TotalStoredPower = totalStoredPower;
                TotalCapacity = totalCapacity;
                TotalBatteryInput = totalInput;
                TotalBatteryOutput = totalOutput;
                ChargePercentage = TotalStoredPower / TotalCapacity * 100;

            }
        }
   
    }
}
