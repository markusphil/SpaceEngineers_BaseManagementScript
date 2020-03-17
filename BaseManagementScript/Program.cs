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

        private readonly CargoModule _cargo;
        private readonly PowerModule _power;
        

        public Program()
        {
            //Set Update frequency to every 100th game tick
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
                        
            _cargo = new CargoModule(GridTerminalSystem,Me);
            _power = new PowerModule(GridTerminalSystem, Me);
        }

        public void Save()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            _cargo.CheckCargoCapacity();
            _power.CheckBatteryCapacity();
            _power.CheckPowerProduction();
            Echo("Cargo: " +_cargo.Percentage + "%");
            Echo("battery Charge: " + _power.ChargePercentage + "%");
            Echo("power production: " + _power.TotalPowerProduction + "MW");
        }

    }
}