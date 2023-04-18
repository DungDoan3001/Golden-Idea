using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Api.Helpers
{
    class RandomDateTime
    {
        public DateTime start;
        public Random gen;
        public int range;

        public RandomDateTime()
        {
            start = new DateTime(2023, 1, 1);
            gen = new Random();
            range = (DateTime.Today - start).Days;
        }

        public DateTime Next()
        {
            return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
        }
    }
}
