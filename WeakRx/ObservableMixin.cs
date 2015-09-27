using System;
using System.Threading;
using System.Reactive.Disposables;

namespace WeakRx
{
    /// <summary>
    /// Provides a set of static methods for weakly subscribing delegates to observables.
    /// </summary>
    public static class ObservableMixin
    {
        #region WeakSubscribe delegate-based overloads

        /// <summary>
        /// Subscribes an element handler to an observable sequence.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <returns>IDisposable object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> is null.</exception>
        public static IDisposable WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext)
            where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            var observer = new AnonymousWeakObserver<TARGET, T>(target, onNext);
            observer.Disposable = source.Subscribe(observer);
            return observer;
        }

        /// <summary>
        /// Subscribes an element handler and an exception handler to an observable sequence.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <returns>IDisposable object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onError"/> is null.</exception>
        public static IDisposable WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext, Action<TARGET, Exception> onError)
            where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            var observer = new AnonymousWeakObserver<TARGET, T>(target, onNext, onError);
            observer.Disposable = source.Subscribe(observer);
            return observer;
        }

        /// <summary>
        /// Subscribes an element handler and a completion handler to an observable sequence.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <returns>IDisposable object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IDisposable WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext, Action<TARGET> onCompleted)
            where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            var observer = new AnonymousWeakObserver<TARGET, T>(target, onNext, onCompleted);
            observer.Disposable = source.Subscribe(observer);
            return observer;
        }

        /// <summary>
        /// Subscribes an element handler, an exception handler, and a completion handler to an observable sequence.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <returns>IDisposable object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IDisposable WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext, Action<TARGET, Exception> onError, Action<TARGET> onCompleted)
            where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            var observer = new AnonymousWeakObserver<TARGET, T>(target, onNext, onError, onCompleted);
            observer.Disposable = source.Subscribe(observer);
            return observer;
        }

        #endregion

        #region WeakSubscribe overloads with CancellationToken

        /// <summary>
        /// Subscribes an element handler to an observable sequence, using a CancellationToken to support unsubscription.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> is null.</exception>
        public static void WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext, CancellationToken token)
            where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            source.WeakSubscribe_(new AnonymousWeakObserver<TARGET, T>(target, onNext), token);
        }

        /// <summary>
        /// Subscribes an element handler and an exception handler to an observable sequence, using a CancellationToken to support unsubscription.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onError"/> is null.</exception>
        public static void WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext, Action<TARGET, Exception> onError, CancellationToken token)
            where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            source.WeakSubscribe_(new AnonymousWeakObserver<TARGET, T>(target, onNext, onError), token);
        }

        /// <summary>
        /// Subscribes an element handler and a completion handler to an observable sequence, using a CancellationToken to support unsubscription.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onCompleted"/> is null.</exception>
        public static void WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext, Action<TARGET> onCompleted, CancellationToken token)
             where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            source.WeakSubscribe_(new AnonymousWeakObserver<TARGET, T>(target, onNext, onCompleted), token);
        }

        /// <summary>
        /// Subscribes an element handler, an exception handler, and a completion handler to an observable sequence, using a CancellationToken to support unsubscription.
        /// </summary>
        /// <typeparam name="TARGET">The type of target object.</typeparam>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="target">The subscribing object.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="target"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static void WeakSubscribe<TARGET, T>(this IObservable<T> source, TARGET target, Action<TARGET, T> onNext, Action<TARGET, Exception> onError, Action<TARGET> onCompleted, CancellationToken token)
            where TARGET : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            source.WeakSubscribe_(new AnonymousWeakObserver<TARGET, T>(target, onNext, onError, onCompleted), token);
        }

        private static void WeakSubscribe_<TARGET, T>(this IObservable<T> source, AnonymousWeakObserver<TARGET,T> observer, CancellationToken token)
            where TARGET : class
        {
            if (token.CanBeCanceled)
            {
                if (!token.IsCancellationRequested)
                {
                    var r = new SingleAssignmentDisposable();

                    observer.Disposable = source.Subscribe(
                        observer.OnNext,
                        ex =>
                        {
                            using (r)
                                observer.OnError(ex);
                        },
                        () =>
                        {
                            using (r)
                                observer.OnCompleted();
                        }
                    );

                    r.Disposable = token.Register(observer.Dispose);
                }
            }
            else
            {
                observer.Disposable = source.Subscribe(observer);
            }
        }

        #endregion
    }
}
