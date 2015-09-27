using System;
using System.Reactive;
using System.Reactive.Disposables;

namespace WeakRx
{
    /// <summary>
    /// Class to create an IObserver&lt;T&gt; instance from delegate-based implementations of the On* methods
    /// that act on a target object.  A weak reference is kept to the target such that the observer doesn't
    /// extend the lifetime of the target.  If the target is garbage collected, then the observer will dispose
    ///  itself on the next invocation of one of the On* methods.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TARGET">The type of target object.</typeparam>
    public sealed class AnonymousWeakObserver<TARGET, T> : ObserverBase<T>
        where TARGET : class
    {
        internal static readonly Action<TARGET> NOP = (target) => { };

        // Note: delegate would be  (target, ex) => ex.Throw(); if class were in System.Reactive.Core
        internal static readonly Action<TARGET, Exception> Throw = (target, ex) => { System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw(); };

        private readonly WeakReference<TARGET> weakref;
        private readonly Action<TARGET, T> onNext;
        private readonly Action<TARGET, Exception> onError;
        private readonly Action<TARGET> onCompleted;

        /// <summary>
        /// Notifies the observer of a new element in the sequence.
        /// </summary>
        /// <param name="value">Next element in the sequence.</param>
        protected override void OnNextCore(T value)
        {
            TARGET target;
            if (weakref.TryGetTarget(out target)) onNext(target, value);
            else Dispose();
        }

        /// <summary>
        /// Notifies the observer that an exception has occurred.
        /// </summary>
        /// <param name="error">The error that has occurred.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        protected override void OnErrorCore(Exception error)
        {
            try
            {
                TARGET target;
                if (weakref.TryGetTarget(out target)) onError(target, error);
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Notifies the observer of the end of the sequence.
        /// </summary>
        protected override void OnCompletedCore()
        {
            try
            {
                TARGET target;
                if (weakref.TryGetTarget(out target)) onCompleted(target);
            }
            finally
            {
                Dispose();
            }
        }

        private readonly SingleAssignmentDisposable m = new SingleAssignmentDisposable();

        /// <summary>
        /// A Disposable tied to the lifetime of this oberserver.
        /// </summary>
        public IDisposable Disposable
        {
            set { m.Disposable = value; }
        }

        /// <summary>
        /// Core implementation of IDisposable.
        /// </summary>
        /// <param name="disposing">true if the Dispose call was triggered by the IDisposable.Dispose method; false if it was triggered by the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing) m.Dispose();
        }

        /// <summary>
        /// Creates an observer from the specified OnNext, OnError, and OnCompleted actions.
        /// </summary>
        /// <param name="target">Object that owns the subscription.</param>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onError">Observer's OnError action implementation.</param>
        /// <param name="onCompleted">Observer's OnCompleted action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public AnonymousWeakObserver(TARGET target, Action<TARGET, T> onNext, Action<TARGET, Exception> onError, Action<TARGET> onCompleted)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            this.weakref = new WeakReference<TARGET>(target);
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        /// <summary>
        /// Creates an observer from the specified OnNext action.
        /// </summary>
        /// <param name="target">Object that owns the subscription.</param>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="onNext"/> is null.</exception>
        public AnonymousWeakObserver(TARGET target, Action<TARGET, T> onNext)
            : this(target, onNext, Throw, NOP)
        {
        }

        /// <summary>
        /// Creates an observer from the specified OnNext and OnError actions.
        /// </summary>
        /// <param name="target">Object that owns the subscription.</param>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onError">Observer's OnError action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onError"/> is null.</exception>
        public AnonymousWeakObserver(TARGET target, Action<TARGET, T> onNext, Action<TARGET, Exception> onError)
            : this(target, onNext, onError, NOP)
        {
        }

        /// <summary>
        /// Creates an observer from the specified OnNext and OnCompleted actions.
        /// </summary>
        /// <param name="target">Object that owns the subscription.</param>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onCompleted">Observer's OnCompleted action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onCompleted"/> is null.</exception>
        public AnonymousWeakObserver(TARGET target, Action<TARGET, T> onNext, Action<TARGET> onCompleted)
            : this(target, onNext, Throw, onCompleted)
        {
        }
    }

}
