

using Newtonsoft.Json;
using System.IO;

namespace NodeGraphFrame.Runtime
{
    public static class DeserializationUtil
    {
        public static GraphData DeserializeGraph(string assetPath)
        {
            var contents = File.ReadAllText(assetPath);
            var graphData = JsonConvert.DeserializeObject<GraphData>(contents);
            return graphData;
        }
    }
}

