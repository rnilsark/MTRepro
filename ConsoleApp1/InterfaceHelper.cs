using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    static class InterfaceHelper
    {
        public static int CountInterfaces(Type t) => GetInterfaces(t).Count();
        
        private static Holder GetInterfaces(Type type)
        {
            var h = new Holder(type);
            foreach (var t in type.GetInterfaces()) h.Add(GetInterfaces(t));
            return h;
        }

        private class Holder
        {
            private Type T { get; }
            private List<Holder> Ts { get; } = new List<Holder>();

            public Holder(Type t) => T = t;

            public void Add(Holder t) => Ts.Add(t);

            public int Count() => 1 + Ts.Sum(holder => holder.Count());

            public override string ToString()
            {
                return ToString("");
            }

            private string ToString(string prefix)
            {
                if (!Ts.Any()) return "";

                var s = new StringBuilder();
                s.AppendLine($"{prefix}===");
                foreach (var holder in Ts)
                {
                    s.AppendLine($"{prefix}{holder.T.Name}");
                }
                
                s.AppendLine($"{prefix}>>>");
                foreach (var holder in Ts)
                {
                    s.AppendLine(holder.ToString(prefix + " "));
                }

                s.AppendLine($"{prefix}<<<");

                return s.ToString();
            }
        }
    }
}
