using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus.Handlers {
    class WeatherHandler : Handler<Utils.Weather> {

        public override void Execute(GameLocation loc, GameTime time, Event e, string[] args) {
            IsTemporary = bool.Parse(args[2]);
            OldValue = Utils.CurrentWeather;
            Utils.SetWeather(Value = Utils.GetWeather(args[1]));

            Debug();
        }

        public override void TransformRealValueBack() {
            Utils.SetWeather(OldValue);
            ResetValue();
        }
    }
}
