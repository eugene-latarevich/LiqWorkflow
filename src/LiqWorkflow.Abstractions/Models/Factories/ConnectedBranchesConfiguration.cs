using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Models.Builder;

namespace LiqWorkflow.Abstractions.Models.Factories
{
    public class ConnectedBranchesConfiguration
    {
        private readonly IEnumerable<CreatingBranchConfiguration> _branchesData;

        public ConnectedBranchesConfiguration(
            IWorkflowConfiguration workflowConfiguration,
            IEnumerable<CreatingActivityConfiguration> activitiesData, 
            IEnumerable<CreatingBranchConfiguration> branchesData)
        {
            _branchesData = branchesData.OrderBy(x => x.Configuration.Order);
            
            ActivitiesData = activitiesData;

            WorkflowConfiguration = workflowConfiguration;
            Branches = new List<IWorkflowBranch>(branchesData.Count());
            Activities = new List<IWorkflowActivity>(branchesData.Count());
        }

        public ICollection<IWorkflowActivity> Activities { get; }
        public ICollection<IWorkflowBranch> Branches { get; }

        public string ProcessingActivityId { get; private set; }
        public CreatingBranchConfiguration ProcessingBranchData { get; private set; }

        public IWorkflowConfiguration WorkflowConfiguration { get; }

        public IEnumerable<CreatingBranchConfiguration> BranchesData
        {
            get
            {
                if (string.IsNullOrEmpty(ProcessingActivityId))
                {
                    return _branchesData;
                }

                var activityData = ActivitiesData.FirstOrDefault(x => x.Configuration.ActivityId == ProcessingActivityId);
                return _branchesData.Where(branchData => activityData.BranchIds.Contains(branchData.Configuration.BranchId));
            }
        }
        public IEnumerable<CreatingActivityConfiguration> ActivitiesData { get; }

        public ConnectedBranchesConfiguration WithProcesingBranchData(CreatingBranchConfiguration configuration)
        {
            ProcessingBranchData = configuration;

            return this;
        }

        public ConnectedBranchesConfiguration ForActivity(string activityId)
        {
            ProcessingActivityId = activityId;

            return this;
        }

        public void RefreshProcessingInfo()
        {
            ProcessingActivityId = null;
            ProcessingBranchData = null;
        }
    }
}
