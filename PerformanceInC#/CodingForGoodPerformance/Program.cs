using System;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace CodingForGoodPerformance
{
    public class Md5VsSha256 {
        private const int N = 10000;
        private readonly byte[] data;

        private readonly SHA256 sha256 = SHA256.Create();
        private readonly MD5 mD5 = MD5.Create();

        public Md5VsSha256() {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        public byte[] Sha256() => sha256.ComputeHash(data);
        
        [Benchmark]
        public byte[] Md5() => mD5.ComputeHash(data);
    }
    class Program
    {
        static void Main(string[] args)
        {
            /// Build benchmark with optimization enabled.
            /// https://benchmarkdotnet.org/articles/guides/troubleshooting.html#debugging-benchmarks
            var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
            // var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            // Console.WriteLine(summary);
        }
    }
}
