using System;
using System.Collections.Generic;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models.Builder;
using LiqWorkflow.Abstractions.Models.Factories;
using LiqWorkflow.Branches;
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

        public IWorkflowBuilder WithBranch<TBranch>(IBranchConfiguration configuration)
            where TBranch : class, IWorkflowBranch 
                => WithBranch(typeof(TBranch), configuration);

        public IWorkflowBuilder WithBranch(Type type, IBranchConfiguration configuration)
        {
            _branchesData.Add(new CreatingBranchConfiguration{Type = type, Configuration = configuration});

            return this;
        }

        public IWorkflowBuilder WithActivity<TActivity>(IActivityConfiguration configuration)
            where TActivity : class, IWorkflowActivity
                => WithActivity(typeof(TActivity), configuration);

        public IWorkflowBuilder WithActivity(Type type, IActivityConfiguration configuration)
        {
            _activitiesData.Add(new CreatingActivityConfiguration{Type = type, Configuration = configuration});

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
