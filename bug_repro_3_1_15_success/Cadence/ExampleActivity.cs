using System.Threading.Tasks;
using Neon.Cadence;

namespace bug_repro_3_1_15_success.Cadence
{
    [Activity(AutoRegister = true)]
    public class ExampleActivity : ActivityBase, IExampleActivity
    {
        public Task<bool> RunExampleActivityAsync()
        {
            Activity.Logger.LogInfo("RunExampleActivityAsync - Executing");
            return Task.FromResult(true);
        }
    }

    [ActivityInterface]
    public interface IExampleActivity : IActivity
    {
        [ActivityMethod]
        Task<bool> RunExampleActivityAsync();
    }
}