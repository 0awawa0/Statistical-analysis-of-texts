using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Шифр_простой_замены
{
    public partial class Decrypt : Form
    {
        
        public Decrypt()
        {
            InitializeComponent();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void Decrypt_Shown(object sender, EventArgs e)
        {

        }
        //
        //Обработка загрузки формы
        //
        private void Decrypt_Load(object sender, EventArgs e)
        {

            string alfavite = String.Empty;
            for (int i = 0; i < Form1.Alf.Length; i++)
            {
                alfavite = alfavite + Form1.Alf[i].ToString(); //Читаем используемый алфавит
            }
            label1.Text = "Используемый алфавит:\n" + alfavite; //Выводим используемый алфавит на форму в формате метки

            foreach (var button in this.Controls) //Для каждого элемента из списка Controls для данной формы
            {
                if (button is Button) //Если данный элемент является объектом класса Button
                {
                    ((Button)button).MouseEnter += Program.button_Enter; //Задаем обработку наведения курсора на кнопку
                    ((Button)button).MouseLeave += Program.button_Leave; //Задаем обработку убирания курсора с кнопки
                }
            }

        }

        //
        //Обработка закрытия формы
        //
        private void Decrypt_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); //Закрываем приложение
        }

        //
        //Функция расшифровки
        //  Здесь сравниваются две таблицы статистики: Letter содержит статистику из файла, Letter1 содержит статистику шифротекста
        public string DecryptFunc(string text, List<Data> Letter1, List<Data> Letter)
        {
            string text1 = String.Empty; //Переменная text1 содержит расшифрованный текст
            for (int i = 0, j = 0; i < text.Length; i++, j = 0)
            {
                if (Array.IndexOf(Form1.Alf, text[i]) == -1) //Если i-й символ шифротекста не входит в используемый алфавит
                    text1 = text1 + text[i]; //Он игнорируется и записывается в расшифрованный текст
                else //иначе
                {
                    string letter = text[i].ToString(); //Данный символ записывается в отдельную переменную
                    while (j < Letter.Count && letter != Letter1[j].Letter) //Ищем индекс данного символа в таблице
                        j++;
                    if (j >= Letter.Count) //Если мы прошли всю статистику и не нашли нужный символ
                    {
                        MessageBox.Show("Превышено допустимое значение счетчика." + //Выводим сообщение
                            "\nВозможно была допущена ошибка при вводе." +
                            "Читайте 'О программе'.");
                        return String.Empty; //Возвращаем управление вызывающей функции с пустой строкой
                    }
                    double min = 1;//Базовое значение минимальной частоты принимаем за 1
                    int kmin = 0; //Индекс символа с минимальной частотой
                    for (int k = 0; k < Letter.Count; k++) //Ищем минимальную разность частот двух букв в разных таблицах
                        if (min > Math.Sqrt(Math.Pow((Letter1[j].freq - Letter[k].freq), 2) / 2)) //Если разность частот меньше минимальной разности
                        {
                            min = Math.Sqrt(Math.Pow((Letter1[j].freq - Letter[k].freq), 2) / 2); //Переписываем минимум
                            kmin = k; //Запоминаем индекс буквы, разность частот с которой мнимальна
                        }
                    text1 = text1 + Letter[kmin].Letter; //В расшифрованный текст записываем букву из таблицы статистики из файла
                }
            }
            return text1; //Возвращаем управление вызывающей функцие с расшифрованным текстом          
        }

        //Обработка нажатия на кнопку 3 ("Выйти")
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Приложение закрывается
        }

        //Обработка нажатия на кнопку 2 ("В главное меню")
        private void button2_Click(object sender, EventArgs e)
        {
            new Form1().Show(); //Создаем и выводим на экран форму главного меню
            this.Hide(); //А данную форму скрываем
        }

        //Обработка нажатия на кнопку 1 ("Расшифровать")
        private void button1_Click(object sender, EventArgs e)
        {
            string sub; //Данная переменная хранит подстроку для чтения статистики из файла
            string text = textBox1.Text.ToLower(); //Считываем текст с формы 1 (верхняя)
            List<Data> Letter1 = new List<Data>(); //Создаем два списка объектов класса Data (описан в Program.cs)
            List<Data> Letter = new List<Data>(); //Эти списки будут хранить таблицы статистики
            openFileDialog1.Title = "Выберите файл со статистикой"; //Задаем заголовок для диалога открытия файла
            openFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение файла
            openFileDialog1.FileName = "DefaultName"; //Задаем базовое имя файла
            openFileDialog1.Filter = "Text documents (.txt)|*.txt"; //Задаем фильтр
            openFileDialog1.ShowDialog(); //Выводим диалог на экран
            StreamReader Reader; //Создаем объект для чтения из файла
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл для чтения статистики
            }
            catch (FileNotFoundException) //В случае исключения
            {
                MessageBox.Show("Ошибка открытия файла статистики"); //Выводим сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            Reader.ReadLine(); //Считываем одну строку из файла со статистикой без записи в переменную
            while (!Reader.EndOfStream) //До конца файла
            {
                sub = Reader.ReadLine(); //Считываем подстроку из файла
                Letter.Add(new Data() { Letter = sub.Substring(0, 1), freq = double.Parse(sub.Substring(2)) }); //Выделяем из подстроки букву и частоту и записываем в первую таблицу
                Letter1.Add(new Data(){Letter = sub.Substring(0,1), freq=0.00}); //Выделяем из подстроки букву и записываем ее во вторую таблицу с нулевой частотой
            }
            
            Program.Count(text, Letter1); //Считаем частоты букв в шифротексте и записываем во вторую таблицу
            Reader.Close(); //Закрываем поток чтения из файла
            textBox2.Text = DecryptFunc(text, Letter1, Letter); //Вызываем функцию расшифровки и записываем возвращаемое значение в форму 2 (нижняя)

        }

       

        //
        //Взлом Шифра Цезаря по предпологаемому ключу, обработка нажатия на копку 4 ("Шифро Цезаря по ключу")
        //
        private void button4_Click(object sender, EventArgs e)
        {
            new CisarKey().ShowDialog(this); //Создаем и выводим форму для ввода индекса правильно расшифрованной буквы на экран
            int index = CisarKey.Index-1; //Записываем введенный пользователем индекс
            string Text = textBox1.Text.ToLower(); //Считываем шифротекст
            if (Text == String.Empty || textBox2.Text==String.Empty) //Если шифротекст пустой, или если расшифрованный текст пустой
            {
                MessageBox.Show("Поле textBox1 или textBox2 пусто. Оба поля должны быть заполнены."); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            string text1 = String.Empty; //Эта переменная хранит расшифрованный текст
            char ch1='a', ch2='a'; //ch1 - буква в шифротексте ch2- соответствующая буква в расшифрованном тексте
            try
            {
                ch1 = textBox1.Text.ToLower()[index];//В шифротексте находим букву с индексом index и записываем в одну переменную
                ch2 = textBox2.Text.ToLower()[index];//В расшифрованном тексте находим букву с индексом index и записываем в другую
            }
            catch (IndexOutOfRangeException) //В случае исключения 
            {
                MessageBox.Show("Буквы с таким номером не существует"); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            int char1_index = Array.IndexOf(Form1.Alf, ch1); //Находим номера букв в алфавите
            int char2_index = Array.IndexOf(Form1.Alf, ch2);
            int key = char1_index - char2_index; //Ключ получаем как их разность
            for (int i = 0; i < Text.Length; i++) //На основе полученного ключа, дешифруем текст
            {
                ch1 = Text[i]; //Записываем букву шифротекста
                if (Array.IndexOf(Form1.Alf, Text[i]) == -1) //Если данной буквы нет в используемом алфавите
                    text1 = text1 + Text[i]; //Она записывается в расшифрованный текст без изменений
                else //Иначе
                {
                    char1_index = Array.IndexOf(Form1.Alf, ch1); //Записываем номер буквы шифротекста в алфавите
                    if (char1_index - key > Form1.Alf.Length - 1) //Если разность номера и ключа больше размера алфавита
                        char1_index = char1_index - key - (Form1.Alf.Length - 1); //Находим индекс расшифрованной буквы по формуле
                    else //Иначе
                        if (char1_index - key < 0) //Если разность номера и ключа меньше нуля
                            char1_index = Form1.Alf.Length - (key - char1_index); //Применяем другую формулу
                        else //Если ни одно из условий не верно
                            char1_index = char1_index - key; // Как индекс расшифрованной буквы берем разность номера и ключа
                    text1 =text1 + Form1.Alf[char1_index]; //Добавляем нужную букву к расшифрованному тексту

                }
            }
            textBox2.Text = text1; //Выводим расшифрованный текст в форму 2 (нижняя)
        }

        //
        //Брутфорс Шифра Цезаря, обработка кнопки 5 ("Шифр Цезаря (brute)")
        //
        private void button5_Click(object sender, EventArgs e)
        {
            new TxtorDoc().ShowDialog();//Создаем и выводим диалоговое окно для выбора расширения сохраняемых файлов
            backgroundWorker1.RunWorkerAsync(); //Создаем второй поток для брутфорса
        }
        //Второй поток для брутфорса шифра Цезаря
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            char ch = ' '; //Вспомогательная переменная
            string format = TxtorDoc.format; //Записываем расширение, в котором будут сохранятся файлы

            string text = textBox1.Text.ToLower(); //Считывание зашифрованного текста
            try
            {
                ch = text[0]; //Пытаемся записать первую букву шифротекста
            }
            catch (IndexOutOfRangeException)
            {
                return; //В случае выхода индекса за допустимые пределы, передаем управление вызывающей функцие
            }
            string text1 = String.Empty; //Данная переменная хранит расшифрованный текст
            int key, k;
            key = new Random().Next(); //Случайный ключ используется для создания папки
            string path = "Brute" + key.ToString(); //Путь к папке с расшифровками
            Directory.CreateDirectory(path); //Создаем папку
            StreamWriter Writer; //Создаем объект для записи в папку
            path = path + "\\"; //Модифицируем путь, для использования его в роли пути для сохранения файлов
            for (int i = 0; i < Form1.Alf.Length; i++) //Перебираем все варианты
            {
                Writer = new StreamWriter(path + "Text" + i.ToString() + format, false, Encoding.Default); //Создаем файл для сохранения расшифровки
                k = Array.IndexOf(Form1.Alf, ch); //Индекс первой буквы шифротекста в алфавите
                key = k - i; //Считается ключ
                for (int j = 0; j < text.Length; j++) //Проход по тексту
                {
                    if (Array.IndexOf(Form1.Alf, text[j]) == -1) //Символы, не входящие в алфавит игнорируются
                    {
                        text1 = text1 + text[j];
                    }
                    else
                    {
                        k = Array.IndexOf(Form1.Alf, text[j]); //Получаем индекс каждой буквы текста в алфавите
                        if (k - key > Form1.Alf.Length - 1) //Если разность индекса буквы и ключа больше кол-ва букв в алфавите
                        {
                            k = k - key - (Form1.Alf.Length - 1); //Считаем индекс расшифрованной буквы по формуле
                        }
                        else
                        {
                            if (k - key < 0) //Если разность меньше нуля
                            {
                                k = Form1.Alf.Length - (key - k); //Меняем формулу
                            }
                            else
                            {
                                k = k - key; //Если ни одно из условий не выполнено, как индекс берем разность индекса и ключа
                            }
                        }
                        text1 = text1 + Form1.Alf[k]; //Добавляем к расшифровке нужную букву
                    }
                }
                Writer.Write(text1); //Записываем расшифрованный текст в файл
                text1 = String.Empty; //Обнуляем расшифрованный текст
                Writer.Close(); //И закрываем поток записи
            }
            MessageBox.Show("Расшифрованные тексты сохранены в папке " + path); //По окончании выводим сообщение о сохранении файлов
        }

        
        //
        //Вычисление индекса совпадения двух текстов, кнопка 6 ("Индекс совпадения")
        //
        private void button6_Click(object sender, EventArgs e)
        {
            bool check = false; //Служебная переменная
            int N1 = 0, N2 = 0; //N1-общее кол-во проверенных букв, N2-кол-во проверенных букв в одной строке текста
            double index1 = 0; //index1 - общая частота совпадений по тексту
            double index2 = 0; //index2 - частота совпадений в одной строке
            string text=String.Empty, text1=String.Empty; //Сами тексты
            StreamReader Reader1, Reader2; //Создаем два объекта для чтения текстов из файлов
            openFileDialog1.Title = "Выберите файл исходного текста"; //Задаем заголовок диалога открытия файла
            openFileDialog1.DefaultExt = "(.txt)"; //Задаем базовое расширение файла
            openFileDialog1.FileName = "Default"; //Задаем базовое имя файла
            openFileDialog1.Filter = "Text documents (.txt) |*.txt"; //Задаем фильтр
            openFileDialog1.ShowDialog(); //Выводим диалог для открытия файла
            try
            {
                Reader1 = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем первый файл для чтения
            }
            catch(FileNotFoundException)//В случаее возникновения исключения
            {
                MessageBox.Show("Ошибка открытия файла с исходником"); //Выводим соответствуюещее сообщение
                return;//И возвращаем управление вызывающей функцие
            }
            openFileDialog1.Title = "Выберите файл с расшифровкой"; //Меняем заголовок диалогового окна
            openFileDialog1.ShowDialog(); //Выводим далог открытия файла
            try
            {
                Reader2 = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем второй файл для чтения
            }
            catch (FileNotFoundException) //В случае исключения
            {
                MessageBox.Show("Ошибка открытия файла с расшифровкой"); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            try
            {
                while (!Reader1.EndOfStream || !Reader2.EndOfStream) //Читаем пока один из потоков не закончитя
                {
                    check = false; //Значение проверки изначально ложно
                    text = Reader1.ReadLine().ToLower(); //Считываем строку из первого файла
                    text1 = Reader2.ReadLine().ToLower(); //Счтиываем строку из второго файла
                    for (int i = 0; i < text.Length; i++) //Цикл проверки строки
                    {
                        if (Array.IndexOf(Form1.Alf, text[i]) != -1) //Если в первой строке есть хотя бы одна буква из используемого алфавита
                        {
                            check = true; //Значение проверки меняем на истину
                            break; //И прерываем цикл проверки
                        }
                    }
                    //Вторую строку не проверяем, предпологается что второй текст - это расшифрованный первый текст. В таком
                    //случае если в исходном тексте в данной строке есть хоть одна буква, то и в расшифрованном тексте тоже есть буква.
                    if (check == true) //Если проверка верна
                    {
                        index2 = 0; //Кол-во совпадений
                        N2 = 0; //Количество сравниваемых букв
                        try
                        {
                            for (int i = 0; i < text.Length && i < text1.Length; i++) //Цикл сравнения
                            {
                                if (Array.IndexOf(Form1.Alf, text[i]) == -1 || Array.IndexOf(Form1.Alf, text1[i]) == -1) //Если i-го символа какой-либо строки нет в алфавите
                                {
                                    continue; //Пропускаем данную итерацию
                                }
                                else //Иначе
                                    N2++; //Увеличиваем кол-во сравниваемых букв на 1
                                if (text[i] == text1[i]) //Если буквы в двух строках совпадают
                                {
                                    index2++; //Кол-во совпадений увеличиваем на 1
                                }
                            }
                        }
                        catch (NullReferenceException) //Данное исключение возникает, когда мы обращаемся к несуществующему объекту
                        {
                            MessageBox.Show("Количество символов в файлах не совпадает"); //Выводим сообщение, что кол-во символов в файлах не совпадает
                            return; //И возвращаем управление вызывающей функцие
                        }
                        if (index2 != 0 && N2 != 0) //Если кол-во совпадений и кол-во проверенных букв не равно нулю
                            index2 = index2 / N2; //Считаем частоту совпадений
                        if (index1 == 0 && index2 == 0 && N2 == 0 && N1 == 0) //Если все переменные равны нулю
                        {
                            index1 = 0; //Общую частоту совпадений приравниваем нулю
                        }
                        else //В любом другом случае
                        {
                            index1 = (index1 * N1 + index2 * N2) / (N1 + N2); //Общую частоту совпадений считаем по формуле
                        }
                        N1 += N2; //К общему кол-ву проверенных букв добавляем кол-во проверенных букв в данной строке
                    }
                }
            }
            catch (NullReferenceException) //При возникновении исключения
            {
                MessageBox.Show("Количество символов в файлах не совпадает."); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            textBox2.Text = index1.ToString(); //В конце вычислений выводим общую частоту совпадений в форму 2 (нижняя)
        }
        //
        //Расшифровка с файла, кнопка 7 ("Расшифровать с файла")
        //
        private void button7_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(BackThread); //Создаем поток для выполнения
            thread.SetApartmentState(ApartmentState.STA); //Задаем параметры потока
            thread.Start(); //Запускаем поток

        }
        //Поток для расшифровки из файла
        private void BackThread()
        {

            StreamWriter Writer; //Создаем объект для записи в файл
            bool check; //Переменная для проверки
            string sub; //Подстрока
            string text = String.Empty; //Шифротекст
            int N1 = 0, N2 = 0; //Кол-во просчитанных букв
            List<Data> Letter1 = new List<Data>(); //Letter1 - хранит статистику по всему шифротексту
            List<Data> Letter = new List<Data>();//Letter - хранит статистику из файла
            List<Data> Letter2 = new List<Data>();//Letter2 - хранит статистику по одной строке шифротекста
            openFileDialog1.Title = "Выберите файл со статистикой"; //Задаем заголовок диалога открытия файла
            openFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение
            openFileDialog1.FileName = "DefaultName"; //Задаем базовое имя файла
            openFileDialog1.Filter = "Text documents (.txt)|*.txt"; //Задем фильтр
            openFileDialog1.ShowDialog(); //Выводим диалог на экран
            StreamReader Reader; //Создаем объект для чтения из файла
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл со статистикойдля чтения
            }
            catch (FileNotFoundException) //При возникновении исключения
            {
                MessageBox.Show("Ошибка открытия файла статистики"); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            Reader.ReadLine(); //Читаем одну строку без записи
            while (!Reader.EndOfStream) //Читаем до конца файла статистики
            {
                sub = Reader.ReadLine(); //Считываем одну строку из таблицы статистики
                Letter.Add(new Data() { Letter = sub.Substring(0, 1), freq = double.Parse(sub.Substring(2)) }); //Из подстроки выделяем букву и частоту
                Letter1.Add(new Data() { Letter = sub.Substring(0, 1), freq = 0.00 }); //В остальные таблицы записываем букву с нулевой частотой
                Letter2.Add(new Data() { Letter = sub.Substring(0, 1), freq = 0.00 });
            }
            Reader.Close(); //Закрываем поток чтения из файла
            //
            //Считаем статистику по файлу с шифротекстом
            //
            openFileDialog1.Title = "Выберите файл с шифротекстом"; //Меняем заголовок открытия файла
            openFileDialog1.ShowDialog(); //Выводим диалог открытия файла
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл с шифротекстом для чтения
            }
            catch (FileLoadException) //В случае исключения
            {
                MessageBox.Show("Ошибка открытия файла с шифротекстом"); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            while (!Reader.EndOfStream) //Читаем до конца файла
            {
                check = false; //Изначально проверка ложна
                text = Reader.ReadLine().ToLower(); //Считывается одна строка шифротекста                
                for (int i = 0; i < text.Length; i++) //Проверяем строку на наличие букв
                {
                    if (Array.IndexOf(Form1.Alf, text[i]) != -1) //Если в строке находится хотя бы одна буква из используемого алфавита
                    { 
                        check = true; //Значение проверки приравниаем истине
                        break; //И прерываем цикл
                    }
                }
                if (check == true) //Если проверка имеет значение истина
                {
                    N2 = Program.Count(text, Letter2); //Считаем частоту букв в строке шифротекста и записываем кол-во букв в ней
                    for (int i = 0; i < Letter.Count; i++)
                    {
                        try
                        {
                            Letter1[i].freq = (Letter2[i].freq * N2 + Letter1[i].freq * N1) / (N1 + N2); //Пересчитываем общую частоту каждой буквы по шифротексту с помощью формулы
                        }
                        catch (DivideByZeroException) //Если возникает деление на ноль
                        {
                            Letter1[i].freq = (Letter2[i].freq * N2 + Letter1[i].freq * N1) / 1; //Меняем формулу
                        }
                        Letter2[i].freq = 0.00; //Зануляем частоты букв в одной строке
                    }
                    N1 = N1 + N2; //К кол-ву всех проссчитанных  букв добавляем кол-во букв в одной строке
                }
            }
            Reader.Close(); //Закрываем поток чтения
            Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем тот же файл для чтения
            //
            //Расшифровываем
            //
            saveFileDialog1.Title = "Сохранить расшифрованный текст как"; //Задаем заголовок диалога сохранения файла
            saveFileDialog1.DefaultExt = "(.txt)"; //Задаем базовое расширение файла
            saveFileDialog1.Filter = "Text document (.txt) |*.txt"; //Задаем фильтр
            saveFileDialog1.ShowDialog(); //Выводим диалог сохранения файла
            try
            {
                Writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default); //Открываем файл для записи
            }
            catch (FileNotFoundException) //В случае исключения
            {
                MessageBox.Show("Ошибка создания файла"); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            while (!Reader.EndOfStream) //Читаем файл до конца
            {
                check = false; //Изначально проверка ложна
                text = Reader.ReadLine().ToLower(); //Считывается одна строка шифротекста
                for (int i = 0; i < text.Length; i++) //Цикл проверки строки на наличие букв
                {
                    if (Array.IndexOf(Form1.Alf, text[i]) != -1) //Если в строке шифротекста есть хотя бы одна буква из используемого алфавита
                    {
                        check = true; //Значение проверки меняем на истину
                        break; //И прерываем цикл
                    }
                }
                if (check == true) //Если значение проверки истинно
                {
                    Writer.WriteLine(DecryptFunc(text, Letter1, Letter)); //Вызываем функцию расшифровки, и возвращаемое ей значение записываем в файл
                }
                else
                {
                    Writer.WriteLine(text); //Если проверка ложна, записываем в файл строку без расшифровки
                }
            }
            Writer.Close(); //Закрываем поток записи
            MessageBox.Show("Расшифровка завершена"); //В конце выводим сообщение об успешной расшифровке
        }

        //
        //Смена используемого алфавита, обработка нажатия на кнопку 8 ("Сменить алфавит")
        //
        private void button8_Click(object sender, EventArgs e)
        {
            StreamReader Reader; //Создаем объект для чтения из файла
            openFileDialog1.Title = "Выберите файл с алфавитом"; //Задаем заголовок диалога открыитя файла
            openFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение файла
            openFileDialog1.Filter = "Text documents(.txt)| *.txt"; //Задаем заголовок
            openFileDialog1.ShowDialog(); //Выводим диалог открытия
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл для чтения
            }
            catch (FileNotFoundException) //В случае исключения
            {
                MessageBox.Show("Ошибка открытия файла с алфавитом"); //Выводим соответствующее сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            string alfavite = Reader.ReadLine(); //Считываем алфавит
            Form1.Alf = new char[alfavite.Length]; //Выделяем память под новый алфавит
            for (int i = 0; i < alfavite.Length; i++)
            {
                Form1.Alf[i] = alfavite[i]; //Каждый считанный символ записываем как букву нового алфавита
            }
            label1.Text = "Используемый алфафит:\n" + alfavite; //Меняем метку
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Info().Show();
        }
        }
    }
