using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus {
    internal static class Condtioner {

        public static int Conditions(string[] conditions, GameLocation loc, int ogResult) {
            for (int i = 1; i < conditions.Length; i++) {
                if (conditions[i][0] != '@')
                    continue;

                var args = conditions[i].Substring(1).Split(' ');

                if (args[0].Equals("isFestival")) {
                    if (!Game1.isFestival())
                        return -1;

                    continue;
                } else if (args[0].Equals("isWedding")) {
                    if (!Game1.weddingToday)
                        return -1;

                    continue;
                } else if (args[0].Equals("isBirthday")) {
                    if (!Utils.IsBirthdayHappeningToday)
                        return -1;

                    continue;
                } else if (args[0].Equals("birthday")) {
                    if (!Utils.IsNpcsBirthdayToday(args[1]))
                        return -1;

                    continue;
                } else if (args[0].Equals("married")) {
                    if (!Game1.player.spouse.Equals(args[1]))
                        return -1;

                    continue;
                } else if (args[0].Equals("day")) {
                    if (args[1].IndexOf(", ") > 0) {
                        var days = Utils.Split(args[1], ", ");

                        for (int j = 0; j < days.Length; j++) {
                            var day = days[j];
                            var isNumber = int.TryParse(day, out int index);

                            if (isNumber) {
                                if (!Utils.SameDay((DayOfWeek) index))
                                    return -1;
                            } else {
                                if (!Utils.SameDay(day))
                                    return -1;
                            }
                        }
                    } else {
                        var isNumber = int.TryParse(args[1], out int index);

                        if (isNumber) {
                            if (!Utils.SameDay((DayOfWeek) index))
                                return -1;
                        } else {
                            if (!Utils.SameDay(args[1]))
                                return -1;
                        }
                    }

                    continue;
                } else if (args[0].Equals("season")) {
                    if (args[1].IndexOf(", ") > 0) {
                        var seasons = Utils.Split(args[1], ", ");

                        for (int j = 0; j < seasons.Length; j++) {
                            var season = seasons[j];
                            var isNumber = int.TryParse(season, out int index);

                            if (isNumber) {
                                if (!Utils.SameSeason((Utils.Season)index))
                                    return -1;
                            } else {
                                if (!Utils.SameSeason(season))
                                    return -1;
                            }
                        }
                    } else {
                        var isNumber = int.TryParse(args[1], out int index);

                        if (isNumber) {
                            if (!Utils.SameSeason((Utils.Season)index))
                                return -1;
                        } else {
                            if (!Utils.SameSeason(args[1]))
                                return -1;
                        }
                    }

                    continue;
                } else if (args[0].Equals("weather")) {
                    if (args[1].IndexOf(", ") > 0) {
                        var weathers = Utils.Split(args[1], ", ");

                        for (int j = 0; j < weathers.Length; j++) {
                            var weather = weathers[j];
                            var isNumber = int.TryParse(weather, out int index);

                            if (isNumber) {
                                if (!Utils.WeatherMatch((Utils.Weather)index))
                                    return -1;
                            } else {
                                if (!Utils.WeatherMatch(weather))
                                    return -1;
                            }
                        }
                    } else {
                        var isNumber = int.TryParse(args[1], out int index);

                        if (isNumber) {
                            if (!Utils.WeatherMatch((Utils.Weather)index))
                                return -1;
                        } else {
                            if (!Utils.WeatherMatch(args[1]))
                                return -1;
                        }
                    }

                    continue;
                }
            }

            return ogResult;
        }
    }
}
