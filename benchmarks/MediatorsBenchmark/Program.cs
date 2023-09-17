using BenchmarkDotNet.Running;
using MediatorsBenchmark;

BenchmarkRunner.Run<SendBenchmark>();
BenchmarkRunner.Run<ForeachAwaitBenchmark>();
BenchmarkRunner.Run<TaskWhenAllBenchmark>();