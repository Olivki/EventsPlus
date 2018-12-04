using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus {
    internal class Schedule {

        public List<ScheduledAction<Utils.Weather>> Weather_Actions { get; set; } = new List<ScheduledAction<Utils.Weather>>();

        public List<ScheduledAction<string>> Morning_Event_Actions { get; set; } = new List<ScheduledAction<string>>();

        internal void PerformWeatherActions() {
            var mod = ModEntry.instance;
            foreach (ScheduledAction<Utils.Weather> action in Weather_Actions) {
                if (action.IsToday && !action.ValueMatch(Utils.CurrentWeather)) {
                    mod.Monitor.Log($"Forcefully changing the weather from {Utils.CurrentWeather} to " +
                                    $"{action.Value}.", LogLevel.Debug);
                    Utils.SetWeather(action.Value);
                }
            }
        }

        internal void PerformMorningEventActions() {
            var mod = ModEntry.instance;
            foreach (ScheduledAction<string> action in Morning_Event_Actions) {
                if (action.IsToday) {
                    mod.Monitor.Log($"Activating the scheduled morning event. (ID:{action.Value}.)", LogLevel.Debug);

                    var validEventId = Utils.TryGetEventAtCurrentLocation(action.Value, out Event e);

                    if (validEventId) {
                        Game1.currentLocation.currentEvent = e;
                    } else {
                        mod.Monitor.Log($"There is no event with the id of {action.Value}@{Game1.currentLocation.Name}.",
                                        LogLevel.Error);
                    }
                }
            }
        }

        internal void ClearOldActions() {
            var mod = ModEntry.instance;
            Weather_Actions.RemoveAll(action => action.HasExpired);
            Morning_Event_Actions.RemoveAll(action => action.HasExpired);
            mod.SaveSchedule();
        }

    }
}
