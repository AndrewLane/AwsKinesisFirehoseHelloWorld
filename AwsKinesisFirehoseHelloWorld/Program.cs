using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using Amazon;
using Amazon.KinesisFirehose;
using Amazon.KinesisFirehose.Model;
using Newtonsoft.Json;

namespace AwsKinesisFirehoseHelloWorld
{
    class Program
    {
        static void Main()
        {
            var deliveryStreamName = ConfigurationManager.AppSettings["AWS.Kineses.DeliveryStreamName"];
            var millisecondsToSleepBetweenPuts = Convert.ToInt32(ConfigurationManager.AppSettings["MillisecondsToSleepBetweenPuts"]);
            var awsRegion = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWS.Region"]);

            //spit out the delivery stream names available
            var firehoseClient = new AmazonKinesisFirehoseClient(awsRegion);
            var streams = firehoseClient.ListDeliveryStreams();
            Console.WriteLine("Here are your delivery stream names:");
            foreach (var stream in streams.DeliveryStreamNames)
            {
                Console.WriteLine($"{stream}");
            }

            Console.WriteLine("Starting PutRecord infinite loop:");
            while (true)
            {
                //throw un some dummy data
                var data = new FooDataPoint
                {
                    Bar = "wut",
                    SomeInt = (int)(DateTime.UtcNow.Ticks % 100),
                    WhenWasIt = DateTime.UtcNow
                };

                using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data))))
                {
                    var response = firehoseClient.PutRecord(new PutRecordRequest { DeliveryStreamName = deliveryStreamName, Record = new Record { Data = ms } });
                    Console.WriteLine($"{response.HttpStatusCode} {DateTime.Now:s}");
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(millisecondsToSleepBetweenPuts));
            }
        }
    }
}
