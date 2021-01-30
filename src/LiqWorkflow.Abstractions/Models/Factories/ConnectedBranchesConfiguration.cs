using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Models.Builder;

namespace LiqWorkflow.Abstractions.Models.Factories
{
    public class ConnectedBranchesConfiguration
    {
        private readonly IEnumerable<CreatingBranchConfiguration> _branchesData;

        public ConnectedBranchesConfiguration(IEnumerable<CreatingActivityConfiguration> activitiesData, IEnumerable<CreatingBranchConfiguration> branchesData)
        {
            _branchesData = branchesData.OrderBy(x => x.Configuration.Order);
            
            ActivitiesData = activitiesData;

            Branches = new List<IWorkflowBranch>(branchesData.Count());
            Activities = new List<IWorkflowActivity>(branchesData.Count());
        }

        public string ProcessingActivityId { get; private set; }

        public ICollection<IWorkflowActivity> Activities { get; }
        public ICollection<IWorkflowBranch> Branches { get; }

        public CreatingBranchConfiguration ProcessingBranchData { get; private set; }

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

        public ConnectedBranchesConfiguration WithBranchData(CreatingBranchConfiguration configuration)
        {
            ProcessingBranchData = configuration;

            return this;
        }

        public ConnectedBranchesConfiguration WithActivity(string activityId)
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
