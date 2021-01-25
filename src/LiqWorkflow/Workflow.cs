using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Abstractions.Models.Enums;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow
{
    public class Workflow : IWorkflow
    {
        private readonly IEnumerable<IWorkflowBranch> _branches;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public Workflow(
            IWorkflowConfiguration configuration,
            IEnumerable<IWorkflowBranch> branches)
        {
            Configuration = configuration;

            _branches = branches;
        }

        public WorkflowStatus Status { get; private set; } = WorkflowStatus.NotStarted;

        public IWorkflowConfiguration Configuration { get; }

        public async Task<WorkflowResult> StartAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                ThrowIfNotValidConfiguration();

                Status = WorkflowStatus.Starting;

                await _branches.ForEachAsync(branch => branch.PulseAsync(Configuration.CancellationTokenSource.Token));

                Status = WorkflowStatus.Executing;

                return WorkflowResult.Ok();
            }
            catch (Exception exception)
            {

            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return WorkflowResult.Error();
        }

        public async Task<WorkflowResult> StopAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                Status = WorkflowStatus.Stopping;

                Configuration.CancellationTokenSource.Cancel(true);

                Status = WorkflowStatus.Stopped;

                return WorkflowResult.Ok();
            }
            catch (Exception exception)
            {

            }
            finally
            {
                _semaphoreSlim.Release();
            }
           
            return WorkflowResult.Error();
        }

        private void ThrowIfNotValidConfiguration()
        {
            if (_branches == null || !_branches.Any() || _branches.Any(x => !x.IsValid()))
            {
                throw new InvalidWorkflowConfigurationException();
            }
        }
    }
}
