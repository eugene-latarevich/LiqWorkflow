using System;
using System.Collections.Generic;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models.Builder;
using LiqWorkflow.Abstractions.Models.Factories;
using LiqWorkflow.Branches;
using LiqWorkflow.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow
{
    class WorkflowBuilder : IWorkflowBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWorkflowBranchFactory _workflowBranchFactory;
        private readonly List<CreatingBranchConfiguration> _branchesData = new List<CreatingBranchConfiguration>();
        private readonly List<CreatingActivityConfiguration> _activitiesData = new List<CreatingActivityConfiguration>();

        private IWorkflowConfiguration _workflowConfiguration;

        public WorkflowBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _workflowBranchFactory = _serviceProvider.GetService<IWorkflowBranchFactory>();
        }

        public IWorkflowBuilder WithConfiguration(IWorkflowConfiguration configuration)
        {
            _workflowConfiguration = configuration;
            
            return this;
        }

        public IWorkflowBuilder WithBranch(IBranchInitData initData)
        {
            _branchesData.Add(initData.CreateBranchConfigurationForBuilder());

            return this;
        }

        public IWorkflowBuilder WithActivity(IActivityInitData initData)
        {
            _activitiesData.Add(initData.CreateActivityConfigurationForBuilder());

            return this;
        }

        public IWorkflow Build()
        {
            var messageEventBroker = _serviceProvider.GetService<IWorkflowMessageEventBroker>();

            var branches = _workflowBranchFactory.BuildConnected(new ConnectedBranchesConfiguration(_workflowConfiguration, _activitiesData, _branchesData));

            return new Workflow(_workflowConfiguration, branches, messageEventBroker);
        }
    }
}
