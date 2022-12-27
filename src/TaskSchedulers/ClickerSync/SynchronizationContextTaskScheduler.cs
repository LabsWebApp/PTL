using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClickerSync
{
    internal class SynchronizationContextTaskScheduler : TaskScheduler
    {
        private readonly SynchronizationContext _synchronizationContext;

        public SynchronizationContextTaskScheduler()
            : this(SynchronizationContext.Current 
                   ?? throw new NotSupportedException("Приложение без SynchronizationContext")) { }

        public SynchronizationContextTaskScheduler(SynchronizationContext synchronizationContext) =>
            _synchronizationContext = synchronizationContext;

        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task) =>
            _synchronizationContext.Post(_ => base.TryExecuteTask(task), null);

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
            _synchronizationContext == SynchronizationContext.Current && base.TryExecuteTask(task);
    }
}
