﻿// Copyright(c) Microsoft.All Rights Reserved.Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.ProjectSystem.VS
{
    /// <inheritdoc cref="ISolutionService"/>
    [Export(typeof(ISolutionService))]
    internal sealed class SolutionService : OnceInitializedOnceDisposedAsync, ISolutionService, IVsSolutionEvents, IVsPrioritizedSolutionEvents, IDisposable
    {
        private readonly JoinableTaskContext _joinableTaskContext;
        private readonly IVsUIService<IVsSolution> _solution;
        private uint _cookie = VSConstants.VSCOOKIE_NIL;
        
        public bool IsSolutionClosing { get; private set; }

        [ImportingConstructor]
        public SolutionService(JoinableTaskContext joinableTaskContext, IVsUIService<SVsSolution, IVsSolution> solution)
            : base(new JoinableTaskContextNode(joinableTaskContext))
        {
            _joinableTaskContext = joinableTaskContext;
            _solution = solution;
        }

        public new Task InitializeAsync(CancellationToken cancellationToken)
        {
            return base.InitializeAsync(cancellationToken);
        }

        // We handle both prioritized and regular before/after events to update state as early as possible,
        // and ensure we set the value regardless of whether the caller supports one or both interfaces.

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)                                 => UpdateClosing(false);
        public int OnBeforeCloseSolution(object pUnkReserved)                                                 => UpdateClosing(true);
        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)                                    => HResult.OK;
        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)              => HResult.OK;
        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)                                => HResult.OK;
        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)               => HResult.OK;
        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)                        => HResult.OK;
        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)            => HResult.OK;
        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)                                => HResult.OK;
        public int OnAfterCloseSolution(object pUnkReserved)                                                  => HResult.OK;

        public int PrioritizedOnAfterOpenSolution(object pUnkReserved, int fNewSolution)                      => UpdateClosing(false);
        public int PrioritizedOnBeforeCloseSolution(object pUnkReserved)                                      => UpdateClosing(true);
        public int PrioritizedOnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)                         => HResult.OK;
        public int PrioritizedOnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)                     => HResult.OK;
        public int PrioritizedOnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)    => HResult.OK;
        public int PrioritizedOnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy) => HResult.OK;
        public int PrioritizedOnAfterCloseSolution(object pUnkReserved)                                       => HResult.OK;
        public int PrioritizedOnAfterMergeSolution(object pUnkReserved)                                       => HResult.OK;
        public int PrioritizedOnBeforeOpeningChildren(IVsHierarchy pHierarchy)                                => HResult.OK;
        public int PrioritizedOnAfterOpeningChildren(IVsHierarchy pHierarchy)                                 => HResult.OK;
        public int PrioritizedOnBeforeClosingChildren(IVsHierarchy pHierarchy)                                => HResult.OK;
        public int PrioritizedOnAfterClosingChildren(IVsHierarchy pHierarchy)                                 => HResult.OK;
        public int PrioritizedOnAfterRenameProject(IVsHierarchy pHierarchy)                                   => HResult.OK;
        public int PrioritizedOnAfterChangeProjectParent(IVsHierarchy pHierarchy)                             => HResult.OK;
        public int PrioritizedOnAfterAsynchOpenProject(IVsHierarchy pHierarchy, int fAdded)                   => HResult.OK;

        private HResult UpdateClosing(bool isClosing)
        {
            IsSolutionClosing = isClosing;
            return HResult.OK;
        }

        protected override async Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            await _joinableTaskContext.Factory.SwitchToMainThreadAsync(cancellationToken);

            IVsSolution? solution = _solution.Value;
            Assumes.Present(solution);

            Verify.HResult(solution.AdviseSolutionEvents(this, out _cookie));
        }

        protected override async Task DisposeCoreAsync(bool initialized)
        {
            if (initialized && _cookie != VSConstants.VSCOOKIE_NIL)
            {
                await _joinableTaskContext.Factory.SwitchToMainThreadAsync();

                IVsSolution? solution = _solution.Value;
                Assumes.Present(solution);

                Verify.HResult(solution.UnadviseSolutionEvents(_cookie));
                _cookie = VSConstants.VSCOOKIE_NIL;
            }
        }
    }
}