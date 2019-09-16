﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Input;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Input.Commands
{
    [ExportCommandGroup(CommandGroup.VisualStudioStandard97)]
    [AppliesTo(ProjectCapability.DotNet)]
    internal class VS97AddItemCommandHandler : AbstractAddItemCommandHandler
    {
        private static readonly ImmutableDictionary<long, ImmutableArray<TemplateDetails>> s_templateDetails = ImmutableDictionary<long, ImmutableArray<TemplateDetails>>.Empty
            //                     Command Id                                Capability                      DirNamePackageGuid          DirNameResourceId                                       TemplateNameResourceId
            .CreateTemplateDetails(VisualStudioStandard97CommandId.AddClass, ProjectCapability.CSharp,       LegacyCSharpPackageGuid,    LegacyCSharpStringResourceIds.IDS_PROJECTITEMTYPE_STR,  LegacyCSharpStringResourceIds.IDS_TEMPLATE_NEWCSharpCLASS )

            .CreateTemplateDetails(VisualStudioStandard97CommandId.AddClass, ProjectCapability.VisualBasic,  LegacyVBPackageGuid,        LegacyVBStringResourceIds.IDS_VSDIR_VBPROJECTFILES,     LegacyVBStringResourceIds.IDS_VSDIR_ITEM_CLASS            );

        protected override ImmutableDictionary<long, ImmutableArray<TemplateDetails>> GetTemplateDetails() => s_templateDetails;

        [ImportingConstructor]
        public VS97AddItemCommandHandler(ConfiguredProject configuredProject, IPhysicalProjectTree projectTree, IUnconfiguredProjectVsServices projectVsServices, IVsUIService<IVsAddProjectItemDlg> addItemDialog, IVsUIService<SVsShell, IVsShell> vsShell)
            : base(configuredProject, projectTree, projectVsServices, addItemDialog, vsShell)
        {
        }
    }
}
