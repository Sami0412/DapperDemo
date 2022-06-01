using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Runner
{
    public static class Extensions
    {
        public static void Output(this object item)
        {
            var serialiser = new SerializerBuilder().Build();
            var yaml = serialiser.Serialize(item);
            Console.WriteLine(yaml);
        }
    }
}
