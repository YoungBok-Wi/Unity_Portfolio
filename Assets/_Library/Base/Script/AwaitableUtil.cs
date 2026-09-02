using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>Unity 내장 Awaitable 용 비동기 유틸. UniTask 를 쓰지 않는 대신 자주 필요한 것들을 채워 둔다</summary>
    public static class AwaitableUtil
    {
        #region Function
        /// <summary>이미 끝난 Awaitable 을 반환한다 (UniTask.CompletedTask 자리)</summary>
        public static Awaitable Completed()
        {
            var source = new AwaitableCompletionSource();
            source.SetResult();
            return source.Awaitable;
        }
        /// <summary>result 를 담은 채 이미 끝난 Awaitable 을 반환한다 (UniTask.FromResult 자리)</summary>
        public static Awaitable<T> FromResult<T>(T result)
        {
            var source = new AwaitableCompletionSource<T>();
            source.SetResult(result);
            return source.Awaitable;
        }
        /// <summary>tasks 를 모두 기다려 결과를 tasks 순서대로 반환한다 (UniTask.WhenAll 자리)</summary>
        // 순차 await 처럼 보이지만 Awaitable 은 만들어질 때 이미 돌기 시작하므로 실제로는 동시에 진행된다
        public static async Awaitable<T[]> WhenAll<T>(List<Awaitable<T>> tasks)
        {
            var results = new T[tasks.Count];
            for (int i = 0; i < tasks.Count; i++)
                results[i] = await tasks[i];
            return results;
        }
        #endregion
    }
}
