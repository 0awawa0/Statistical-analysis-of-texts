using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Шифр_простой_замены
{
    public partial class TxtorDoc : Form
    {
        public static string format=".txt"; //Статическая переменная для хранения выбранного формата
        public TxtorDoc()
        {
            InitializeComponent();
        }

        //
        //Обработка нажатия на кнопку 1 ("Ок")
        //
        private void button1_Click(object sender, EventArgs e) 
        {
            if (radioButton1.Checked == true) //Если отмечена радио-кнопка 1
                format = ".doc"; //Расширение становится .doc
            if (radioButton2.Checked == true) //Если отмечена радио-кнопка 2
                format = ".txt"; //Расширение становится .txt
            if (radioButton3.Checked == true) //Если отмечена радио-кнопка 3
            {
                try
                {
                    format = textBox1.Text; //Пробуем установить расширение, введенное пользователем
                }
                catch (FormatException) //При несовпадении форматов
                {
                    MessageBox.Show("Неверное расширение файла. Ввдетие расширение в виде '.txt'."); //Выводим соответствующее сообщение
                    return; //И передаем управление вызывающей функцие
                }
                if (textBox1.Text == String.Empty) //Если форма для ввода пустая
                {
                    MessageBox.Show("Введите ваше расширение в форму ниже"); //Выводим сообщение
                    return; //И возвращаем управление вызывающей функцие
                }
                if (format[0] != '.') //Если введенный формат не начинается с точки
                {
                    MessageBox.Show("Неверное расширение файла. Ввдетие расширение в виде '.txt'."); //Выводим соответствующее сообщение
                    return; //И возвращаем управление вызывающей функцие
                }
            }
            this.Close(); //Закрываем форму
        }

        private void TxtorDoc_Load(object sender, EventArgs e)
        {

        }

        //
        //Обработка наведения курсора на кнопку
        //
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.ForestGreen; //При наведении курсора на кнопку меняем цвет фона на зеленый
        }

        //
        //Обработка убирания курсора с кнопки
        //
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Crimson; //При убирании курсора с кнопки меняем цвет фона на красный
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Info().Show();
        }
    }
}
