using System;
using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Factories;
using LiqWorkflow.Abstractions.Models.Builder;
using LiqWorkflow.Abstractions.Models.Factories;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow.Factories
{
    class WorkflowBranchFactory : IWorkflowBranchFactory
    {
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;

        public WorkflowBranchFactory(IWorkflowMessageEventBroker workflowMessageEventBroker)
        {
            _workflowMessageEventBroker = workflowMessageEventBroker;
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

                var branch = (IWorkflowBranch)Activator.CreateInstance(branchData.Type, branchData.Configuration, branchActivities.ToDictionary(x => x.Configuration.ActivityId), _workflowMessageEventBroker);
                
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

                var activity = (IWorkflowActivity)Activator.CreateInstance(activityData.Type, activityData.Configuration, activityChildBranches.ToDictionary(x => x.Configuration.BranchId), _workflowMessageEventBroker);

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

        private bool ExistsActivity(string activityId, ConnectedBranchesConfiguration configuration) 
            => configuration.Activities.Any(x => x.Configuration.ActivityId == activityId);

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
    }
}
