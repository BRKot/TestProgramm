namespace ClassLibrary
{
    public class Menu
    {
        private bool quite = false;
        private bool humanread = false;
        private string pathFolder = "";
        private string pathLog = "";
        public void MainMenu()
        {
            string choos;

            do
            {

                Console.WriteLine("==========================================================================================================" +
                    "\nВведите команду." +
                   "\n\"-q\" - отключает вывод дерева на консоль" +
                   "\n\"-p\" - ввод папки для её рекурсивного обхода" +
                   "\n\"-o\" - ввод пути к файлу в который запишется файл" +
                   "\n\"-h\" - вывод в понятной человеку форме. Пример: без ввода команды показанный вес файла 1048576 байт, с веденной командой 1 мегабайт" +
                   "\n\"-start\" - вывод дерева папок с учетом всех введенных параметров" +
                   "\n\"-esc - выход из программы" +
                   "\n==========================================================================================================");
                choos = ReadComand();
                SwitchDo(choos);
            } while (choos != "-esc");

        }
        private string ReadComand()
        {
            bool ok = false;
            string line;
            do
            {
                Console.Write("Введите команду:");
                line = Console.ReadLine();
                if (line.ToLower() == "-q" || line.ToLower() == "-p" || line.ToLower() == "-o" || line.ToLower() == "-h"|| line.ToLower() == "-start" || line.ToLower() == "-esc")
                {
                    ok = true;
                }
                else
                    Console.WriteLine("Такой команды не существует.");
                
            } while (!ok);
            return line.ToLower();
        }

        private void SwitchDo(string choos)
        {
            switch(choos)
            {
                case "-q":
                    {
                        this.quite = true;
                        break;
                    }
                case "-p":
                    {
                        this.pathFolder = Helper.EnterPathFolder();
                        break;
                    }
                case "-o":
                    {
                        this.pathLog = Helper.EnterPathFolder();
                        break;
                    }
                case "-h":
                    {
                        this.humanread = true;
                        break;
                    }
                case "-start":
                    {
                        if (this.pathFolder != "")
                        {
                            Helper.CreateFolderTree(this.pathFolder, this.humanread);
                            try { Helper.ShowTree(this.quite, this.pathLog); }
                            catch { Console.WriteLine("\n!!!Не удается записать файл в папку!!!\n Возможно у вас не достаточно прав.\n Попробуйте запустить программу от имени администратора\n"); }
                        }
                        else if (this.pathFolder == "") Console.WriteLine("Перед выводом укажите папку для рекурсивного обхода");
                        break;
                    }
                
            }
        }
    }
}
