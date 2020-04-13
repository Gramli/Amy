using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Amy.Asynchronous
{
    public class TaskExecuter<T>
    {
        private readonly List<Func<string, CancellationToken, T>> functionsToExecute;
        protected CancellationTokenSource tokenSource;
        public TaskExecuter()
        {
            this.functionsToExecute = new List<Func<string, CancellationToken, T>>();
        }

        public virtual void AddFunction(Func<string, CancellationToken, T> function)
        {
            this.functionsToExecute.Add(function);
        }

        public virtual T[] ExecuteAll(params string[] arguments)
        {
            tokenSource = new CancellationTokenSource();
            Task<T>[] tasks = new Task<T>[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                int x = i;
                tasks[x] = (Task<T>.Factory.StartNew(() => { return this.functionsToExecute[x](arguments[x], tokenSource.Token); }));
            }
            return Task.WhenAll(tasks).Result;
        }
    }
}
