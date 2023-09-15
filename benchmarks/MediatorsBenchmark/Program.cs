using BenchmarkDotNet.Running;
using MediatorsBenchmark;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;

BenchmarkRunner.Run<SendBenchmark>();
BenchmarkRunner.Run<ForeachAwaitBenchmark>();
BenchmarkRunner.Run<TaskWhenAllBenchmark>();