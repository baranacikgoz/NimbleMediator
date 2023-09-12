using BenchmarkDotNet.Running;
using MediatorsBenchmark;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;

BenchmarkRunner.Run<SequentialBenchmark>();
BenchmarkRunner.Run<ConcurrentBenchmark>();

// var services = new ServiceCollection();

// services.AddNimbleMediator(config =>
// {
//     config.RegisterHandlersFromAssembly(typeof(CreateUserRequestNimbleMediator).Assembly);
// });
