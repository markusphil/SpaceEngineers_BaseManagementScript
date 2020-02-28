using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using System.Runtime.CompilerServices;
using EmptyKeys.UserInterface.Generated.AtmBlockView_Bindings;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        private List<IMyCargoContainer> _containers = new List<IMyCargoContainer>();
        private readonly IMyTextSurface _cargoTextPanel;
        

        public Program()
        {
            //Set Update frequency to every 100th game tick
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            // get Blocks in constructor for better performance => reload after added container
            GridTerminalSystem.GetBlocksOfType(_containers, block => block.IsSameConstructAs(Me));
            _cargoTextPanel = GridTerminalSystem.GetBlockWithName("Cargo LCD") as IMyTextPanel;
            _cargoTextPanel.ContentType = ContentType.TEXT_AND_IMAGE;

        }

        public void Save()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            ShowCargoCapacity();
        }

        private void ShowCargoCapacity()
        {

        float totalMaxVolume = 0;
        float totalCurrentVolume = 0;

        _cargoTextPanel.WriteText("Cargo Capacity \n ------------------------ \n", false);
            foreach (var container in _containers)
            {
                var name = container.CustomName;
                var maxVolume = (float)container.GetInventory().MaxVolume*1000;
                var currentVolume = (float)container.GetInventory().CurrentVolume*1000;

                _cargoTextPanel.WriteText("\n" + name + ": " + currentVolume + " / " + maxVolume + " l", true);
                totalMaxVolume += maxVolume;
                totalCurrentVolume += currentVolume;
            }
           
            _cargoTextPanel.WriteText(
                "\n------------------------\n Total: " + totalCurrentVolume + " / " + totalMaxVolume + " l", true);
        }


    }
}