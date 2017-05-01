using System;
using Newtonsoft.Json;

namespace AwsKinesisFirehoseHelloWorld
{
    /// <summary>
    /// Models a data point that we're sending through the firehose
    /// </summary>
    public class FooDataPoint
    {
        [JsonProperty(PropertyName = "bar")]
        public string Bar { get; set; }

        [JsonProperty(PropertyName = "someint")]
        public int SomeInt { get; set; }

        [JsonProperty(PropertyName = "whenwasit")]
        public DateTime WhenWasIt { get; set; }
    }
}
