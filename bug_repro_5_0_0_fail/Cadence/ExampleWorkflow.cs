using System.Threading.Tasks;
using Neon.Cadence;

namespace bug_repro_5_0_0_fail.Cadence
{
    [Workflow(AutoRegister = true)]
    public class ExampleWorkflow : WorkflowBase, IExampleWorkflow
    {
        public async Task<bool> RunExampleWorkflowAsync()
        {
            Workflow.Logger.LogInfo("RunExampleWorkflowAsync - starting");
            var activityStub = Workflow.Current.NewActivityStub<IExampleActivity>();
            var result = await activityStub.RunExampleActivityAsync();
            Workflow.Logger.LogInfo($"RunExampleWorkflowAsync - Activity Result: {result}");
            return result;
        }
    }

    [WorkflowInterface]
    public interface IExampleWorkflow : IWorkflow
    {
        [WorkflowMethod]
        public Task<bool> RunExampleWorkflowAsync();
    }
}