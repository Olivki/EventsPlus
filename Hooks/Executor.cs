using EventsPlus.Handlers;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus {
    internal static class Executor {

        private static String lastCommand = "";

        private static Handler<Utils.Weather> weatherHandler = new WeatherHandler();
        private static Handler<int> timeHandler = new TimeHandler();

        internal static void ExecuteCommands(GameLocation loc, GameTime time, Event e, string[] args) {
            var mod = ModEntry.instance;
            PrintCurrentCommand();

            if (args[0].Equals("weather")) {
                weatherHandler.Execute(loc, time, e, args);

                e.CurrentCommand++;
            } else if (args[0].Equals("resetWeather")) {
                if (!weatherHandler.IsReset)
                    weatherHandler.TransformRealValueBack();

                e.CurrentCommand++;
            } else if (args[0].Equals("time")) {
                timeHandler.Execute(loc, time, e, args);

                e.CurrentCommand++;
            } else if (args[0].Equals("resetTime")) {
                if (!timeHandler.IsReset)
                    timeHandler.TransformRealValueBack();

                e.CurrentCommand++;
            } else if (args[0].Equals("season")) {
                Game1.currentSeason = args[1];

                e.CurrentCommand++;
            }

            ExecuteExperimentalCommands(loc, e, args, mod);
            ExecuteScheduleCommands(loc, e, args, mod);
        }

        private static void ExecuteExperimentalCommands(GameLocation loc, Event e, string[] args, ModEntry mod) {
            if (args[0].Equals("@conditionFork")) {
                // I'm a little tea pot.

                e.CurrentCommand++;
            } else if (args[0].Equals("@debug")) {
                var debugString = string.Join(" ", args).Replace("@debug ", "");
                Game1.game1.parseDebugInput(debugString);

                e.CurrentCommand++;
            }
        }

        private static void ExecuteScheduleCommands(GameLocation loc, Event e, string[] args, ModEntry mod) {
            if (args[0].Equals("#weather")) {
                var weather = Utils.GetWeather(args[1]);
                var type = args[2];

                if (type.Equals("in")) {
                    var day = SDate.Now().DaysSinceStart + int.Parse(args[3]);
                    mod.schedule.Weather_Actions.Add(new ScheduledAction<Utils.Weather>(weather, day));

                    e.CurrentCommand++;
                } else if (type.Equals("on")) {
                    var day = int.Parse(args[3]);
                    mod.schedule.Weather_Actions.Add(new ScheduledAction<Utils.Weather>(weather, day));

                    e.CurrentCommand++;
                } else if (type.Equals("tomorrow")) {
                    var day = SDate.Now().DaysSinceStart + 1;
                    mod.schedule.Weather_Actions.Add(new ScheduledAction<Utils.Weather>(weather, day));

                    e.CurrentCommand++;
                }
            } else if (args[1].Equals("#morningEvent")) {
                var @event = args[1];
                var type = args[2];

                if (type.Equals("in")) {
                    var day = SDate.Now().DaysSinceStart + int.Parse(args[3]);
                    mod.schedule.Morning_Event_Actions.Add(new ScheduledAction<string>(@event, day));

                    e.CurrentCommand++;
                } else if (type.Equals("on")) {
                    var day = int.Parse(args[3]);
                    mod.schedule.Morning_Event_Actions.Add(new ScheduledAction<string>(@event, day));

                    e.CurrentCommand++;
                } else if (type.Equals("tomorrow")) {
                    var day = SDate.Now().DaysSinceStart + 1;
                    mod.schedule.Morning_Event_Actions.Add(new ScheduledAction<string>(@event, day));

                    e.CurrentCommand++;
                }
            }
        }

        internal static void CurrentEventChanged(Event e) {
            if (e != null)
                return;

            ClearHandlers();
        }

        private static void ClearHandlers() {
            if (weatherHandler.IsTemporary && !weatherHandler.IsReset)
                weatherHandler.TransformRealValueBack();

            if (timeHandler.IsTemporary && !timeHandler.IsReset)
                timeHandler.TransformRealValueBack();
        }

        public static void PrintCurrentCommand() {
            if (Game1.CurrentEvent == null)
                return;

            string current = Utils.CurrentEventCommand;

            if (lastCommand.Equals(current))
                return;

            lastCommand = current;

            var e = Game1.CurrentEvent;

            System.Diagnostics.Trace.WriteLine($"Event[{e.id}@{Game1.currentLocation.Name}]:" +
                                               $" Command[{Utils.CurrentEventCommandId}] = \"{current}\".");
        }
    }
}
