using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eomat.Helpers.Resquests
{

    class RequestHandler : IDisposable
    {
        SemaphoreSlim _semaphore = new SemaphoreSlim(25);
        HashSet<Task> _pending = new HashSet<Task>();
        object _lock = new Object();

        async Task ProcessAsync(string data)
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    // simuate work
                    Thread.Sleep(1000);
                    Console.WriteLine(data);
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async void QueueItemAsync(string data)
        {
            var task = ProcessAsync(data);
            lock (_lock)
                _pending.Add(task);
            try
            {
                await task;
            }
            catch
            {
                if (!task.IsCanceled && !task.IsFaulted)
                    throw; // not the task's exception, rethrow
                // don't remove faulted/cancelled tasks from the list
                return;
            }
            // remove successfully completed tasks from the list 
            lock (_lock)
                _pending.Remove(task);
        }

        public async Task WaitForCompleteAsync()
        {
            Task[] tasks;
            lock (_lock)
                tasks = _pending.ToArray();
            await Task.WhenAll(tasks);
        }
        public void Dispose()
        {
            if (_semaphore == null) return;
            _semaphore.Dispose();
            _semaphore = null;
        }
    }
}
