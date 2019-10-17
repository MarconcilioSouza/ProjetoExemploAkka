using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.Infrastructure
{
    public static class Extensions
    {
        public static int TryToInt(this object valor)
        {
            var v = valor?.ToString() ?? "";
            int.TryParse(v, out int i);
            return i;
        }
    }
}
