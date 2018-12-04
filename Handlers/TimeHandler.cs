using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus.Handlers {
    class TimeHandler : Handler<int> {

        public override void Execute(GameLocation loc, GameTime time, Event e, string[] args) {
            IsTemporary = bool.Parse(args[2]);
            OldValue = Game1.timeOfDay;
            Utils.SetTime(Value = Convert.ToInt32(args[1]));

            Debug();
        }

        public override void TransformRealValueBack() {
            Utils.SetTime(OldValue);
            ResetValue();
        }
    }
}
