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
    public partial class CisarKey : Form
    {
        public static int Index;
        
        public CisarKey()
        {
            InitializeComponent();
        }

        //
        //Обработка нажатия на кнопку 1 ("Ок")
        //
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Index = int.Parse(textBox1.Text); //Пытаемся считать индекс
            }
            catch (FormatException) //Если введено не целое число, либо вовсе не число
            {
                Index = 1; //Задаем индекс как 1
            }
            this.Close(); //Закрываем форму
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
            button1.BackColor = Color.Crimson; //При убирании курсора с кнопки меняем цвет фона обратно на красный
        }

        private void CisarKey_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Info().Show();
        }
    }
}
