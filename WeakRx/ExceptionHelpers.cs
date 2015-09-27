// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Reactive.PlatformServices;

namespace System.Reactive
{
    internal static class ExceptionHelpers
    {
        private static Lazy<IExceptionServices> s_services = new Lazy<IExceptionServices>(Initialize);

        public static void Throw(this Exception exception)
        {
            s_services.Value.Rethrow(exception);
        }

        public static void ThrowIfNotNull(this Exception exception)
        {
            if (exception != null)
                s_services.Value.Rethrow(exception);
        }

        private static IExceptionServices Initialize()
        {
            return PlatformEnlightenmentProvider.Current.GetService<IExceptionServices>() ?? new DefaultExceptionServices();
        }
    }
}

#if HAS_EDI
namespace System.Reactive.PlatformServices
{
    //
    // WARNING: This code is kept *identically* in two places. One copy is kept in System.Reactive.Core for non-PLIB platforms.
    //          Another copy is kept in System.Reactive.PlatformServices to enlighten the default lowest common denominator
    //          behavior of Rx for PLIB when used on a more capable platform.
    //
    internal class DefaultExceptionServices/*Impl*/ : IExceptionServices
    {
        public void Rethrow(Exception exception)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
#else
namespace System.Reactive.PlatformServices
{
    internal class DefaultExceptionServices : IExceptionServices
    {
        public void Rethrow(Exception exception)
        {
            throw exception;
        }
    }
}
#endif