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
   

    public partial class Form1 : Form
    { 

        private void Form1_Shown(object sender, EventArgs e)
        {

        }
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
        }

        public static char[] Alf; //Массив сиволов, содержит в себе текущий алфавит

        //Обработка нажатия кнопки 1 ("Статистика")
        private void button1_Click(object sender, EventArgs e)
        {
            new Statistics().Show(); //Создается форма Statistics и выводится на экран
            this.Hide(); //А эта форма скрывается
        }

        //Обработка нажатия кнопки 2 ("Расшифровать")
        private void button2_Click(object sender, EventArgs e)
        {
            new Decrypt().Show(); //Создается форма Decrypt и выводится на экран
            this.Hide(); //А эта форма скрывается
        }

        //Обработка нажатия кнопки 3 ("Выход")
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Приложение закрывается
        }

        //Обработка закрытия формы
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(Application.OpenForms.Count==0)
                Application.Exit(); //Приложение закрывается
        }

        //Обработка загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader Reader; //Создаем объект Reader для чтения из файла
            try
            {
                Reader = new StreamReader("Alfavites\\rus.txt", Encoding.Default); //Пробуем открыть файл с алфавитом
            }
            catch (FileLoadException) //В случае исключения
            {
                MessageBox.Show("Ошибка открытия файла");//Выводим на экран соответствующее сообщение
                return;
            }
            string text = Reader.ReadLine(); //Данная переменная содержит считанную из файла строку текста
            Alf = new char[text.Length]; //Выделяем память под алфавит, соответсвующий длине считанной строки
            for (int i = 0; i < text.Length; i++)
                Alf[i] = text[i]; //Каждый символ строки записывается в алфавит
            label1.Text = text; //Алфавит выводится на форму в формате метки, это тот массив, с которым в данный момент работает программа

            foreach (var button in this.Controls) //Для каждого объекта из группы Controls данной формы
            {
                if (button is Button) //Если этот объект является объектом класса Button
                {
                    ((Button)button).MouseEnter += Program.button_Enter; //Задаем обработку наведения курсора на кнопку
                    ((Button)button).MouseLeave += Program.button_Leave; //Задаем обработку убирания курсора с кнопки
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Info().Show();
        }


    }
}
