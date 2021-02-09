using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models.Builder;
using LiqWorkflow.Abstractions.Models.Factories;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow.Branches
{
    class WorkflowBranchFactory : IWorkflowBranchFactory
    {
        private readonly IContainer _container;

        public WorkflowBranchFactory(IContainer container)
        {
            _container = container;
        }

        public IEnumerable<IWorkflowBranch> BuildConnected(ConnectedBranchesConfiguration configuration)
        {
            foreach (var branchData in configuration.BranchesData)
            {
                if (configuration.Branches.Any(x => x.Configuration.BranchId == branchData.Configuration.BranchId))
                {
                    continue;
                }

                var branchActivities = CreateBranchActivities(configuration.WithProcesingBranchData(branchData));

                var branch = _container.GetService<WorkflowBranch>(branchData.GetConstructorParameters(configuration.WorkflowConfiguration, branchActivities));
                
                configuration.Branches.Add(branch);
            }

            configuration.RefreshProcessingInfo();

            return configuration.Branches.Where(x => x.Configuration.StartingBranch).AsEnumerable();
        }

        private IEnumerable<IWorkflowActivity> CreateBranchActivities(ConnectedBranchesConfiguration configuration)
        {
            foreach (var activityId in configuration.ProcessingBranchData.Configuration.ActivityIds)
            {
                if (ExistsActivity(activityId, configuration))
                {
                    continue;
                }

                var activityData = FindActivityData(activityId, configuration);
                
                var activityChildBranches = GetOrCreateActivityBranches(activityData, configuration);

                var activity = CreateActivity(activityData, activityChildBranches);

                configuration.Activities.Add(activity);
            }

            var branchActivities = configuration.Activities
                .Where(activity => activity.Branches
                    .Any(branch => branch.Value.Configuration.BranchId == configuration.ProcessingBranchData.Configuration.BranchId));

            if (!branchActivities.Any())
            {
                throw new NotFoundException($"Activities weren't found for branch with BranchId={configuration.ProcessingBranchData.Configuration.BranchId}");
            }

            return branchActivities;
        }

        private bool ExistsActivity(string activityId, ConnectedBranchesConfiguration configuration) => configuration.Activities.Any(x => x.Configuration.ActivityId == activityId);

        private CreatingActivityConfiguration FindActivityData(string activityId, ConnectedBranchesConfiguration configuration)
        {
            var activityData = configuration.ActivitiesData.FirstOrDefault(x => x.Configuration.ActivityId == activityId);
            return activityData ?? throw new NotFoundException($"Activity data with ActivityId={activityId} wasn't found.");
        }

        private IEnumerable<IWorkflowBranch> GetOrCreateActivityBranches(CreatingActivityConfiguration activityData, ConnectedBranchesConfiguration configuration)
        {
            if (activityData.Configuration.Transition.HasChildBranches)
            {
                BuildConnected(configuration.ForActivity(activityData.Configuration.ActivityId));

                var activityBranches = configuration.Branches.Where(branch => activityData.BranchIds.Contains(branch.Configuration.BranchId));

                if (!activityBranches.Any())
                {
                    throw new NotFoundException($"Branches weren't found for activity with ActivityId={activityData.Configuration.ActivityId}");
                }

                return activityBranches;
            }

            return Enumerable.Empty<IWorkflowBranch>();
        }

        private IWorkflowActivity CreateActivity(CreatingActivityConfiguration activityData, IEnumerable<IWorkflowBranch> activityChildBranches)
        {
            var serviceKey = activityData.Configuration.RestorePoint
                ? activityData.RestoredActivityKey ?? ServiceKeys.RestorableActivity
                : activityData.ActiviyKey ?? ServiceKeys.ExecutableActivity;

            return _container.GetKeyedService<IWorkflowActivity>(serviceKey, activityData.GetConstructorParameters(activityChildBranches));
        }
    }
}
