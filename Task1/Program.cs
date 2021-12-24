using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task1 {

    // Задача 1
    // Даны три потока каждый из которых выводит на консоль 1, 2 и 3 соответственно
    // Потоки запускаются в произвольном порядке
    // Сделайте так что бы на консоль было выведено 1, 2 и 3 (по порядку, каждое значение в отдельной строке)

    internal class Program {
        static AutoResetEvent event1;
        static AutoResetEvent event2;

        static void Main(string[] args) {
            using (event1 = new AutoResetEvent(false))
            using (event2 = new AutoResetEvent(false)) {
                var tasks = new Task[] { Task.Run(WriteOne), Task.Run(WriteThree), Task.Run(WriteTwo) };
                Task.WaitAll(tasks);
            }
        }

        static void WriteOne() {
            Console.WriteLine("1");
            event1.Set();
        }

        static void WriteTwo() {
            event1.WaitOne();
            Console.WriteLine("2");
            event2.Set();
        }

        static void WriteThree() {
            event2.WaitOne();
            Console.WriteLine("3");
        }
    }
}
