using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Task2 {

    // Задача 2
    // Поток «продьюсер» (один) создает элементы данных и помещает их в очередь, на создание одного элемента тратится 100мс
    // Потоки «консьюмеры» (несколько) берут элементы из очереди и обрабатывают их, на обработку тратится 400мс
    // Общее количество элементов данных создаваемых продьюсером равно 100
    // Напишите консольное приложение которое создает потоки «продьюсер» и «консьюмеры» и ожидает когда они закончат работу
    // Выберите оптимальное количество потоков «консьюмер»
    // Проведите измерение времени выполнения(например используя Stopwatch)
    // Для имитации «бурной» деятельности потоков используйте Thread.Sleep(100) и Thread.Sleep(400) соответственно

    internal class Program {
        static ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        static ManualResetEventSlim producerFinished;

        static void Main(string[] args) {
            var sw = new Stopwatch();
            sw.Start();
            using (producerFinished = new ManualResetEventSlim(false)) {
                var tasks = CreateProducerAndConsumers();
                Task.WaitAll(tasks);
            }
            sw.Stop();
            Console.WriteLine($"Done! Elapsed: {sw.Elapsed}");
        }

        static Task[] CreateProducerAndConsumers() {
            var tasks = new List<Task>();
            tasks.Add(Task.Run(Producer));
            for (int i = 0; i < 4; i++) // Try 2, 3, 4 or more consumers
                tasks.Add(Task.Run(Consumer));
            return tasks.ToArray();
        }

        static void Producer() {
            for (int i = 0; i < 100; i++) {
                // Do hard work producing item
                Thread.Sleep(100);
                queue.Enqueue(i);
            }
            producerFinished.Set();
        }

        static void Consumer() {
            while (!producerFinished.IsSet || !queue.IsEmpty) {
                if (queue.TryDequeue(out int item)) {
                    // Do hard work consuming item
                    Thread.Sleep(400);
                }
            }
        }
    }
}
