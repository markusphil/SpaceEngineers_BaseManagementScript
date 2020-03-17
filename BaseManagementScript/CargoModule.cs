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
        public class CargoModule
        {
            private readonly List<IMyCargoContainer> _containers = new List<IMyCargoContainer>();
            private readonly IMyTextSurface _cargoTextPanel;

            public float TotalMaxVolume;
            public float TotalCurrentVolume;
            public float Percentage;



            // constructor
            public CargoModule(IMyGridTerminalSystem grid, IMyProgrammableBlock me)
            {
                grid.GetBlocksOfType(_containers, block => block.IsSameConstructAs(me));
                _cargoTextPanel = grid.GetBlockWithName("Cargo LCD") as IMyTextPanel;
                _cargoTextPanel.ContentType = ContentType.TEXT_AND_IMAGE;
            }

            // methods
            public void CheckCargoCapacity()
            {

                float totalMaxVolume = 0;
                float totalCurrentVolume = 0;
       
                _cargoTextPanel.WriteText("Cargo Capacity \n ------------------------ \n", false);
                foreach (var container in _containers)
                {
                    var name = container.CustomName;
                    var maxVolume = (float)container.GetInventory().MaxVolume * 1000;
                    var currentVolume = (float)container.GetInventory().CurrentVolume * 1000;

                    _cargoTextPanel.WriteText("\n" + name + ": " + currentVolume + " / " + maxVolume + " l", true);
                    totalMaxVolume += maxVolume;
                    totalCurrentVolume += currentVolume;
                }

                _cargoTextPanel.WriteText(
                    "\n------------------------\n Total: " + totalCurrentVolume + " / " + totalMaxVolume + " l", true);

                TotalMaxVolume = totalMaxVolume;
                TotalCurrentVolume = totalCurrentVolume;
                Percentage = totalCurrentVolume / totalMaxVolume * 100;

            }
        }

        
    }
}
