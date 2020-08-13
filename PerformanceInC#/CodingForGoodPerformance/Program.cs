using System;
using System.Text;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Runtime.InteropServices;

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
    
    public class SecondBenchmark {
        private byte[] data;

        [GlobalSetup]
        public void Init() {
            data = new byte[15];

            data[0] = (byte)'H';
            data[1] = (byte)'\0';
            data[2] = (byte)'E';
            data[3] = (byte)'\0';
            data[4] = (byte)'L';
            data[5] = (byte)'\0';
            data[6] = (byte)'L';
            data[7] = (byte)'\0';
            data[8] = (byte)'O';
            data[9] = (byte)'\0';
            data[10] = (byte)'\0';
            data[11] = (byte)'\0';
            data[12] = (byte)'\0';
            data[13] = (byte)'\0';
            data[14] = (byte)'\0';
        }

        [Benchmark]
        public string Current() {
            string ret = System.Text.Encoding.Unicode.GetString(data);
            if (ret.IndexOf('\0') > -1)
                ret = ret.Substring(0, ret.IndexOf('\0'));

            return ret;
        }
    }
    
    public class ThirdBenchmark {
        private byte[] data;

        [Params(20, 200, 1000, 20000)]
        public decimal N;

        [Params(95)]
        public decimal fill;

        [GlobalSetup]
        public void Setup() {
            decimal lengthActual = N * (fill / 100);

            byte[] unicode = Encoding.Unicode.GetBytes(new string('F', (int)lengthActual/2));
            byte[] received = new byte[(int)N];
            Buffer.BlockCopy(unicode, 0, received, 0, unicode.Length);

            data = received;
        }

        [Benchmark]
        public string Benchmark_Current() {
            string ret = Encoding.Unicode.GetString(data);
            if (ret.IndexOf('\0') > -1)
                ret = ret.Substring(0, ret.IndexOf('\0'));
            return ret;
        }

        [Benchmark]
        public string Benchmark_PtrToString() {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try {
                return Marshal.PtrToStringUni(handle.AddrOfPinnedObject());
            } finally {
                handle.Free();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /// Build benchmark with optimization enabled.
            /// https://benchmarkdotnet.org/articles/guides/troubleshooting.html#debugging-benchmarks
            var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
            
            // var summary = BenchmarkRunner.Run<SecondBenchmark>();
        }
    }
}
