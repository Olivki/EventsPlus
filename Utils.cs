using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus {
    static partial class Utils {

        public static string[] Split(string s, string deli) => s.Split(deli.ToCharArray(), 
                                                                       StringSplitOptions.RemoveEmptyEntries);

        // Stardew Valley related methods. V

        // Event methods
        public static bool TryGetEventAtCurrentLocation(string id, out Event e) {
            return TryGetEvent(Game1.currentLocation, id, out e);
        }

        public static bool TryGetEvent(GameLocation loc, string id, out Event e) {
            var events = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + loc.Name);

            foreach (KeyValuePair<string, string> pair in events) {
                var condition = pair.Key;
                var args = condition.Split('/');

                if (args.Length <= 0)
                    continue;

                var foundId = args[0];

                if (foundId.Equals(id)) {
                    e = new Event(pair.Value);
                    return true;
                }
            }

            e = null;
            return false;
        }

        // Misc

        public static NPC TodaysBirthdayNPC => Utility.getTodaysBirthdayNPC(SDate.Now().Season, SDate.Now().Day);

        public static bool IsBirthdayHappeningToday => TodaysBirthdayNPC!= null;

        public static bool IsNpcsBirthdayToday(string name) => IsBirthdayHappeningToday 
            ? TodaysBirthdayNPC == Game1.getCharacterFromName(name, true) : false;

        public static void SetDayOfMonth(int day) {
            Game1.stats.DaysPlayed = (uint)(Utility.getSeasonNumber(Game1.currentSeason) * 28 +day * Game1.year);
            Game1.dayOfMonth = day;
        }

        public static void SetTime(int time) {
            Game1.timeOfDay = time;

            // This is here to actual update the light engine, otherwise, if you switch from say, 2100 to 600 the light
            // will still be dark and not update.
            Game1.outdoorLight = Color.White;
        }

        public static int CurrentEventCommandId {
            get {
                var e = Game1.CurrentEvent;
                return Math.Min(e.eventCommands.Length - 1, e.CurrentCommand);
            }
        }

        public static string CurrentEventCommand {
            get {
                var e = Game1.CurrentEvent;
                return e.eventCommands[CurrentEventCommandId];
            }
        }

        // Stardew Valley date related methods.
        public static DayOfWeek GetDay(string name) => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), name, true);

        public static bool SameDay(string day) => SameDay(GetDay(day));

        public static bool SameDay(DayOfWeek day) => SDate.Now().DayOfWeek.Equals(day);

        // Seasons
        public static Season GetSeason(string name) => (Season)Enum.Parse(typeof(Season), name, true);

        public static Season CurrentSeason => GetSeason(SDate.Now().Season);

        public static bool SameSeason(string name) => SameSeason(GetSeason(name));

        public static bool SameSeason(Season season) => CurrentSeason.Equals(season);

        // Weather related methods.
        public static Weather GetWeather(string weather) {
            return (Weather) Enum.Parse(typeof(Weather), weather);
        }

        public static bool IsKnownWeather(string weather) {
            return weather != null && Enum.TryParse<Weather>(weather, out Weather w);
        }

        public static int ToWeatherIndex(string weatherName) => (int)Enum.Parse(typeof(Weather), weatherName);

        public static string WeatherForTomorrowName => Enum.GetName(typeof(Weather), ((Weather)Game1.weatherForTomorrow));

        public static Weather WeatherForTomorrow => (Weather)Game1.weatherForTomorrow;

        public static string CurrentWeatherName => Enum.GetName(typeof(Weather), (int)CurrentWeather);

        public static Weather CurrentWeather {
            get {
                if (!Game1.isLightning
                    && !Game1.isSnowing
                    && !Game1.isRaining
                    && !Game1.isDebrisWeather) {
                    return Weather.sunny;
                } else if (!Game1.isLightning
                           && !Game1.isSnowing
                           && Game1.isRaining
                           && !Game1.isDebrisWeather
                           && !Game1.isFestival()
                           && !Game1.weddingToday) {
                    return Weather.rainy;
                } else if (Game1.isLightning
                           && !Game1.isSnowing
                           && Game1.isRaining
                           && !Game1.isDebrisWeather
                           && !Game1.isFestival()
                           && !Game1.weddingToday) {
                    return Weather.stormy;
                } else if (!Game1.isLightning
                           && !Game1.isSnowing
                           && !Game1.isRaining
                           && Game1.isDebrisWeather
                           && !Game1.isFestival()
                           && !Game1.weddingToday) {
                    return Weather.windy;
                } else if (!Game1.isLightning
                           && Game1.isSnowing
                           && !Game1.isRaining
                           && !Game1.isDebrisWeather
                           && !Game1.isFestival()
                           && !Game1.weddingToday) {
                    return Weather.snowy;
                } else if (!Game1.isLightning
                           && !Game1.isSnowing
                           && !Game1.isRaining
                           && !Game1.isDebrisWeather
                           && Game1.isFestival()
                           && !Game1.weddingToday) {
                    return Weather.festival;
                } else if (!Game1.isLightning
                           && !Game1.isSnowing
                           && !Game1.isRaining
                           && !Game1.isDebrisWeather
                           && !Game1.isFestival()
                           && Game1.weddingToday) {
                    return Weather.wedding;
                }

                throw new NotImplementedException("The current weather flags represent an unknown weather type.");
            }
        }

        public static void SetWeather(string weather) => SetWeather((Weather)GetWeather(weather));

        public static void SetWeather(Weather weather) {
            switch (weather) {
                case Weather.sunny:
                    Game1.isLightning = false;
                    Game1.isSnowing = false;
                    Game1.isRaining = false;
                    Game1.isDebrisWeather = false;
                    Game1.debrisWeather.Clear();
                    break;

                case Weather.rainy:
                    Game1.isLightning = false;
                    Game1.isSnowing = false;
                    Game1.isRaining = true;
                    Game1.isDebrisWeather = false;
                    Game1.debrisWeather.Clear();
                    break;

                case Weather.stormy:
                    Game1.isLightning = true;
                    Game1.isSnowing = false;
                    Game1.isRaining = true;
                    Game1.isDebrisWeather = false;
                    Game1.debrisWeather.Clear();
                    break;

                case Weather.windy:
                    Game1.isLightning = false;
                    Game1.isSnowing = false;
                    Game1.isRaining = false;
                    Game1.isDebrisWeather = true;
                    Game1.populateDebrisWeatherArray();
                    break;

                case Weather.snowy:
                    Game1.isLightning = false;
                    Game1.isSnowing = true;
                    Game1.isRaining = false;
                    Game1.isDebrisWeather = false;
                    Game1.debrisWeather.Clear();
                    break;

                case Weather.festival:
                    Game1.isLightning = false;
                    Game1.isSnowing = false;
                    Game1.isRaining = false;
                    Game1.isDebrisWeather = false;
                    Game1.debrisWeather.Clear();
                    break;

                case Weather.wedding:
                    Game1.isLightning = false;
                    Game1.isSnowing = false;
                    Game1.isRaining = false;
                    Game1.isDebrisWeather = false;
                    Game1.debrisWeather.Clear();
                    break;

                default:
                    throw new NotImplementedException($"{weather} is not a known weather type.");
            }
        }

        public static bool WeatherMatch(string name) => WeatherMatch(GetWeather(name));

        public static bool WeatherMatch(Weather weather) => CurrentWeather.Equals(weather);
    }
}
