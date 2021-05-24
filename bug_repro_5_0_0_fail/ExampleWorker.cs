using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using bug_repro_5_0_0_fail.Cadence;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neon.Cadence;

namespace bug_repro_5_0_0_fail
{
    public class ExampleWorker : BackgroundService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IOptions<CadenceSettings> _cadenceSettings;
        private readonly ILogger<ExampleWorker> _logger;

        public ExampleWorker(ILogger<ExampleWorker> logger, IHostApplicationLifetime appLifetime,
            IOptions<CadenceSettings> cadenceSettings)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _cadenceSettings = cadenceSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStarted.Register(OnStopping);
            _appLifetime.ApplicationStarted.Register(OnStopped);

            try
            {
                _logger.LogInformation($"Connecting to Cadence at {string.Join(",", _cadenceSettings.Value.Servers)}");
                using var _cadenceClient = await CadenceClient.ConnectAsync(_cadenceSettings.Value);
                _logger.LogInformation("Successfully connected to Cadence");

                _logger.LogInformation("Registering Assembly");
                await _cadenceClient.RegisterAssemblyAsync(Assembly.GetExecutingAssembly());
                _logger.LogInformation("Starting ExampleWorker");
                await _cadenceClient.StartWorkerAsync(_cadenceSettings.Value.DefaultTaskList);

                _logger.LogInformation("Running ExampleWorkflow");
                var stub = _cadenceClient.NewWorkflowStub<IExampleWorkflow>();
                await stub.RunExampleWorkflowAsync();
            }
            catch (ConnectException e)
            {
                _logger.LogError(
                    $"Cannot connect to Cadence. Tried connecting to: {string.Join(",", _cadenceSettings.Value.Servers)}");
                _logger.LogError(e.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
                _logger.LogError(e.StackTrace);
            }

            _appLifetime.StopApplication();
        }

        private void OnStarted()
        {
            _logger.LogInformation("ExampleWorker Starting");
        }

        private void OnStopping()
        {
            _logger.LogInformation("ExampleWorker Stopping");
        }

        private void OnStopped()
        {
            _logger.LogInformation("ExampleWorker Stopped");
        }
    }
}