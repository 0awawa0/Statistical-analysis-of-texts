using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Шифр_простой_замены
{
    public class Data //Класс Data используется для хранения таблиц статистики частот букв
    {
        //Каждый экземпляр класс хранит одну букву и ее частоту как число с плавающей точкой с двойной точностью
        public string Letter { get; set; } // Буква
        public double freq { get; set; } //Частота
    }

    static class Program
    {
        //
        //Процедура обработки наведения курсора на кнопку
        //  Описывается в главном модуле, чтобы не писать для каждой кнопки в отдельности
        public static void button_Enter(object sender, EventArgs e) 
        {
            ((Button)sender).BackColor = Color.ForestGreen; //При наведении курсора на кнопку меняем цвет фона на зеленый
        }

        //
        //Процедура обработки убирания курсора с кнопки
        // 
        public static void button_Leave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Crimson; //При убирании курсора с кнопки меняем цвет фона на красный
        }

        //
        //Функция подсчета частоты букв в тексте
        //  Описывается в главном модуле, чтобы ее можно было свободно использовать во всех формах программы
        //  Функция принимает два параметра: текст и список объектов класса Data хранящий частоты букв текста.
        //  Список Letter будет изменятся во время работы функции. Текст изменятся не будет
        public static int Count(string text, List<Data> Letter)
        {
            int count = 0, i = 0; //count - кол-во букв
            bool check = true;
            for (i = 0; i < text.Length; i++)
            {
                if (Array.IndexOf(Form1.Alf, text[i]) == -1) //Если i-го символа текста нет в используемом алфавите
                    check = false; //Значение проверки становится ложным
                else //Иначе
                    check = true; //Значение проверки становится истиной
                if (check == true) //Если проверка истинна
                    for (int j = 0; j < Letter.Count; j++)
                    {
                        string letter = text[i].ToString(); //Записываем i-ю букву текста
                        if (letter == Letter[j].Letter) //Если эта буква равна j-ой букве в списке
                        {
                            Letter[j].freq++; //Частота j-ой буквы увеличивается на 1
                            count++; //Кол-во букв увеличивается на 1
                        }
                    }
            }
            if (count == 0) //Если кол-во букв в итоге равно 0
                return 0; //Возвращаем 0
            else //Иначе
            {
                for (i = 0; i < Letter.Count; i++)
                    Letter[i].freq /= count; //Делим частоту каждой буквы в списке на кол-во букв
                return count; //Возвращаем кол-во букв
            }
        }
       
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThreadAttribute]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
