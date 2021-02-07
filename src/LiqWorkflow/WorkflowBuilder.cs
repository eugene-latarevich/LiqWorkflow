using System.Collections.Generic;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models.Builder;
using LiqWorkflow.Abstractions.Models.Factories;
using LiqWorkflow.Branches;
using LiqWorkflow.Common.Extensions;

namespace LiqWorkflow
{
    class WorkflowBuilder : IWorkflowBuilder
    {
        private readonly IContainer _container;
        private readonly IWorkflowBranchFactory _workflowBranchFactory;
        private readonly List<CreatingBranchConfiguration> _branchesData = new List<CreatingBranchConfiguration>();
        private readonly List<CreatingActivityConfiguration> _activitiesData = new List<CreatingActivityConfiguration>();

        private IWorkflowConfiguration _workflowConfiguration;

        public WorkflowBuilder(IContainer container)
        {
            _container = container;
            _workflowBranchFactory = _container.GetService<IWorkflowBranchFactory>();
        }

        public IWorkflowBuilder WithConfiguration(IWorkflowConfiguration configuration)
        {
            _workflowConfiguration = configuration;
            
            return this;
        }

        public IWorkflowBuilder WithBranch(IBranchInitData initData)
        {
            _branchesData.Add(initData.CreateBranchConfigurationForBuilder(_container));

            return this;
        }

        public IWorkflowBuilder WithActivity(IActivityInitData initData)
        {
            _activitiesData.Add(initData.CreateActivityConfigurationForBuilder(_container));

            return this;
        }

        public IWorkflow Build()
        {
            var messageEventBroker = _container.GetService<IWorkflowMessageEventBroker>();

            var branches = _workflowBranchFactory.BuildConnected(new ConnectedBranchesConfiguration(_workflowConfiguration, _activitiesData, _branchesData));

            return new Workflow(_workflowConfiguration, branches, messageEventBroker);
        }
    }
}
