using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Шифр_простой_замены
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
        }
        private void Statistics_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void Statistics_Shown(object sender, EventArgs e)
        {
        }

        //Обработка закрытия формы
        private void Statistics_FormClosed(object sender, FormClosedEventArgs e)
        {
                Application.Exit(); //Приложение закрывается
        }

        //Обработака нажатия на кнопку 4 ("В главное меню")
        private void button4_Click(object sender, EventArgs e)
        {
            new Form1().Show(); //Создаем и выводим на экран форму главного меню
            this.Hide(); //А данную форму скрываем
        }

        //Обработка нажатия на кнопку 5 ("Выйти")
        private void button5_Click(object sender, EventArgs e)
        {
                Application.Exit();
        }
        //
        //Создание новой таблицы статистики (Кнопка 1 - "Новая таблица")
        //
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Выберите файл с алфавитом"; //Задаем заголовок диалогового окна открытия файла
            openFileDialog1.DefaultExt = ".txt"; //Задаем базовое расшиерние файла
            openFileDialog1.FileName = "DefaultName"; //Задаем базовое имя файла
            openFileDialog1.Filter = "(.txt)|*.txt"; //Задаем фильтр
            openFileDialog1.ShowDialog(); //Выводим диалог на экран
            string alf = String.Empty; //В данной переменной будет хранится алфавит
            try
            {
                alf = System.IO.File.ReadAllText(openFileDialog1.FileName, Encoding.Default); //Пытаемся считать текст из файла
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка при открытии файла"); //При ошбике чтения выводим соответствующее сообщение
                return; //И возвращаем управление приложением вызывающей функцие
            }
            textBox1.Text = alf; //Считанный с файла алфавит выводим в форму для текста
            //List<Data> Letter = new List<Data>(); //Создаем список объектов класса Data (описан в Programm.cs)
            saveFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение файла для диалога сохранения файла
            saveFileDialog1.FileName = "Table"; //Задаем базовое имя файла
            saveFileDialog1.Filter = "Text documents (.txt)|*.txt"; //Задаем фильтр
            saveFileDialog1.ShowDialog(); //Выводим диалог на экран
            StreamWriter sw; //Создаем объект для записи в файл
            sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default);  //Открываем файл для записи
            sw.WriteLine('0'); //Записываем в файл начальное значение кол-ва символов, с которых собрана статистика
            for (int i = 0; i < alf.Length; i++) //Каждый символ алфавита записываем в файл с частотой равной нулю
                    sw.WriteLine(alf[i].ToString() + ' ' + (double)0); //Формат записи: "буква частота"
            sw.Close(); //Закрываем файл для записи
        }
        //
        //Изменение статистики (Кнопка 2 - "Внести изменения")
        //
        private void button2_Click(object sender, EventArgs e)
        {
            int N1,N2; //Данные переменные хранят кол-во символов, с которых собрана статистика
            StreamReader Reader; //Создаем объект для чтения из файла
            string text = textBox1.Text.ToLower(), s; //Считываем весь текст из формы в переменную text
            List<Data> Letter = new List<Data>(); //Создаем два списка объектов Data (описан в Program.cs)
            List<Data> Letter1 = new List<Data>();
            openFileDialog1.Title = "Выберите файл со статистикой"; //Задаем заголовок для окна открытия файла
            openFileDialog1.DefaultExt = ".txt"; // Задаем базовое расширения файла
            openFileDialog1.FileName = "DefaultName"; //Задаем базовое имя файла
            openFileDialog1.Filter = "Text documents(.txt)|*.txt"; //Задаем фильтр
            openFileDialog1.ShowDialog(); //Выводим диалоговое окно на экран
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем таблицу статистики для чтения
                N1=int.Parse(Reader.ReadLine()); //Считываем кол-во символов
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка открытия файла со статистикой"); //Если возникла ошибка открытия или чтения из файла выводится сообщение
                return; // И управление передается вызывающей функцие
            }
            while (!Reader.EndOfStream) //До конца потока чтения, т.е. до конца файла
            { 
                s = Reader.ReadLine(); //Считываем подстроку, имеющую формат "буква частота"
                Letter.Add(new Data() { Letter = s.Substring(0, 1), freq = double.Parse(s.Substring(2)) }); //Добавляем в первый список объектов букву с частотой
                Letter1.Add(new Data() { Letter = s.Substring(0, 1), freq = 0.00 }); //Во второй - букву с нулевой частотой
            }
            Reader.Close(); //Закрываем файл для чтения

            StreamWriter Writer = new StreamWriter(openFileDialog1.FileName, false, Encoding.Default); //Создаем объект для записи в файл
            N2=Program.Count(text, Letter1); //Вызываем функцию Count (описана в Program.cs) для подсчета частоты появления букв в тексте
            //в N2 записывается кол-во букв в тексте
            Writer.WriteLine((N1+N2).ToString()); //Записываем в файл сумму N1+N2 как кол-во букв, с которых собрана статистика
            for (int i = 0; i < Letter.Count; i++) //Идем по списку объектов
            {
                try
                {
                    Letter[i].freq = (Letter1[i].freq * N2 + Letter[i].freq * N1) / (N1 + N2); //В частоту i-й буквы в первом списке записываем новое значение, посчитанное по формуле
                }
                catch (DivideByZeroException)
                {
                    Letter[i].freq = (Letter1[i].freq * N2 + Letter[i].freq * N1) / 1; //При делении на ноль меняем формулу
                }
                Writer.WriteLine(Letter[i].Letter + ' ' + Letter[i].freq.ToString()); //Записываем обновленные данные из первого списка объктов в файл
            }
            Writer.Close(); //Закрываем файл для записи
            textBox1.Text =(N1+N2).ToString(); //Выводим в форму для текста итоговое кол-во букв, с которых собрана статистика
        }
        //
        //Очищение статистики (Кнопка 3 - "Очистить статистику")
        //
        private void button3_Click(object sender, EventArgs e)
        {
            string sub; //Переменная для хранения подстроки
            openFileDialog1.Title = "Выберите файл статистики"; //Задаем заголовок диалога открытия файла
            openFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение файла
            openFileDialog1.Filter = "Text documents (.txt)|*.txt"; //Задаем фильтр
            openFileDialog1.ShowDialog(); //Открываем диалог
            StreamReader Reader; //Создаем объект для чтения из файла
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл для записи
            }
            catch(FileNotFoundException)
            {
                MessageBox.Show("Ошибка открытия файла статистики"); //В случае ошибки открытия файла выводим сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            List<Data> Letter = new List<Data>(); //Создаем список объектов Data(описан в Program.cs)
            Reader.ReadLine(); //Считываем строку без записи, чтобы пропустить число и перейти к таблице
            while (!Reader.EndOfStream) //Идем до конца файла
            {
                sub = Reader.ReadLine(); //Считываем подстрокку
                Letter.Add(new Data() { Letter = sub.Substring(0, 1), freq = double.Parse(sub.Substring(2)) }); //Из подстроки выделяем букву и ее частоту и записываем в список объектов
            }
            Reader.Close(); //Закрываем файл для чтения
            StreamWriter Writer = new StreamWriter(openFileDialog1.FileName, false, Encoding.Default); //Открываем файл для записи
            Writer.WriteLine("0"); //В кол-во проверенных букв записываем ноль
            for (int i = 0; i < Letter.Count; i++)
            {
                Writer.WriteLine(Letter[i].Letter + ' ' + (0.00).ToString()); //Пишем в файл новую таблицу "буква частота" с нулевыми частотами
            }
            Writer.Close(); //Закрываем файл для записи
        }

        //
        //Внесение изменений на основе текста из файла (Кнопка 6 - "Текст из файла")
        //
        private void button6_Click(object sender, EventArgs e)
        {
            bool check; //Служебная переменная
            int N0, N1=0, N2=0; //Хранят кол-во символов, с которых собрана статистика
            StreamReader Reader; //Создаем объект для чтения из файла
            StreamWriter Writer;//И объект для записи в файл
            string text =String.Empty, s;//text - переменная с текстом, s-вспомогательная переменная для чтения статистики из файла
            List<Data> Letter = new List<Data>(); //Letter - хранит статистику из файла
            List<Data> Letter1 = new List<Data>(); //Letter1 хранит статистику из текста
            List<Data> Letter2 = new List<Data>(); //Letter 2 вспомогательный массив, хранит статистику из одной строки файла
            openFileDialog1.Title = "Выберите файл со статистикой"; //Задаем заголовок для диалога открытия файла
            openFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение файла
            openFileDialog1.FileName = "DefaultName"; //Задаем базовое имя файла
            openFileDialog1.Filter = "Text documents(.txt)|*.txt"; //Задаем фильтр
            openFileDialog1.ShowDialog(); //Выводим диалог на экран
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл со статистикой для чтения
                try
                {
                    N0 = int.Parse(Reader.ReadLine()); //Считываем кол-во символов, с которых статистика уже собрана
                }
                catch (FormatException) 
                {
                    MessageBox.Show("Неверный формат файла статистики"); //В случае ошибки чтения выводим сообщение
                    return; //И передаем управление вызывающей функцие
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка открытия файла со статистикой"); //В случае ошибки открытия файла выводим сообщение
                return;//И передаем управление вызывающей функцие
            }
            while (!Reader.EndOfStream) //Идем до конца файла со статистикой
            {
                s = Reader.ReadLine(); //Считываем подстроку
                Letter.Add(new Data() { Letter = s.Substring(0, 1), freq = double.Parse(s.Substring(2)) }); //В первый список записываем букву с частотой
                Letter1.Add(new Data() { Letter = s.Substring(0, 1), freq = 0.00 }); //Во второй и третий списки записываем букву с нулевой частотой
                Letter2.Add(new Data() { Letter = s.Substring(0, 1), freq = 0.00 });
            }
            Reader.Close(); //Закрываем файл для чтения
            Writer = new StreamWriter(openFileDialog1.FileName, false, Encoding.Default); //Открываем этот же файл для записи
            //
            //
            //
            openFileDialog1.Title = "Выберите файл с текстом"; //Заголовок диалога открытия файла
            openFileDialog1.ShowDialog(); //Выводим диалогове окно
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл с текстом для чтения
            }
            catch
            {
                MessageBox.Show("Ошибка открытия файла с текстом"); //В случае ошибки файла выводим сообщение
                return; //И передаем управление вызывающей функцие
            }
            while (!Reader.EndOfStream) //Идем до конца файла
            {
                check = false; //Изначально проверка дает ложный результат
                text = Reader.ReadLine().ToLower(); //В переменную text записываем одну строку текста из файла
                for (int i = 0; i < text.Length; i++) //Идем по строке
                {
                    if (Array.IndexOf(Form1.Alf, text[i]) != -1) //Если попадается хоть одна буква
                    {
                        check = true; //Меняем значение проверки на истину
                        break; //И прерываем цикл
                    }
                }
                if (check == true) //Если проверка истинна, т.е. если в строке есть хотя бы одна буква из используемого алфавита
                {
                    N2 = Program.Count(text, Letter2); //Считаем в строке частоты букв с помощью функции Count (описана в Program.cs)
                    for (int i = 0; i < Letter.Count; i++) //Идем по списку объектов (не имеет значения по какому из трех, они имеют одинаковый размер)
                    {
                        try
                        {
                            Letter1[i].freq = (Letter2[i].freq * N2 + Letter1[i].freq * N1) / (N1 + N2); //Считаем и записываем новое значение частоты для каждой
                            // i-ой буквы из списка, получая обновленную статистику по всему тексту из файла
                        }
                        catch (DivideByZeroException)
                        {
                            Letter1[i].freq = (Letter2[i].freq * N2 + Letter1[i].freq * N1); //В случае деления на ноль меняем формулу расчета
                        }
                        Letter2[i].freq = 0.00; //Для i-й буквы в списке 3, хранящим статистику по строке, снова задаем частоту ноль
                    }
                    N1 = N1 + N2; //К кол-ву букв в файле добавляем кол-во букв в просчитанной строке
                }
            }
            Reader.Close();//Закрываем файл для чтения                 
            //Так, в списке Letter 1 теперь хранится полная статистика по файлу
            //В следующем блоке мы объединяем статистику из файла со статистикой по тексту
            //            
            Writer.WriteLine((N1 + N0).ToString()); //Кол-во букв соответствует сумме кол-ва букв в тексте с кол-вом букв, которые уже были просчтианы
            for (int i = 0; i < Letter.Count; i++)
            {
                try
                {
                    Letter[i].freq = (Letter1[i].freq * N1 + Letter[i].freq * N0) / (N0 + N1); //Для каждой i-ой буквы в исходной статистике из файла меняем частоту по формуле
                }
                catch (DivideByZeroException)
                {
                    Letter[i].freq = (Letter1[i].freq * N1 + Letter[i].freq * N0) / 1; //При делении на ноль частота меняется
                }
                Writer.WriteLine(Letter[i].Letter + ' ' + Letter[i].freq.ToString()); //Записываем в файл обновленные данные
            }
            Writer.Close(); //Закрываем файл для записи
            textBox1.Text = (N0 + N1).ToString(); //В форму для текста выводим итоговое кол-во букв, с которых собрана статистика
            Reader.Close(); //Закрываем файл для чтения
        }
        //
        //Объединение двух статистик в одну (Кнопка 7 - "Объединить статистики")
        //
        private void button7_Click(object sender, EventArgs e)
        {
            string s,s1; //Вспомогательные переменные
            int N1, N2, N3; //Данные переменные хранят кол-ва букв, с которых собрана статистика
            StreamWriter Writer; //Создаем объект для записи в файл
            StreamReader Reader1, Reader2; //Создаем два объекта для чтения из двух разных файлов со статистикой
            openFileDialog1.Title = "Окрыть статистику 1"; //Задаем заголовок диалога открыитя файла
            openFileDialog1.DefaultExt = ".txt"; // Задаем базовое расширение 
            openFileDialog1.Filter = "Text documents (.txt)|*.txt"; // Задаем фильтр
            openFileDialog1.ShowDialog(); //Выводим диалогове окно на экран
            try
            {
                Reader1 = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Откываем файл для чтения
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка открытия файла статистики"); //При ошибке открытия выводим сообщение
                return; //И передаем управление вызывающей функцие
            }
            openFileDialog1.Title = ("Открыть статистику 2"); //Задаем заголовок
            openFileDialog1.ShowDialog();//Выводим диалогове окно открытия файла
            try
            {
                Reader2 = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл для чтения
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка открытия файла статистики"); //При ошибке открытия выводим сообщение
                return; //И передаем управление вызывающей функцие
            }
            N1 = int.Parse(Reader1.ReadLine()); //Считываем кол-во символов в первой статистике
            N2 = int.Parse(Reader2.ReadLine()); //Считываем кол-во символов во второй статистике
            N3 = N1 + N2; //Объединяем иъ

            List<Data> Letter1=new List<Data>(); //Создаем три списка объектов для записи статистики
            List<Data> Letter2=new List<Data>();
            List<Data> Letter3=new List<Data>();
            while (!Reader1.EndOfStream) //Читаем до конца файла
            {
                s = Reader1.ReadLine(); //Считываем первую подстроку
                s1 = Reader2.ReadLine(); //Считываем вторую подстроку
                Letter1.Add(new Data{Letter = s.Substring(0,1), freq = double.Parse(s.Substring(2))}); //Из первой подстроки выделяем букву и частоту из первой статистики
                Letter2.Add(new Data { Letter = s1.Substring(0, 1), freq = double.Parse(s1.Substring(2)) }); //Из второй подстроки выделяем букву и частоту из втрой статистики
                Letter3.Add(new Data { Letter = s.Substring(0, 1), freq = 0.00 }); //В третью статистику сначала записываем нулевые частоты для букв
            }

            saveFileDialog1.Title = "Сохранить файл как"; //Задаем заголовок диалога для сохранения файла
            saveFileDialog1.Filter = "Text documents (.txt)|*.txt"; //Задаем фильтр
            saveFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение
            saveFileDialog1.ShowDialog(); //Выводим диалог сохранения файла
            try
            {
                Writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default); //Открываем файл для записи
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка сохранения файла"); //В случае ошибки выводим сообщение
                return; //И передаем управление вызывающей функцие
            }
            Writer.WriteLine(N3.ToString()); //В файл записываем число букв
            for (int i = 0; i < Letter3.Count; i++)
            {
                Letter3[i].freq = (Letter1[i].freq * N1 + Letter2[i].freq * N2) / N3; //Новые частоты считаем по формуле
                Writer.WriteLine(Letter3[i].Letter + ' ' + Letter3[i].freq.ToString()); //Обновленные данные записываем в файл
            }
            Writer.Close(); //Закрываем файл для записи
            Reader1.Close(); //Закрываем файл для чтения
            Reader2.Close(); //Закрываем файл для чтения
        }

        //
        //Смена используемого алфавита (Кнопка 8 - "Сменить алфавит")
        //
        private void button8_Click(object sender, EventArgs e)
        {
            StreamReader Reader; //Создаем объект для чтения из файла
            openFileDialog1.Title = "Выберите файл с алфавитом"; //Задаем заголовок диалога открытия
            openFileDialog1.DefaultExt = ".txt"; //Задаем базовое расширение файла
            openFileDialog1.Filter = "Text documents(.txt)| *.txt"; //Задаем фильтр
            openFileDialog1.ShowDialog(); //Выводим на экран диалог открытия
            try
            {
                Reader = new StreamReader(openFileDialog1.FileName, Encoding.Default); //Открываем файл для чтения
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка открытия файла с алфавитом"); //При ошибке выводим сообщение
                return; //И возвращаем управление вызывающей функцие
            }
            string alfavite = Reader.ReadLine(); //Считываем алфавит из файла
            Form1.Alf = new char[alfavite.Length]; //Выделяем память под новый массив
            for (int i = 0; i < alfavite.Length; i++)
            {
                Form1.Alf[i] = alfavite[i]; //Каждый символ строки записываем как букву алфавита
            }
            label1.Text = "Используемый алфафит:\n" + alfavite; //Выводим обновленный алфавит на форму в формате метки
        }
        //
        //Обработка загрузки формы
        //
        private void Statistics_Load(object sender, EventArgs e)
        {
            string alf=String.Empty;
            for (int i = 0; i < Form1.Alf.Length; i++)
                alf = alf + Form1.Alf[i];
            label1.Text = "Используемый алфавит:\n" + alf; //Выводим на форму используемый алфавит

            foreach (var button in this.Controls) //Для каждого объекта формы из списка Controls
            {
                if (button is Button) //Если объект имеет класс Button
                {
                    ((Button)button).MouseEnter += Program.button_Enter; //Задаем обработку наведения курсора на кнопку
                    ((Button)button).MouseLeave += Program.button_Leave; //И обработку убирания курсора с кнопки
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Info().Show();
        }

        }

        
    }