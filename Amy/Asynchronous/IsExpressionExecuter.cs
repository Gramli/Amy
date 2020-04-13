using System;
using System.Linq;
using System.Threading;

namespace Amy.Asynchronous
{
    public class IsExpressionExecuter : TaskExecuter<bool>
    {
        public override void AddFunction(Func<string, CancellationToken, bool> function)
        {
            var func = new Func<string, CancellationToken, bool>((arg,token) =>
             {
                 var result = function(arg, token);
                 if (!result)
                 {
                     tokenSource.Cancel();
                 }
                 return result;
             });
            base.AddFunction(func);
        }

        public bool IsExpression(params string[] arguments)
        {
            return ExecuteAll(arguments).All(x => x);
        }
    }
}
