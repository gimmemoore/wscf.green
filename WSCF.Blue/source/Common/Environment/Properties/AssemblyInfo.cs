using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: AssemblyTitle("WSCF Environment Helper")]
[assembly: AssemblyDescription("Helpers for registering and installing WSCF")]
[assembly: FileIOPermission(SecurityAction.RequestMinimum, Unrestricted=true)]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
