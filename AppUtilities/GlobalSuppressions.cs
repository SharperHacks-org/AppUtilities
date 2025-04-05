// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0160:Convert to block scoped namespace", Justification = "Block scope is a waste of screen space when there's only one namespace per file.", Scope = "namespace", Target = "~N:SharperHacks.CoreLibs.AppUtilities")]
[assembly: SuppressMessage("Style", "IDE0011:Add braces", Justification = "They aren't always needed", Scope = "member", Target = "~P:SharperHacks.CoreLibs.AppUtilities.AppConfig.RootDataPath")]
