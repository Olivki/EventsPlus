using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus.Handlers {
    abstract class Handler<T> {

        public T Value { get; protected set; }

        public T OldValue { get; protected set; }

        public bool IsTemporary { get; protected set; } = true;

        public abstract void Execute(GameLocation loc, GameTime time, Event e, string[] args);

        public abstract void TransformRealValueBack();

        public void ResetValue() => Value = default(T);

        public bool IsReset => Value == null || Value.Equals(default(T));

        protected void Debug() {
            System.Diagnostics.Trace.WriteLine($"Old: {OldValue}, New: {Value}");
        }
    }
}
