using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs
{
    using static Helpers;
    public struct Result<TFailure, TSuccess>
    {
        public TFailure Failure { get; internal set; }
        public TSuccess Success { get; internal set; }

        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        public Option<TFailure> OptionalFailure => IsFailure ? Some(Failure) : None;

        public Option<TSuccess> OptionalSuccess => IsSuccess ? Some(Success) : None;

        internal Result(TFailure failure)
        {
            IsFailure = true;
            Failure = failure;
            Success = default(TSuccess);
        }

        internal Result(TSuccess success)
        {
            IsFailure = false;
            Failure = default(TFailure);
            Success = success;
        }

        public TResult Match<TResult>(
                Func<TFailure, TResult> failure,
                Func<TSuccess, TResult> success
            )
            => IsFailure ? failure(Failure) : success(Success);

        public Unit Match(
                Action<TFailure> failure,
                Action<TSuccess> success
            )
            => Match(ToFunc(failure), ToFunc(success));

        public static implicit operator Result<TFailure, TSuccess>(TFailure failure)
            => new Result<TFailure, TSuccess>(failure);

        public static implicit operator Result<TFailure, TSuccess>(TSuccess success)
            => new Result<TFailure, TSuccess>(success);

        public static Result<TFailure, TSuccess> Of(TSuccess obj) => obj;
        public static Result<TFailure, TSuccess> Of(TFailure obj) => obj;
    }

    public struct Unit
    {
        public static Unit Successful { get { return new Unit(); } }
    }

    public static partial class Helpers
    {
        private static readonly Unit unit = new Unit();

        public static Unit Unit() => unit;

        public static Func<T, Unit> ToFunc<T>(Action<T> action) => o =>
        {
            action(o);
            return Unit();
        };

        public static Func<Unit> ToFunc(Action action) => () =>
        {
            action();
            return Unit();
        };
    }
}
