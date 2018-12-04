using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus {
    internal class ScheduledAction<T> {

        public T Value { get; private set; }
        public int ExecutionDay { get; private set; }

        public ScheduledAction() : this(-1) { }

        public ScheduledAction(T value) : this(value, SDate.Now().DaysSinceStart) { }

        public ScheduledAction(int date) : this(default(T), date) { }

        public ScheduledAction(T value, int date) {
            Value = value;
            ExecutionDay = date;
        }

        public bool ValueMatch(T compare) => Value.Equals(compare);

        public bool IsValid => ExecutionDay >= 0 && !Value.Equals(default(T));

        public bool IsToday => IsValid && Math.Abs(Difference) == 0;

        public bool HasExpired => IsValid && Difference < 0;

        public int Difference => ExecutionDay - SDate.Now().DaysSinceStart;

        public static ScheduledAction<T> Default => new ScheduledAction<T>();
    }
}
