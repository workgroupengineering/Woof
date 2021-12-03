﻿namespace Woof.DataProtection;

/// <summary>
/// Specifies the scope of the data protection to be applied by Protect and Unprotect methods.
/// Cross-platform.
/// </summary>
public enum DataProtectionScope {

#pragma warning disable CA1416 // The enumeration works everywhere, this module is multiplatform

    /// <summary>
    /// The protected data is associated with the current user.
    /// Only threads running under the current user context can unprotect the data.
    /// </summary>
    CurrentUser = System.Security.Cryptography.DataProtectionScope.CurrentUser,

    /// <summary>
    /// The protected data is associated with the machine context.
    /// Any process running on the computer can unprotect data.
    /// This enumeration value is usually used in server-specific applications
    /// that run on a server where untrusted users are not allowed access.
    /// </summary>
    LocalMachine = System.Security.Cryptography.DataProtectionScope.LocalMachine,

#pragma warning restore CA1416

}

/// <summary>
/// Provides conversion to and from system types.
/// </summary>
public static class DataProtectionScopeExtensions {

    /// <summary>
    /// Returns the Windows specific system type. Same value.
    /// </summary>
    /// <param name="scope">Data protection scope.</param>
    /// <returns><see cref="System.Security.Cryptography.DataProtectionScope"/>.</returns>
    public static System.Security.Cryptography.DataProtectionScope AsSystemType(this DataProtectionScope scope)
        => (System.Security.Cryptography.DataProtectionScope)(int)scope;

    /// <summary>
    /// Returns the cross-platform type. Same value.
    /// </summary>
    /// <param name="scope">Data protection scope.</param>
    /// <returns><see cref="DataProtectionScope"/>.</returns>
    public static DataProtectionScope AsCrossPlatform(this System.Security.Cryptography.DataProtectionScope scope)
        => (DataProtectionScope)(int)scope;

}