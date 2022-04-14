namespace ClassLibrary
{
    public static  class Helper
    {
        public static List<string> filesAndFolders = new List<string>();
        static public string FolderName(string path) => path.Split('\\')[path.Split('\\').Length - 1];

        static public long FolderInfo(string folder)
        {
            long folderSize = 0;

            try
            {
                var files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
                foreach (string file in files) folderSize += (long)((new FileInfo(file)).Length);
            }
            catch
            {
                Console.WriteLine("\n!!!Не достаточно прав для записи в корневую папку!!!\n");
            }
            

            return folderSize;
        }
        //Привидение к понятному человеку типу
        static public string SetInfo(long info, bool humanread)
        {
            if (humanread)
            {
                string units = "bytes";
                double dInfo = 0;
                if (info > 1024)
                {
                    dInfo = info / 1024;
                    units = "kilobytes";
                }
                if (dInfo > 1024)
                {
                    dInfo /= 1024;
                    units = "megabytes";
                }
                if (dInfo > 1024)
                {
                    dInfo /= 1024;
                    units = "gigabytes";
                }
                dInfo = Math.Round(dInfo, 2);

                return $" ({dInfo.ToString()} {units})";
            }
            else 
                return $"({info.ToString()} bytes)";


        }
        //Подсчет веса файла по ASCII
        static private int ListInfo()
        {
            int info = 0;
            foreach (string element in filesAndFolders)
                info += (element.Length + 1);
            return info-1;
        }

        public static async void WriteToFile(string pathLog = "")
        {
            var thisDay = DateTime.Today;
            try
            {
                await File.WriteAllLinesAsync($"{pathLog}\\{ListInfo()}-{thisDay.ToString("d")}".Replace('.', '-') + ".txt", filesAndFolders);
            }
            catch
            {
                Console.WriteLine("\n!!!Не достаточно прав для записи в корневую папку!!!\n");
            }

        }

        //Создание дерева
        public static void CreateFolderTree(string path, bool humanread, int i = 0)
        {
            if(!Directory.Exists(path))
            {
                Console.WriteLine("Перед выводом иерархии требуется указать папку");
                return;
            }
            i++;
            //Добавление информации о папке
            filesAndFolders.Add(new string('-', i) + Helper.FolderName(path) + Helper.SetInfo(Helper.FolderInfo(path), humanread));


            i++;
            List<string> files = new List<string>();
            SaveFilesName(ref files, path);//запись всех файлов в список
            //Добавление информации о файлах в список
            foreach (string file in files)
            {
                filesAndFolders.Add(new string('-', i) + Path.GetFileName(file) + Helper.SetInfo((new FileInfo(file)).Length, humanread));
            }
            files.Clear();
            SaveDirectoryName(ref files, path);// запись всех папок в список
            //Входим во все папки, находящиеся в папке
            foreach (string file in files)
            {
                CreateFolderTree(file, humanread, i);
            }
        }
        //Вывести дерево
        public static void ShowTree(bool quite, string pathLog = "")
        {
            Helper.WriteToFile(pathLog);
            if(!quite)
                foreach (var node in filesAndFolders)
                {
                    Console.WriteLine(node);
                }
            Console.WriteLine("Нажмите Enter чтобы продолжить");
            Console.ReadLine();
            Console.Clear();
            filesAndFolders.Clear();
        }
        //Запись всех директорий в список
        static void SaveDirectoryName(ref List<string> names, string path)
        {
            try
            {
                var files = Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
                ForeachAdd(ref names, ref files);
            }
            catch
            {
                Console.WriteLine("\n!!!Не достаточно прав для записи в корневую папку!!!\n");
            }
            
        }
        //Запись всех файлов папки в список
        static void SaveFilesName(ref List<string> names, string path)
        {
            try
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                ForeachAdd(ref names, ref files);
            }
            catch
            {
                Console.WriteLine("\n!!!Не достаточно прав для записи в корневую папку!!!\n");
            }
        }
        //Добавление в список папок и файлов
        static void ForeachAdd(ref List<string> names, ref string[] files)
        {
            foreach (string file in files)
            {
                names.Add(file);
            }
        }
        static public string EnterPathFolder()
        {
            string path;
            bool ok = true;
            do
            {
                Console.Write("Введите директорию: ");
                path = Console.ReadLine();
                if (!Directory.Exists(path))
                    Console.WriteLine("Такой директории не существет.");
                else
                    ok = false;
            } while (ok);
            return path;
        }

    }
}