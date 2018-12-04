using EventsPlus.Handlers;
using Harmony;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EventsPlus {
    public class ModEntry : Mod {

        private string lastCommand = "";
        private string storagePath;

        internal Schedule schedule;

        internal static ModEntry instance;

        private Event lastEvent;

        public ModEntry() {
            instance = this;
        }

        public override void Entry(IModHelper helper) {
            var harmony = HarmonyInstance.Create("se.proxus.eventsplus");

            harmony.PatchAll(Assembly.GetExecutingAssembly());

            SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            GameEvents.UpdateTick += GameEvents_UpdateTick;
            TimeEvents.AfterDayStarted += TimeEvents_AfterDayStarted;
        }

        private void TimeEvents_AfterDayStarted(object sender, EventArgs e) {
            schedule.PerformWeatherActions();
            schedule.PerformMorningEventActions();
        }

        private void SaveEvents_BeforeSave(object sender, EventArgs e) {
            schedule.PerformWeatherActions();
            schedule.ClearOldActions();
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e) {
            ReadSchedule();
        }

        private void GameEvents_UpdateTick(object sender, EventArgs args) {
            CheckForCurrentEventUpdate();

            if (!Monitor.IsVerbose || Game1.CurrentEvent == null)
                return;

            string current = Utils.CurrentEventCommand;

            if (lastCommand.Equals(current))
                return;

            lastCommand = current;

            var e = Game1.CurrentEvent;

            Monitor.Log($"Event[{e.id}@{Game1.currentLocation.Name}]:" +
                        $" Command[{Utils.CurrentEventCommandId}] = \"{current}\".", LogLevel.Debug);
        }

        private void CheckForCurrentEventUpdate() {
            var currentEvent = Game1.CurrentEvent;

            if (lastEvent == currentEvent)
                return;

            lastEvent = currentEvent;

            Executor.CurrentEventChanged(lastEvent);
        }

        private void ReadSchedule() {
            storagePath = $"schedules/{Constants.SaveFolderName}.epsch";

            if (!File.Exists(Path.Combine(Helper.DirectoryPath, storagePath))) {
                Helper.Data.WriteJsonFile<Schedule>(storagePath, schedule = new Schedule());
            }

            schedule = Helper.Data.ReadJsonFile<Schedule>(storagePath);
        }

        internal void SaveSchedule() {
            Helper.Data.WriteJsonFile<Schedule>(storagePath, schedule);
        }
    }
} 
