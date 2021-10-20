using System;
using System.IO;

namespace SF._8_FINAL.TASK3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите полное имя папки, которую требуется почистить: ");
            string path = Console.ReadLine();
            if (!path.Contains(@":\"))
            {
                Console.WriteLine("Указан некорректный путь к папке: " + path);
            }
            else
            {
                var di = new DirectoryInfo(path);
                try
                {
                    if (di.Exists)
                    {
                        long size_old = DirSize(di);
                        Console.WriteLine("Исходный размер папки {0} составляет {1} байт", path, size_old);
                        int allcnt = DelDirFile(di);
                        long size_new = DirSize(di);
                        Console.WriteLine("Удалено каталогов и папок {0} шт., освобождено {1} байт", allcnt, size_old - size_new);
                        Console.WriteLine("Текущий размер папки {0} составляет {1} байт", path, size_new);
                    }
                    else
                    {
                        Console.WriteLine("Каталога {0} не существует", path);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Рекурсивный метод очистки папки
        /// </summary>
        /// <param name="d"></param>
        static int DelDirFile(DirectoryInfo d)
        {
            int cnt = 0;

            FileInfo[] fil = d.GetFiles();

            foreach (FileInfo file in fil)
            {
                if (file.LastAccessTime < DateTime.Now - TimeSpan.FromMinutes(30))
                {
                    file.Delete();
                    cnt++;
                }
            }

            DirectoryInfo[] direct = d.GetDirectories();

            foreach (DirectoryInfo dir in direct)
            {
                if (dir.LastAccessTime < DateTime.Now - TimeSpan.FromMinutes(30))
                {
                    dir.Delete(true);
                    cnt++;
                }
                else
                {
                    DelDirFile(dir);
                }
            }
            return cnt;
        }

        /// <summary>
        /// Рекурсивный метод подсчета объема папки с файлами
        /// </summary>
        /// <param name="d"></param>
        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fil = d.GetFiles();

            foreach (FileInfo file in fil)
            {
                size += file.Length;
            }

            DirectoryInfo[] direct = d.GetDirectories();

            foreach (DirectoryInfo dir in direct)
            {
                size += DirSize(dir);
            }

            return size;
        }
    }
}
